using Skymail_PortalCorretor.Skymail.Model;
using System;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http;
using Newtonsoft.Json;
using System.IO;
using System.IdentityModel.Tokens.Jwt;
using Skymail_PortalCorretor.Util;

namespace Skymail_PortalCorretor.Skymail
{
    public class CallApi
    {
        HttpClient client = null;

        void Start()
        {
            if (client == null)
            {
                client = new HttpClient();
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings["base_uri"]);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            }
        }

        public async Task<Token> CreateTokenAuth(User objUser)
        {
            Start();
            HttpResponseMessage response = await client.PostAsync(
                ConfigurationManager.AppSettings["uri_post_create_token_auth"], new StringContent(
                        JsonConvert.SerializeObject(objUser), Encoding.UTF8, "application/json"));

            string conteudo =
                    response.Content.ReadAsStringAsync().Result;
            Console.WriteLine(conteudo);
            if (response.StatusCode > 0)
            {
                Token token = JsonConvert.DeserializeObject<Token>(conteudo);
                if (token.success)
                {
                    // Associar o token aos headers do objeto
                    // do tipo HttpClient
                    client.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("bearer", GenerateTokenJWT(token).ToString());
                    Console.WriteLine($"Token generated: {token.data.jti}");
                    return token;
                }
                else
                {
                    token = JsonConvert.DeserializeObject<Token>(conteudo);
                    Console.WriteLine($"Token error: {token.message}");
                    return token;
                }
            }
            // return Token of the created resource.
            return null;
        }

        public object GenerateTokenJWT(Token objToken)
        {
            // // Define const Key this should be private secret key  stored in some safe place
            string key = ConfigurationManager.AppSettings["key_api"];
            var payloads = new JwtPayload
           {
               {
                    "jti" ,  objToken.data.jti
                }
            };

            var jwt = TokenManager.EncodeToken(payloads, key);

            return jwt;
        }


        public async Task<EmailList> GetListMail(string query)
        {
            EmailList objMailList = new EmailList();
            HttpResponseMessage response = client.GetAsync(ConfigurationManager.AppSettings["uri_get_list_mails"] +
                "?query=" + query).Result;
            try
            {
                objMailList = JsonConvert.DeserializeObject<EmailList>(response.Content.ReadAsStringAsync().Result);
            }
            catch (WebException ex)
            {
                new Uri(ConfigurationManager.AppSettings["base_uri"]);
                WebResponse errorResponse = ex.Response;
                using (Stream responseStream = errorResponse.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                    String errorText = reader.ReadToEnd();
                    objMailList = JsonConvert.DeserializeObject<EmailList>(errorText);
                    // log errorText
                }
                //throw;
            }

            Console.WriteLine();
            if (objMailList != null)
            {
                if (objMailList.success)
                {
                    Console.WriteLine(objMailList.data.Count + " email(s) encontrados");
                }
                else
                {
                    Console.WriteLine(objMailList.message);
                }
            }
            else
            {
                Console.WriteLine("Token provavelmente expirado!");
            }
            return objMailList;

        }

        public async Task<Mail> GetEmail(string query)
        {
            Mail objMail = new Mail();
            HttpResponseMessage response = client.GetAsync(ConfigurationManager.AppSettings["uri_get_mail"] +
                "/" + query).Result;
            try
            {
                objMail = JsonConvert.DeserializeObject<Mail>(response.Content.ReadAsStringAsync().Result);
            }
            catch (WebException ex)
            {
                new Uri(ConfigurationManager.AppSettings["base_uri"]);
                WebResponse errorResponse = ex.Response;
                using (Stream responseStream = errorResponse.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                    String errorText = reader.ReadToEnd();
                    objMail = JsonConvert.DeserializeObject<Mail>(errorText);
                    // log errorText
                }
                //throw;
            }

            if (response.StatusCode > 0)
            {
                if (objMail.success)
                {
                    Console.WriteLine(objMail.data.mail);
                    return objMail;
                }
                else
                {
                    Console.WriteLine($"Error: {objMail.message}");
                    return objMail;
                }
            }

            return objMail;

        }

        public async Task<Mail> PostCreateMail(Mail objMail)
        {
            HttpResponseMessage response = await client.PostAsync(
                ConfigurationManager.AppSettings["uri_post_create_mail"], new StringContent(
                        JsonConvert.SerializeObject(objMail), Encoding.UTF8, "application/json"));

            string conteudo =
                    response.Content.ReadAsStringAsync().Result;
            Console.WriteLine(conteudo);
            if (response.StatusCode > 0)
            {
                Mail objReturn = JsonConvert.DeserializeObject<Mail>(conteudo);
                if (objReturn.success)
                {
                    return objReturn;
                }
                else
                {
                    objReturn = JsonConvert.DeserializeObject<Mail>(conteudo);
                    Console.WriteLine($"Error: {objReturn.message}");
                    return objReturn;
                }
            }

            response.EnsureSuccessStatusCode();

            // return Token of the created resource.
            return null;
        }

        public async Task<Mail> PutAlterMail(Mail objMail)
        {
            HttpResponseMessage response = await client.PutAsync(
                ConfigurationManager.AppSettings["uri_put_alter_mail"] + "/" + objMail.data.mail, new StringContent(
                        JsonConvert.SerializeObject(objMail), Encoding.UTF8, "application/json"));

            string conteudo =
                    response.Content.ReadAsStringAsync().Result;
            Console.WriteLine(conteudo);
            if (response.StatusCode > 0)
            {
                Mail objReturn = JsonConvert.DeserializeObject<Mail>(conteudo);
                if (objReturn.success)
                {
                    return objReturn;
                }
                else
                {
                    objReturn = JsonConvert.DeserializeObject<Mail>(conteudo);
                    Console.WriteLine($"Error: {objReturn.message}");
                    return objReturn;
                }
            }

            response.EnsureSuccessStatusCode();

            // return Token of the created resource.
            return null;
        }


        public async Task<Mail> DeleteMail(Mail objMail)
        {
            HttpResponseMessage response = await client.DeleteAsync(
                ConfigurationManager.AppSettings["uri_delete_mail"] + "/" + objMail.data.mail);

            string conteudo =
                    response.Content.ReadAsStringAsync().Result;
            Console.WriteLine(conteudo);
            if (response.StatusCode > 0)
            {
                Mail objReturn = JsonConvert.DeserializeObject<Mail>(conteudo);
                if (objReturn.success)
                {
                    return objReturn;
                }
                else
                {
                    objReturn = JsonConvert.DeserializeObject<Mail>(conteudo);
                    Console.WriteLine($"Error: {objReturn.message}");
                    return objReturn;
                }
            }

            response.EnsureSuccessStatusCode();

            // return Token of the created resource.
            return null;
        }
    }
}
