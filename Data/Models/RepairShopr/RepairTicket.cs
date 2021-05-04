using System;
using System.Collections.Generic;
using System.Text;

namespace HUD.Models
{
    public partial class RepairTicket
    {

        public int id { get; set; }
        public int number { get; set; }
        public string subject { get; set; }
        public DateTime created_at { get; set; }
        public int customer_id { get; set; }
        public string customer_business_then_name { get; set; }
        public DateTime due_date { get; set; }
        public DateTime? resolved_at { get; set; }
        public object start_at { get; set; }
        public object end_at { get; set; }
        public int location_id { get; set; }
        public string problem_type { get; set; }
        public string status { get; set; }
        public int ticket_type_id { get; set; }
        public Properties properties { get; set; }
        public int? user_id { get; set; }
        public DateTime updated_at { get; set; }
        public object pdf_url { get; set; }
        public object priority { get; set; }
        public Contact[] comments { get; set; }
        public InputUser user { get; set; }

    }
}
