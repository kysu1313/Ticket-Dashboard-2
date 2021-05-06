//using HUD.Data.Models.RepairShopr;
using HUD.Data.Models.Actions;
using HUD.Data.Models.RepairShopr;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace HUD.Data.Models
{
    public class TopControlsModel : ComponentBase
    {
        [Parameter]
        public ApiConnection _apiConn { get; set; }
        [Parameter]
        public ApplicationDbContext _context{ get; set; }
        [Parameter]
        public IHttpContextAccessor _userContext { get; set; }
        [Parameter]
        public string _userGuid { get; set; }
        [Parameter]
        public bool Greensboro { get; set; }
        [Parameter]
        public bool Winston { get; set; }
        [Parameter]
        public int _refreshRate { get; set; }

        public int LayoutType { get; set; } = 0;
        public bool Normal { get; set; } = false;
        public bool Status { get; set; } = true;
        public int[] _numCardDropdown = new int[] { };

        public TicketList tlist { get; set; }

        /// <summary>
        /// Swap view layout type.
        /// </summary>
        public void SwapLayout()
        {
            Normal = !Normal;
            Status = !Status;
            _apiConn.Layout = Normal;
            _apiConn.CallApi();
        }

        /// <summary>
        /// If location is Greensboro, change to Winston and vice-versa.
        /// </summary>
        public void SwapLocation()
        {
            Greensboro = !Greensboro;
            Winston = !Winston;
            _apiConn.CallApi();
        }

        protected override async Task<Task> OnInitializedAsync()
        {
            return base.OnInitializedAsync();
        }

        /// <summary>
        /// Sets the number of options in the "number of cards" dropdown menu.
        /// </summary>
        public void SetNumCardDropdown()
        {
            _numCardDropdown = _apiConn._numCardDropdown;
            _apiConn.CallApi();
        }

        /// <summary>
        /// Passing api connection, gboro and winston variables to this class.
        /// </summary>
        protected override void OnParametersSet()
        {
            base.OnParametersSet();
        }

    }
}
