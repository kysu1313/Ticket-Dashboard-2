using System;

namespace HUD.Models
{
    partial class RepairTicket
    {
        public class InputUser
        {
            public int id { get; set; }
            public string email { get; set; }
            public string full_name { get; set; }
            public DateTime created_at { get; set; }
            public DateTime updated_at { get; set; }
            public string group { get; set; }
            public bool admin { get; set; }
            public string color { get; set; }
        }

    }
}
