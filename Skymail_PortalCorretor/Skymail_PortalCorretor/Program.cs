using Skymail_PortalCorretor.Skymail;
using Skymail_PortalCorretor.Skymail.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skymail_PortalCorretor
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.Write(new Dao.Contact().GetListContact().Count);


            RunAsync().GetAwaiter().GetResult();
        }

        static async Task RunAsync()
        {
            //Generate and return Token
            
            User objUser = new User();
            objUser.username = ConfigurationManager.AppSettings["login_api"];
            objUser.password = ConfigurationManager.AppSettings["password_api"];
            CallApi callApi = new CallApi();
            Token token = await callApi.CreateTokenAuth(objUser);
            Console.WriteLine($"Token generated {token.data.jti}");


            //List mail by query
            var str = callApi.ListMails("eliel");
            Console.WriteLine(str);
        }
    }
}
