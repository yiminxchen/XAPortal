# XA Portal
A proof of concept solution using IdentityServer4 OpenID Connect and OAuth 2.0 framework to protect web site/api access.
***
###Projects
+ **Identity.Service** - Identity Service using IdentityServer4 with custom User Manager for validation and profile
+ **Identity.EFStore** - User/Role store using Entity Framework Core 1.1
+ **Identity.Manager** - User/Role manager using SQL Server
+ **Portal** - ASP.NET Core MVC Web App demonstrating OpenID Connect Hybrid Flow for user authentication and role base Api authorization
+ **WebData.Service** - ASP.NET Core Web Api service for testing role base authorization using IdentityServer4 access token
+ **AngularClient** - AngularJS client demonstrating OpenID Connect Implicit Flow for identity validation and JWT ID/Access token authorization against Api

***
###Tools
+ Visual Studio 2015 Update 3
+ NPM
+ Bower
+ Gulp
