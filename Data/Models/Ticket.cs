using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HUD.Models
{
    class Ticket
    {
        public int Id { get; set; }
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
        [Display(Name = "Ticket Number")]
        public int TicketNumber { get; set; }
        [Display(Name = "Invoice Number")]
        public int InvoiceNumber { get; set; }
    }
}
