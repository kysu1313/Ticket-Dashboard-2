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
        //string connectionStatus = "Closed";
        //bool isConnected = false;
        //HubConnection _connection = null;

        //List<string> updates = new List<string>();
        public static ApiConnection _apiConn { get; set; }
        public static TopControlsModel _tcm { get; set; }
        //public Base baseClass { get; set; }
        //public Controls controls { get; set; }


        protected override async Task<Task> OnInitializedAsync()
        {
            //_apiConn = new ApiConnection();
            //controls = new Controls();
            //_tcm = new TopControlsModel();
            //baseClass = new Base();
            //_tcm = new TopControlsModel();
            return base.OnInitializedAsync();
        }

        //public Base GetBase()
        //{
        //    return baseClass;
        //}



        [JSInvokable]
        public static string SayHelloCS(string name)
        {
            return "Hello, " + name;
        }
    }
}
