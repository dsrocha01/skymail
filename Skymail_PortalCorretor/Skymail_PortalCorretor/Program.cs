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

            try
            {
                // Start a task - calling an async function in this example
                RunAsync().GetAwaiter().GetResult();
            }
            catch (Exception ex)  //Exceptions here or in the function will be caught here
            {
                Console.WriteLine("Exception: " + ex.Message);
            }

        }

        static async Task RunAsync()
        {
            //Generate and return Token
            User objUser = new User();
            objUser.username = ConfigurationManager.AppSettings["login_api"];
            objUser.password = ConfigurationManager.AppSettings["password_api"];
            CallApi callApi = new CallApi();
            Token token = await callApi.CreateTokenAuth(objUser);

            //List mail by query
            var strGetListMail = callApi.GetListMail("marcus");
            Console.WriteLine(strGetListMail);

            Console.WriteLine();

            //Get by mail
            var strGetMail = callApi.GetEmail("integracao.skymail@bbrk.com.br");
            Console.WriteLine(strGetMail);

            //Create mail
            Mail objMail = new Mail();
            objMail.data.mail = "";

            Console.ReadLine();
        }
    }
}
