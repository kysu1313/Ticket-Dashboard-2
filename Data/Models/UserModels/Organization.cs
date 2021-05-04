using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HUD.Data.Models.UserModels
{
    public class Organization
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string UrlPrefix { get; set; }
        public string Pin { get; set; }
        public string ApiKey { get; set; }
        public List<ApplicationUser> Members { get; set; }
    }
}
