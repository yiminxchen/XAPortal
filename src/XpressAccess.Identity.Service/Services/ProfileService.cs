using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XpressAccess.Identity.EFStore;
using XpressAccess.Identity.Manager;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Extensions;
using System.Security.Claims;
using IdentityModel;

namespace XpressAccess.Identity.Service
{
    /// <summary>
    /// Connect identity user and the profle store
    /// </summary>
    public class ProfileService : IProfileService
    {
        private readonly IdentityUserManager<IdentityUser> _userManager;

        public ProfileService(IdentityUserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        /// <summary>
        /// This method is called whenever claims about the user are requested (e.g. during token creation or via the userinfo endpoint)
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var user = await _userManager.FindbyIdAsync(context.Subject.GetSubjectId());
            var roleList = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>
            {
                new Claim(JwtClaimTypes.Subject, user.Id.ToString()),
                new Claim(JwtClaimTypes.Email, user.Email),
                new Claim(JwtClaimTypes.Scope, "XAPortal")
            };

            // add roles
            foreach (var role in roleList)
            {
                claims.Add(new Claim(JwtClaimTypes.Role, role));
            }


            context.IssuedClaims = claims;
        }

        /// <summary>
        /// This method gets called whenever identity server needs to determine if the user is valid or active (e.g. if the user's account has been deactivated since they logged in).
        /// (e.g. during token issuance or validation).
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public Task IsActiveAsync(IsActiveContext context)
        {
            var user = _userManager.FindbyIdAsync(context.Subject.GetSubjectId()).Result;
            context.IsActive = user?.AccessAllowed ?? false;

            return Task.FromResult(0);
        }
    }
}
