using HUD.Data.Models.RepairShopr;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HUD.Data.Models.Actions
{
    public class WinstonBase : ComponentBase
    {
        [Inject]
        public IConfiguration Configuration { get; set; }
        public static ApiConnection _apiConnW { get; set; }
        public static TopControlsModel _tcm { get; set; }
        public string _prefix { get; set; }
        public string _apiKey { get; set; }
        public const int REFRESH_RATE = 1000;
        public string _userGuid { get; set; }

    }
}
