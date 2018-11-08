using Skymail_PortalCorretor.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skymail_PortalCorretor.Skymail.Model
{
    public class Email : ApiResponse<Email>
    {
        public string mail { get; set; }
        public string displayname { get; set; }
        public string accounttype { get; set; }
        public string creationdate { get; set; }
    }
}
