using Skymail_PortalCorretor.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skymail_PortalCorretor.Skymail.Model
{
    public class Mail : ApiResponse<Mail>
    {
        public string mail { get; set; }
        public string displayname { get; set; }
        public string accounttype { get; set; }
        public string accountstatus { get; set; }
        public string mobilephone { get; set; }
        public string homephone { get; set; }
        public string companyphone { get; set; }
        public string company { get; set; }
        public string physicaldeliveryofficename { get; set; }
        public string department { get; set; }
        public string title { get; set; }
        public List<object> mailalternateaddress { get; set; }
        public List<string> logonhours { get; set; }
        public bool forcepasswordchange { get; set; }
        public bool showinaddressbook { get; set; }
        public string creationdate { get; set; }
    }

    public class EmailList : ApiResponseList<Mail>
    {
        public string mail { get; set; }
        public string displayname { get; set; }
        public string accounttype { get; set; }
        public string creationdate { get; set; }
    }
}
