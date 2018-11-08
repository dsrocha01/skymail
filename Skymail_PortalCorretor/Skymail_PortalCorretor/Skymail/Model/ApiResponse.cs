using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skymail_PortalCorretor.Model
{
    public class ApiResponse<T>
    {
        private bool _success;
        private T _data;
        private string _message;

        public ApiResponse()
        {
        }

        public bool success { get => _success; set => _success = value; }
        public T data { get => _data; set => _data = value; }
        public string message { get => _message; set => _message = value; }
    }
}
