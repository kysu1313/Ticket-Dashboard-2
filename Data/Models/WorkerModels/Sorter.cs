using HUD.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace HUD.Data.Models.WorkerModels
{
    public class Sorter
    {

        public static List<RepairTicket> SortListResult(List<RepairTicket> lst, List<string> _includeTickets, int greensboroId, bool Greensboro)
        {
            List<RepairTicket> allTickets = new List<RepairTicket>();

            List<RepairTicket> rushTicketsGboro = new List<RepairTicket>();
            List<RepairTicket> normalTicketsGboro = new List<RepairTicket>();

            List<RepairTicket> rushTicketsWinston = new List<RepairTicket>();
            List<RepairTicket> normalTicketsWinston = new List<RepairTicket>();

            for (int i = 0; i < lst.Count(); i++)
            {
                if (lst.ElementAt(i).location_id.Equals(greensboroId))
                {
                    NormalVsRush(lst, rushTicketsGboro, normalTicketsGboro, _includeTickets, i);
                }
                else
                {
                    NormalVsRush(lst, rushTicketsWinston, normalTicketsWinston, _includeTickets, i);
                }

            }
            SortByDate(normalTicketsGboro);
            SortByDate(normalTicketsWinston);
            //normalTicketsGboro.Sort((x, y) => DateTime.Compare(x.created_at, y.created_at));
            //normalTicketsWinston.Sort((x, y) => DateTime.Compare(x.created_at, y.created_at));

            foreach (RepairTicket tm in rushTicketsGboro)
            {
                normalTicketsGboro.Insert(0, tm);
            }
            foreach (RepairTicket tm in rushTicketsWinston)
            {
                normalTicketsWinston.Insert(0, tm);
            }

            Debug.WriteLine("Greensboro: " + normalTicketsGboro.Count());
            Debug.WriteLine("Winston: " + normalTicketsWinston.Count());

            if (Greensboro)
            {
                return normalTicketsGboro;
            }
            else
            {
                return normalTicketsWinston;
            }
        }

        public static void SortByDate(List<RepairTicket> list)
        {
            list.Sort((x, y) => DateTime.Compare(x.created_at, y.created_at));
        }

        private static void NormalVsRush(List<RepairTicket> lst, List<RepairTicket> rushTickets, List<RepairTicket> normalTickets, List<string> _includeTickets, int i)
        {
            if (lst.ElementAt(i).status.Equals("Rush"))
            {
                rushTickets.Add(lst.ElementAt(i));
            }
            else if (_includeTickets.Contains(lst.ElementAt(i).status))
            {
                normalTickets.Add(lst.ElementAt(i));
            }
        }

        public static void ParseStatusMap(List<RepairTicket> repairList, Dictionary<string, List<RepairTicket>> _ticketMap)
        {
            foreach (List<RepairTicket> lst in _ticketMap.Values)
            {
                lst.Clear();
            }
            foreach (RepairTicket rt in repairList)
            {
                switch (rt.status)
                {
                    case "New":
                        _ticketMap["New"].Add(rt);
                        break;
                    case "Waiting on Customer":
                        _ticketMap["Waiting on Customer"].Add(rt);
                        break;
                    case "Ready to Repair":
                        _ticketMap["Ready to Repair"].Add(rt);
                        break;
                    case "Waiting for Parts":
                        _ticketMap["Waiting for Parts"].Add(rt);
                        break;
                    case "Customer Reply":
                        _ticketMap["Customer Reply"].Add(rt);
                        break;
                    case "Resolved":
                        _ticketMap["Resolved"].Add(rt);
                        break;
                    case "Sent Offsite":
                        _ticketMap["Sent Offsite"].Add(rt);
                        break;
                    case "Ready for Pickup":
                        _ticketMap["Ready for Pickup"].Add(rt);
                        break;
                    case "In Progress":
                        _ticketMap["In Progress"].Add(rt);
                        break;
                    case "Rush":
                        _ticketMap["Rush"].Add(rt);
                        break;
                    case "Corporate Customer":
                        _ticketMap["Corporate Customer"].Add(rt);
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
