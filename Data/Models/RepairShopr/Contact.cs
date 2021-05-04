using System;
using System.Collections.Generic;
using System.Text;

namespace HUD.Models
{

    public class Contact
    {
        public int id { get; set; }
        public int number { get; set; }
        public string customerName { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public int ticket_id { get; set; }
        public string subject { get; set; }
        public string body { get; set; }
        public string tech { get; set; }
        public bool hidden { get; set; }
        public object user_id { get; set; }
    }

}
