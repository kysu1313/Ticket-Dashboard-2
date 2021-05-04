using HUD.Data.Models.Actions;
using HUD.Data.Models.UserModels;
using HUD.Data.Models.WorkerModels;
using HUD.Models;
using HUD.Models.RepairShopr;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;


namespace HUD.Data.Models.RepairShopr
{
    public class ApiConnection
    {

        public string[] _statusList { get; set; }
        public IHttpContextAccessor _contextAccessor { get; set; }
        public ApplicationDbContext _context { get; set; }
        public UserSettings _settingsModel { get; set; }
        public string _userId { get; set; }
        public string _baseUrl;
        public string _urlPrefix { get; set; }
        public string _apiKey { get; set; }
        public IEnumerable<RepairTicket> _results { get; set; }
        protected List<RepairTicket> _sorted { get; set; }
        public bool Greensboro { get; set; }
        public bool Winston { get; set; }
        public bool Layout { get; set; }
        public int[] _numCardDropdown;
        public int[] _numCardDropdownStatus;
        public int _numCards = 60;
        public int _ticketsPerRow = 10;
        public List<string> _includeTickets;
        public Dictionary<string, bool> _displayMap;
        public Dictionary<string, List<RepairTicket>> _ticketMap;
        public List<RepairTicket> _greensboroTickets { get; set; }
        public List<RepairTicket> _winstonTickets { get; set; }
        public string[] _statusOrder;
        public string[] _defaultStatusOrder { get; } = new string[]
            {
                "Rush","Customer Reply","New","Ready to Repair","In Progress",
                "Waiting for Parts","Sent Offsite","Waiting on Customer"
                ,"Resolved","Ready for Pickup"
            };

        private const int greensboroId = 2666;
        private const int winstonId = 2667;

        public List<RepairTicket> _newTickets = new List<RepairTicket>();
        public List<RepairTicket> _readyToRepairTickets = new List<RepairTicket>();
        public List<RepairTicket> _waitingOnCustomerTickets = new List<RepairTicket>();
        public List<RepairTicket> _waitingForPartsTickets = new List<RepairTicket>();
        public List<RepairTicket> _customerReplyTickets = new List<RepairTicket>();
        public List<RepairTicket> _resolvedTickets = new List<RepairTicket>();
        public List<RepairTicket> _sentOffsiteTickets = new List<RepairTicket>();
        public List<RepairTicket> _readyForPickupTickets = new List<RepairTicket>();
        public List<RepairTicket> _inProgressTickets = new List<RepairTicket>();
        public List<RepairTicket> _rushTickets = new List<RepairTicket>();

        private RepairShoprApi rsa;

        /// <summary>
        /// Constructor, takes bool noting which location we are in.
        /// </summary>
        /// <param name="inGreensboro"></param>
        /// <param name="inWinston"></param>
        public ApiConnection(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, string userGuid, IConfiguration configuration, string prefix, string apiKey, bool inGreensboro, bool inWinston)
        {
            _context = context;
            _contextAccessor = httpContextAccessor;
            _userId = userGuid;
            _urlPrefix = prefix;
            _apiKey = apiKey;
            Greensboro = inGreensboro;
            Winston = inWinston;
            CreateDropdowns();
            GetUserSettings(userGuid);

            InstantiateVars();

            rsa = new RepairShoprApi(configuration, _urlPrefix, _apiKey);

            CallApi();
        }

        private void GetUserSettings(string uid)
        {

            _settingsModel = _context.userSettings.FirstOrDefault(a => a.UserGuid == _userId);
            if (_settingsModel is null)
            {
                UserSettings us = new UserSettings()
                {
                    UserGuid = _userId,
                    location = Greensboro ? "Greensboro" : "Winston",
                    ticketOrder = String.Join(", ", _defaultStatusOrder),
                    ticketsToShow = String.Join(", ", _defaultStatusOrder),
                    numTicketsPerStatus = 10,
                    numTickets = 100
                };
                _context.userSettings.Add(us);
                _context.SaveChanges();
                _settingsModel = us;
            }
            else
            {
                Debug.WriteLine("User already exists");
            }
        }


        /// <summary>
        /// Api call collects tickets, then sorts them and interperets
        /// the data for displaying. Updates all status labels.
        /// </summary>
        public void CallApi()
        {
            _results = rsa.GetTicketList();
            _results = SortListResult(_results.ToList());
            UpdateStatuses(_results.ToList());
            SetStatuses();
        }

        /// <summary>
        /// Sort list of tickets by time since arrival putting rushes at the front.
        /// Include only the tickets that are selected.
        /// </summary>
        /// <param name="lst"></param>
        /// <returns></returns>
        private List<RepairTicket> SortListResult(List<RepairTicket> lst)
        {
            _results = Sorter.SortListResult(lst, _includeTickets, greensboroId, Greensboro);

            return _results.ToList();
        }


        /// <summary>
        /// Initialize the dropdown menus with appropriate values.
        /// </summary>
        private void CreateDropdowns()
        {
            _numCardDropdown = BackParser.CreateDropdowns(_numCardDropdown);
            _numCardDropdownStatus = BackParser.CreateDropdownStatus(_numCardDropdownStatus);
        }

        /// <summary>
        /// Instantiate the maps used to store tickets.
        /// Maps are sorted by the ticket status.
        /// </summary>
        private void InstantiateVars()
        {
            
            if (_results is null)
            {
                _results = new List<RepairTicket>();
            }
            _ticketMap = BackParser.BuildTicketMap(_context, _userId, _ticketMap);
            _displayMap = BackParser.BuildStatusMap(_context, _userId, _displayMap);

            UserSettings usett = UpdateDatabase.GetCurrentUserSettings(_context, _userId);

            if (usett is not null)
            {
                string statusOrderStr = usett.ticketOrder;
                _statusOrder = statusOrderStr.Split(", ");
                _ticketsPerRow = usett.numTicketsPerStatus;
            }
            else
            {
                _statusOrder = new string[]
                    {
                        "Rush","Customer Reply","New","Ready to Repair","In Progress",
                        "Waiting for Parts","Sent Offsite","Waiting on Customer"
                        ,"Resolved","Ready for Pickup"
                    };
            }

            SetStatuses();
        }

        /// <summary>
        /// Add labels to include status list.
        /// This is used to show all tickets when display is first loads.
        /// </summary>
        private void SetStatuses()
        {
            _includeTickets = BackParser.SetStatusList(_userId, _includeTickets, _displayMap);
        }

        /// <summary>
        /// Keep track of the number of each type of ticket.
        /// </summary>
        /// <param name="repairList"></param>
        private void UpdateStatuses(List<RepairTicket> repairList)
        {
            BackParser.ClearMap(_ticketMap);
            Sorter.ParseStatusMap(repairList, _ticketMap);
            BackParser.UpdateStatuses(repairList, _ticketMap);
        }

        /// <summary>
        /// Uses input from the "Order" dropdown menu to sort the tickets by their status.
        /// </summary>
        /// <param name="statusLst"></param>
        public void SetStatusOrder(List<RepairCategory> statusLst)
        {
            _statusOrder = new string[statusLst.Count()];
            for (int i = 0; i < _statusOrder.Length; i++)
            {
                _statusOrder[i] = statusLst[i].status;
            }
        }

        /// <summary>
        /// Parse and sort the list of tickets pulled from the API using their status.
        /// </summary>
        /// <param name="repairList"></param>
        private void ParseStatus(List<RepairTicket> repairList)
        {
            Sorter.ParseStatusMap(repairList, _ticketMap);
        }

        /// <summary>
        /// Get the color of the ticket depending on the tickets status.
        /// This is used for dynamic css.
        /// </summary>
        /// <param name="rt"></param>
        /// <returns></returns>
        public string GetColor(RepairTicket rt)
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

        /// <summary>
        /// Get the icon for the ticket depending on the tickets status.
        /// </summary>
        /// <param name="rt"></param>
        /// <returns></returns>
        public string GetIcon(RepairTicket rt)
        {
            switch (rt.status)
            {
                case "New":
                    return "add";
                case "Waiting on Customer":
                    return "hourglass_bottom";
                case "Ready to Repair":
                    return "thumb_up_alt";
                case "Waiting for Parts":
                    return "pending";
                case "Customer Reply":
                    return "reply";
                case "Resolved":
                    return "grade";
                case "Sent Offsite":
                    return "call";
                case "Ready for Pickup":
                    return "done";
                case "In Progress":
                    return "build";
                case "Rush":
                    return "flash_on";
                default:
                    break;
            }
            return "";
        }

        /// <summary>
        /// Called when num card dropdown value is changed.
        /// </summary>
        /// <param name="i"></param>
        public void NumCardsChange(int i)
        {
            UpdateDatabase.UpdateNumTickets(_context, _userId, i);
            _numCards = i;
        }

        /// <summary>
        /// Called when tickets per row dropdown value is changed.
        /// </summary>
        /// <param name="i"></param>
        public void TicketsPerRowChange(int i)
        {

            UpdateDatabase.UpdateNumTicketsPerRow(_context, _userId, i);
            _ticketsPerRow = i;
        }


        /// <summary>
        /// Update the type of tickets that are included.
        /// This is connected to the dropdown menu.
        /// </summary>
        /// <param name="s"></param>
        public void CollectTicketsToDisplay(string s)
        {
            _displayMap[s] = !_displayMap[s];
            _includeTickets.Clear();
            for (int i = 0; i < _displayMap.Count(); i++)
            {
                if (_displayMap.ElementAt(i).Value)
                {
                    _includeTickets.Add(_displayMap.ElementAt(i).Key);
                }
            }
            UpdateDatabase.UpdateIncludeListInDb(_context, _userId, _includeTickets);
        } 

    }
}
