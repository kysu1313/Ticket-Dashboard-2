using HUD.Data.Models.RepairShopr;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HUD.Data.Models.Actions
{
    public class BaseModel : ComponentBase
    {
        //string url = "https://localhost:44381/updatehub";
        public static ApiConnection _apiConn { get; set; }
        public static TopControlsModel _tcm { get; set; }


        protected override async Task<Task> OnInitializedAsync()
        {
            return base.OnInitializedAsync();
        }



        [JSInvokable]
        public static string SayHelloCS(string name)
        {
            return "Hello, " + name;
        }
    }
}
