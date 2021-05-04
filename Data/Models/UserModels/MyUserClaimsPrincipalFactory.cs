using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HUD.Data.Models.UserModels
{
    public class MyUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<ApplicationUser>
    {
        public MyUserClaimsPrincipalFactory(
            UserManager<ApplicationUser> userManager,
            IOptions<IdentityOptions> optionsAccessor) : base(userManager, optionsAccessor)
        {
        }

        //public async override Task<ClaimsPrincipal> CreateAsync(ApplicationUser user)
        //{
        //    var principal = await base.CreateAsync(user);

        //    //Putting our Property to Claims
        //    //I'm using ClaimType.Email, but you may use any other or your own
        //    ((ClaimsIdentity)principal.Identity).AddClaims(new[] {
        //            new Claim(ClaimTypes.GroupSid, user.OrganizationPrefix)
        //});

        //    return principal;
        //}


        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(ApplicationUser user)
        {
            var identity = await base.GenerateClaimsAsync(user);
            identity.AddClaim(new Claim("UrlPrefix", user.OrganizationPrefix ?? "[Click to edit profile]"));
            identity.AddClaim(new Claim("ApiKey", user.ApiKey?? "[Click to edit profile]"));
            return identity;

        }

    }
}
