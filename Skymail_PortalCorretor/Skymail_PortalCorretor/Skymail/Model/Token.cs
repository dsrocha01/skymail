using Skymail_PortalCorretor.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skymail_PortalCorretor.Skymail.Model
{
    public class Token : ApiResponse<Token>
    {
        public string jti { get; set; }
    }
}
