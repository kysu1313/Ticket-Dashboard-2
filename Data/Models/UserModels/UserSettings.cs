using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HUD.Data.Models.UserModels
{
    public class UserSettings
    {
        public int id { get; set; }
        public string UserGuid { get; set; }
        public string location { get; set; }
        // status1,status2,status3,...
        public string ticketOrder { get; set; }
        // status1,status2,status3,...
        public string ticketsToShow { get; set; }
        public int numTicketsPerStatus { get; set; }
        public int numTickets { get; set; }
    }
}
