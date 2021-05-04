using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HUD.Data.Models.UserModels
{
    public class ApplicationUser : IdentityUser
    {
        public int UserId { get; set; }
        public string OrganizationPrefix { get; set; }
        public string ApiKey { get; set; }
        public bool IsLoggedIn { get; set; }
    }
}
