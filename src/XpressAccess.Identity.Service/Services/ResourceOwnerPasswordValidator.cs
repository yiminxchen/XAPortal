using System.Threading.Tasks;
using XpressAccess.Identity.EFStore;
using XpressAccess.Identity.Manager;
using IdentityServer4.Models;
using IdentityServer4.Validation;

namespace XpressAccess.Identity.Service
{
    /// <summary>
    /// Handles validation of resource owner password credentials
    /// </summary>
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly IdentityUserManager<IdentityUser> _userManager;

        public ResourceOwnerPasswordValidator(IdentityUserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            if (_userManager.ValidateIdentityAsync(context.UserName, context.Password).Result)
            {
                string subjectId = _userManager.FindbyEmailAsync(context.UserName).Result.Id.ToString();
                context.Result = new GrantValidationResult(subjectId, "Password");
                return Task.FromResult(context.Result);
            }
            context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "Failed to validate identity(credential does not match)");
            return Task.FromResult(context.Result);
        }
    }
}
