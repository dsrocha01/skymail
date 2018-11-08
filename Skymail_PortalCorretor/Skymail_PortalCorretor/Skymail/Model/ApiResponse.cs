using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skymail_PortalCorretor.Model
{
    public class ApiResponse<T>
    {
        public bool success { get; set; }
        public T data { get; set; }
        public string message { get; set; }
    }

    public class ApiResponseList<T>
    {
        public bool success { get; set; }
        public List<T> data { get; set; }
        public string message { get; set; }
    }
}
