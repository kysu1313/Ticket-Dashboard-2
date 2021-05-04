using HUD.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace HUD.Data.Models.WorkerModels
{
    public class BackParser
    {
        public static List<string> SetStatusList(string _userId, List<string> _includeTickets, Dictionary<string, bool> _displayMap)
        {
            _includeTickets = new List<string>();

            foreach (string st in _displayMap.Keys)
            {
                if (_displayMap[st])
                {
                    _includeTickets.Add(st);
                }
            }
            return _includeTickets;
        }

        public static int[] CreateDropdowns(int[] _numCardDropdown)
        {
            if (_numCardDropdown is null)
            {
                _numCardDropdown = Enumerable.Range(1, 100).ToArray();
            }
            return _numCardDropdown;
        }



        public static int[] CreateDropdownStatus(int[]  _numCardDropdownStatus)
        {
            if (_numCardDropdownStatus is null)
            {
                _numCardDropdownStatus = Enumerable.Range(1, 30).ToArray();
            }
            return _numCardDropdownStatus;
        }

        public static void UpdateStatuses(List<RepairTicket> repairList, Dictionary<string, List<RepairTicket>> _ticketMap)
        {
            foreach (List<RepairTicket> lst in _ticketMap.Values)
            {
                try
                {
                    lst.Sort((x, y) => DateTime.Compare(x.created_at, y.created_at));
                }
                catch (Exception e)
                {
                    Debug.WriteLine($"compare error: {e}");
                }
            }
        }

        public static void ClearMap(Dictionary<string, List<RepairTicket>> _ticketMap)
        {
            foreach (string key in _ticketMap.Keys)
            {
                List<RepairTicket> lst = _ticketMap[key];
                if (lst != null)
                {
                    _ticketMap[key].Clear();
                }
                _ticketMap[key] = new List<RepairTicket>();
            }
        }

        /// <summary>
        /// Instantiate the maps used to store tickets.
        /// Maps are sorted by the ticket status.
        /// </summary>
        public static Dictionary<string, List<RepairTicket>> BuildTicketMap(ApplicationDbContext context, string userId, Dictionary<string, List<RepairTicket>> map)
        {
            map = new Dictionary<string, List<RepairTicket>>();

            List<string> ticketOrder = UpdateDatabase.GetTicketOrder(context, userId);

            if (ticketOrder is not null)
            {
                foreach (string status in ticketOrder)
                {
                    map.Add(status, new List<RepairTicket>());
                }
            }
            else
            {
                map.Add("Rush", new List<RepairTicket>());
                map.Add("Customer Reply", new List<RepairTicket>());
                map.Add("New", new List<RepairTicket>());
                map.Add("Ready to Repair", new List<RepairTicket>());
                map.Add("In Progress", new List<RepairTicket>());
                map.Add("Waiting for Parts", new List<RepairTicket>());
                map.Add("Sent Offsite", new List<RepairTicket>());
                map.Add("Waiting on Customer", new List<RepairTicket>());
                map.Add("Resolved", new List<RepairTicket>());
                map.Add("Ready for Pickup", new List<RepairTicket>());
            }


            return map;
        }

        /// <summary>
        /// Instantiate the maps used to store status bools.
        /// Maps are sorted by the ticket status.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="userId"></param>
        /// <param name="map"></param>
        /// <returns></returns>
        public static Dictionary<string, bool> BuildStatusMap(ApplicationDbContext context, string userId, Dictionary<string, bool> map)
        {
            map = new Dictionary<string, bool>();

            List<string> includedTickets = UpdateDatabase.GetIncludeTickets(context, userId);


            if (includedTickets is not null)
            {
                foreach (string status in UpdateDatabase.GetTicketOrder(context, userId))
                {
                    map.Add(status, includedTickets.Contains(status));
                }
            }
            else
            {
                map.Add("Rush", true);
                map.Add("Customer Reply", true);
                map.Add("New", true);
                map.Add("Ready to Repair", true);
                map.Add("In Progress", true);
                map.Add("Waiting for Parts", true);
                map.Add("Sent Offsite", true);
                map.Add("Waiting on Customer", true);
                map.Add("Resolved", true);
                map.Add("Ready for Pickup", true);
            }

            return map;
        }
    }
}
