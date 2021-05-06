using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HUD.Data.Models.Actions;
using HUD.Data.Models.RepairShopr;
using HUD.Models;
using HUD.Data.Models.WorkerModels;
using Microsoft.AspNetCore.Http;
//using HUD.Data.Models.RepairShopr;

namespace HUD.Data.Models.Actions
{
    public class DraggableDropDownModel : ComponentBase
    {
        [Parameter]
        public ApiConnection _apiConn { get; set; }
        [Parameter]
        public ApplicationDbContext _context { get; set; }
        [Parameter]
        public IHttpContextAccessor _userContext { get; set; }
        [Parameter]
        public Dictionary<string, List<RepairTicket>> _ticketMap { get; set; }
        [Parameter]
        public string _userGuid { get; set; }
        protected int currentIndex;
        protected List<RepairCategory> itemList;

        public void StartDrag(RepairCategory item)
        {
            currentIndex = GetIndex(item);
        }

        public void ClickItem(RepairCategory item)
        {
            currentIndex = GetIndex(item);
        }

        public int GetIndex(RepairCategory item)
        {
            return itemList.FindIndex(a => a.number == item.number);
        }

        public void Drop(RepairCategory item)
        {
            if (item != null)
            {
                var index = GetIndex(item);
                // get current item
                var current = itemList[currentIndex];
                // remove RepairCategory from current index
                itemList.RemoveAt(currentIndex);
                itemList.Insert(index, current);

                // update current selection
                currentIndex = index;

                _apiConn.SetStatusOrder(itemList);

                UpdateDB(itemList);

                StateHasChanged();
            }
        }

        public void ReportList()
        {
            int i = 0;
        }

        private void UpdateDB(List<RepairCategory> lst)
        {
            List<string> newL = new List<string>();
            foreach (RepairCategory rc in lst)
            {
                newL.Add(rc.status);
            }

            UpdateDatabase.UpdateSortedListInDb(_context, _userGuid, newL);
        }

        protected override async Task<Task> OnInitializedAsync()
        {
            itemList = new List<RepairCategory>();
            int i = 0;
            foreach (var item in _ticketMap.Keys)
            {
                itemList.Add(new RepairCategory { status = item, number = i });
                i++;
            }

            return base.OnInitializedAsync();
        }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
        }
    }
}
