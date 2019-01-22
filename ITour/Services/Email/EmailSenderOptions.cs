using ITour.Services.Email.SendGrid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITour.Services.Email
{
    public class EmailSenderOptions
    {
        public string AppEmailAddress { get; set; }
        public string AppEmailName { get; set; }
        public SendGridSenderOptions SendGrid { get; set; }
    }
}
