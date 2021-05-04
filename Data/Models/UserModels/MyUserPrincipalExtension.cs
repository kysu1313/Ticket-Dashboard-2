using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HUD.Data.Models.UserModels
{
    public static class MyUserPrincipalExtension
    {
        public static string GetGroupSid(this ClaimsPrincipal user)
        {
            if (user.Identity.IsAuthenticated)
            {
                return user.Claims.FirstOrDefault(v => v.Type == ClaimTypes.GroupSid).Value;
            }

            return "";
        }
    }
}
