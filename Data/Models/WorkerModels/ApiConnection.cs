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
using HUD.Data.Models.Common;


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
        private const int greensboroId = 2666;
        private const int winstonId = 2667;
        public string[] _defaultStatusOrder { get; set; }

        private RepairShoprApi rsa;

        /// <summary>
        /// Constructor, takes bool noting which location we are in.
        /// </summary>
        /// <param name="inGreensboro"></param>
        /// <param name="inWinston"></param>
        public ApiConnection(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, string userGuid, IConfiguration configuration, string prefix, string apiKey, bool inGreensboro, bool inWinston)
        {
            _defaultStatusOrder = new string[]
            {
                Constants.RUSH,Constants.CORPORATE_CUSTOMER,Constants.CUSTOMER_REPLY,Constants.NEW,Constants.READY_TO_REPAIR,Constants.IN_PROGRESS,
                Constants.WAITING_FOR_PARTS,Constants.SENT_OFFSITE,Constants.WAITING_ON_CUSTOMER
                ,Constants.RESOLVED,Constants.READY_FOR_PICKUP
            };
            _context = context;
            _contextAccessor = httpContextAccessor;
            _userId = userGuid;
            _urlPrefix = prefix;
            _apiKey = apiKey;
            Greensboro = inGreensboro;
            Winston = inWinston;
            CreateDropdowns();
            rsa = new RepairShoprApi(configuration, _urlPrefix, _apiKey);
            GetUserSettings(userGuid);
            InstantiateVars();

            CallApi();
        }

        private void GetUserSettings(string uid)
        {

            _settingsModel = _context.userSettings.FirstOrDefault(a => a.UserGuid == _userId);
            
            if (_settingsModel is null)
            {
                Dictionary<string, string> settings = rsa.ApiGetSettings();
                UserSettings us = new UserSettings()
                {
                    UserGuid = _userId,
                    location = Greensboro ? Constants.GREENSBORO : Constants.WINSTON,
                    ticketOrder = String.Join(", ", _defaultStatusOrder),
                    ticketsToShow = String.Join(", ", _defaultStatusOrder),
                    numTicketsPerStatus = 10,
                    numTickets = 100,
                    SmallLogo = settings[Constants.LOGO_THUMB],
                    LargeLogo = settings[Constants.LOGO_LARGE],
                    OrgName = settings[Constants.ORGANIZATION_NAME]
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
                _statusOrder = _defaultStatusOrder;
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
                case Constants.NEW:
                    return "new-card";
                case Constants.WAITING_ON_CUSTOMER:
                    return "waiting-on-customer-card";
                case Constants.READY_TO_REPAIR:
                    return "ready-to-repair-card";
                case Constants.WAITING_FOR_PARTS:
                    return "waiting-for-parts-card";
                case Constants.CUSTOMER_REPLY:
                    return "customer-reply-card";
                case Constants.RESOLVED:
                    return "resolved-card";
                case Constants.SENT_OFFSITE:
                    return "sent-offsite-card";
                case Constants.READY_FOR_PICKUP:
                    return "ready-for-pickup-card";
                case Constants.IN_PROGRESS:
                    return "in-progress-card";
                case Constants.RUSH:
                    return "rush-card";
                case Constants.CORPORATE_CUSTOMER:
                    return "corporate-card";
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
                case Constants.NEW:
                    return "add";
                case Constants.WAITING_ON_CUSTOMER:
                    return "hourglass_bottom";
                case Constants.READY_TO_REPAIR:
                    return "thumb_up_alt";
                case Constants.WAITING_FOR_PARTS:
                    return "pending";
                case Constants.CUSTOMER_REPLY:
                    return "reply";
                case Constants.RESOLVED:
                    return "grade";
                case Constants.SENT_OFFSITE:
                    return "call";
                case Constants.READY_FOR_PICKUP:
                    return "done";
                case Constants.IN_PROGRESS:
                    return "build";
                case Constants.RUSH:
                    return "flash_on";
                case Constants.CORPORATE_CUSTOMER:
                    return "corporate_fare";
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
