using Microsoft.AspNetCore.Components;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using HUD.Models;
using Humanizer;
using System.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using HUD.Data.Models.RepairShopr;
using HUD.Data.Models.Actions;
using Microsoft.AspNetCore.Http;
//using HUD.Data.Models.RepairShopr;

namespace HUD.Data.Models
{
    public class TicketList : ComponentBase
    {

        [Parameter]
        public ApiConnection _apiConn { get; set; }
        [Parameter]
        public IHttpContextAccessor _userContext { get; set; }
        [Parameter]
        public bool Layout { get; set; }
        [Parameter]
        public int _refreshRate { get; set; }
        

        protected List<RepairTicket> _results;
        protected int _numCards;
        protected int _ticketsPerRow;
        public Dictionary<string, List<RepairTicket>> _ticketMap;
        protected DateTime _todaysDate = DateTime.Now;
        protected string[] _statusOrder;
        protected List<string> _includeTickets;
        public string _baseUrl;
        [Inject]
        public NavigationManager navigationManager { get; set; }
        private bool isOriginalConn = false;

        

        protected override async Task<Task> OnInitializedAsync()
        {
            return base.OnInitializedAsync();
        }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            RefreshData(_refreshRate);
        }

        private async void RefreshData(Int32 waitTime)
        {
            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken token = source.Token;
            if (_userContext.HttpContext is null || !_userContext.HttpContext.User.Identity.IsAuthenticated)
            {
                return;
            }
            _apiConn.CallApi();
            _results = _apiConn._results.ToList();
            SetResults();
            SetStatusOrder();
            SetIncludeTickets();
            SetTicketMap();
            SetNumCards();
            StateHasChanged();
            await Task.Delay(waitTime, token);
            RefreshData(waitTime);
        }



        public void SetResults()
        {
            if (_results is not null)
            {
                _results.Clear();
            }
            _results = _apiConn._results.ToList();
            
        }

        public void SetStatusOrder()
        {

            _statusOrder = _apiConn._statusOrder;
        }

        public void SetIncludeTickets()
        {

            _includeTickets = _apiConn._includeTickets;
        }

        public void SetTicketMap()
        {
            _ticketMap = _apiConn._ticketMap;
        }

        public void SetNumCards()
        {
            _numCards = _apiConn._numCards;
        }

        public string GetIconForTicket(RepairTicket rt)
        {
            return _apiConn.GetIcon(rt);
        }

        public static string GetColor(RepairTicket rt)
        {
            switch (rt.status)
            {
                case "New":
                    return "new-card";
                case "Waiting on Customer":
                    return "waiting-on-customer-card";
                case "Ready to Repair":
                    return "ready-to-repair-card";
                case "Waiting for Parts":
                    return "waiting-for-parts-card";
                case "Customer Reply":
                    return "customer-reply-card";
                case "Resolved":
                    return "resolved-card";
                case "Sent Offsite":
                    return "sent-offsite-card";
                case "Ready for Pickup":
                    return "ready-for-pickup-card";
                case "In Progress":
                    return "in-progress-card";
                case "Rush":
                    return "rush-card";
                default:
                    break;
            }
            return "";
        }

    }

}