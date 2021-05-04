//using HUD.Data.Models.RepairShopr;
using HUD.Data.Models.RepairShopr;
using HUD.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HUD.Data.Models
{
    public class ContactListModel : ComponentBase
    {

        [Parameter]
        public ApiConnection _apiConn { get; set; }
        [Parameter]
        public List<RepairTicket> _results { get; set; }

        protected List<RepairTicket> _listResults { get; set; }
        protected List<RepairTicket> _sorted { get; set; }
        protected List<Contact> _contacts { get; set; }


        public void UpdateContacts(List<RepairTicket> list)
        {
            _results = list;
            _listResults = list;
            ExtractContacts();
        }

        private async void RefreshData(Int32 waitTime)
        {
            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken token = source.Token;
            _results = (List<RepairTicket>)_apiConn._results;
            ExtractContacts();
            StateHasChanged();

            await Task.Delay(waitTime, token);
            RefreshData(waitTime);
        }

        private void ExtractContacts()
        {
            _contacts = new List<Contact>();
            foreach (RepairTicket ticket in _results)
            {
                if (ticket.status.Equals("Customer Reply"))
                {
                    Contact ct = ticket.comments[0];
                    ct.number = ticket.number;
                    ct.customerName = ticket.customer_business_then_name;
                    _contacts.Add(ct);
                }
            }
        }

        public void UpdatePage()
        {
            StateHasChanged();
        }

        protected override async Task<Task> OnInitializedAsync()
        {
            //GetResults();
            RefreshData(5000);
            return base.OnInitializedAsync();
        }

    }
}
