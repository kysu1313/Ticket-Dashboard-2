using HUD.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using HUD.Data.Models.Common;

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
                    case Constants.NEW:
                        _ticketMap[Constants.NEW].Add(rt);
                        break;
                    case Constants.WAITING_ON_CUSTOMER:
                        _ticketMap[Constants.WAITING_ON_CUSTOMER].Add(rt);
                        break;
                    case Constants.READY_TO_REPAIR:
                        _ticketMap[Constants.READY_TO_REPAIR].Add(rt);
                        break;
                    case Constants.WAITING_FOR_PARTS:
                        _ticketMap[Constants.WAITING_FOR_PARTS].Add(rt);
                        break;
                    case Constants.CUSTOMER_REPLY:
                        _ticketMap[Constants.CUSTOMER_REPLY].Add(rt);
                        break;
                    case Constants.RESOLVED:
                        _ticketMap[Constants.RESOLVED].Add(rt);
                        break;
                    case Constants.SENT_OFFSITE:
                        _ticketMap[Constants.SENT_OFFSITE].Add(rt);
                        break;
                    case Constants.READY_FOR_PICKUP:
                        _ticketMap[Constants.READY_FOR_PICKUP].Add(rt);
                        break;
                    case Constants.IN_PROGRESS:
                        _ticketMap[Constants.IN_PROGRESS].Add(rt);
                        break;
                    case Constants.RUSH:
                        _ticketMap[Constants.RUSH].Add(rt);
                        break;
                    case Constants.CORPORATE_CUSTOMER:
                        _ticketMap[Constants.CORPORATE_CUSTOMER].Add(rt);
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
