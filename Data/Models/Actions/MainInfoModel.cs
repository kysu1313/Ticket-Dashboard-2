//using HUD.Data.Models.RepairShopr;
using HUD.Data.Models.Actions;
using HUD.Data.Models.RepairShopr;
using HUD.Models;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace HUD.Data.Models
{
    public class MainInfoModel : ComponentBase
    {
        [Parameter]
        public ApiConnection _apiConn { get; set; }
        [Parameter]
        public int _refreshRate { get; set; }

        //private ApiConnection _apiConn;
        protected List<MainItem> _results { get; set; }
        protected List<MainItem> _sorted { get; set; }
        protected int _openTickets { get; set; }
        protected string _url = "https://localhost:44372/updatehub";
        protected string _connectionStatus = "Closed";
        protected bool _isConnected;
        protected List<MainItem> bChartIncoming;
        public string _baseUrl;
        [Inject]
        public NavigationManager navigationManager { get; set; }
        //public ApiConnection _apiConn;


        protected MainItem rush = new MainItem
        {
            totalCount = 0,
            name = "Rush"
        };

        protected MainItem sentOffsite = new MainItem
        {
            totalCount = 0,
            name = "Sent Offsite"
        };

        protected MainItem incoming = new MainItem
        {
            totalCount = 0,
            name = "New"
        };

        protected MainItem customerReply = new MainItem
        {
            totalCount = 0,
            name = "Customer Reply"
        };

        protected MainItem inProgress = new MainItem
        {
            totalCount = 0,
            name = "On Bench"
        };

        protected MainItem waitingOnCustomer = new MainItem
        {
            totalCount = 0,
            name = "Waiting on Customer"
        };

        protected MainItem waitingForParts = new MainItem
        {
            totalCount = 0,
            name = "Waiting on Parts"
        };

        protected MainItem resolved = new MainItem
        {
            totalCount = 0,
            name = "Resolved"
        };

        protected MainItem readyToRepair = new MainItem
        {
            totalCount = 0,
            name = "Ready to Repair"
        };

        /// <summary>
        /// Used for testing.
        /// </summary>
        /// <returns></returns>
        public void UpdateValues(Dictionary<string, List<RepairTicket>> _ticketMap)
        {
            //DataChange(_ticketMap);
            StateHasChanged();
        }

        private async void RefreshData()
        {
            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken token = source.Token;
            DataChange();
            StateHasChanged();
            await Task.Delay(_refreshRate, token);
            RefreshData();
        }

        public void DataChange()
        {
            incoming.totalCount = _apiConn._ticketMap["New"].Count();
            waitingOnCustomer.totalCount = _apiConn._ticketMap["Waiting on Customer"].Count();
            readyToRepair.totalCount = _apiConn._ticketMap["Ready to Repair"].Count();
            waitingForParts.totalCount = _apiConn._ticketMap["Waiting for Parts"].Count();
            customerReply.totalCount = _apiConn._ticketMap["Customer Reply"].Count();
            resolved.totalCount = _apiConn._ticketMap["Resolved"].Count();
            sentOffsite.totalCount = _apiConn._ticketMap["Sent Offsite"].Count();
            //_numReadyForPickup = _apiConn._ticketMap["Ready for Pickup"].Count();
            inProgress.totalCount = _apiConn._ticketMap["In Progress"].Count();
            rush.totalCount = _apiConn._ticketMap["Rush"].Count();
            
        }

        /// <summary>
        /// Initialize model when page loads.
        /// Call PerioducUpdatesAsync to begin the continuous updates without needing to refresh the page.
        /// </summary>
        /// <returns></returns>
        protected override async Task<Task> OnInitializedAsync()
        {

            //_apiConn = BaseModel._apiConn;
            return base.OnInitializedAsync();
        }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            RefreshData();
        }


    }

    public class MainItem
    {
        public int totalCount { get; set; }
        public string name { get; set; }
        public int openCount { get; set; }
    }
}
