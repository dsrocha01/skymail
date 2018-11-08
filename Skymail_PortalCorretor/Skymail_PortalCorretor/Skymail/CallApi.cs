using Skymail_PortalCorretor.Skymail.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http;
using Newtonsoft.Json;
using System.IO;
using Microsoft.IdentityModel.Tokens;
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

        public async Task<Token> CreateTokenAuth(User user)
        {
            Start();
            HttpResponseMessage responseToken = await client.PostAsync(
                ConfigurationManager.AppSettings["uri_create_token_auth"], new StringContent(
                        JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json"));

            string conteudo =
                    responseToken.Content.ReadAsStringAsync().Result;
            Console.WriteLine(conteudo);
            if (responseToken.StatusCode == HttpStatusCode.OK)
            {
                Token token = JsonConvert.DeserializeObject<Token>(conteudo);
                if (token.success)
                {
                    // Associar o token aos headers do objeto
                    // do tipo HttpClient
                    client.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("bearer", getJWT(token).ToString());
                    return token;
                }
            }

            responseToken.EnsureSuccessStatusCode();

            // return Token of the created resource.
            return null;
        }

        public object getJWT(Token objToken)
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


        public async Task<String> ListMails(string query)
        {
            HttpResponseMessage response = client.GetAsync(ConfigurationManager.AppSettings["uri_list_mails"] +
                "?query=" + query).Result;

            Console.WriteLine();
            if (response.StatusCode == HttpStatusCode.OK)
                Console.WriteLine(response.Content.ReadAsStringAsync().Result);
            else
                Console.WriteLine("Token provavelmente expirado!");


            return response.Content.ReadAsStringAsync().Result;

        }

        public Email getListMails(object jwtToken, string query)
        {
            Email model = new Email();
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(client.BaseAddress + ConfigurationManager.AppSettings["uri_list_mails"] +
                "?query=" + query);
            request.Method = "GET";
            request.Headers.Add("Authorization", "Bearer " + jwtToken);
            request.Accept = "application/json";
            request.Headers["Authorization"] = "Bearer " + jwtToken;
            try
            {
                WebResponse response = request.GetResponse();
                using (Stream responseStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                    model = JsonConvert.DeserializeObject<Email>(reader.ReadToEnd());
                }
            }
            catch (WebException ex)
            {
                new Uri("https://api.skymail.net.br/v1/");
                WebResponse errorResponse = ex.Response;
                using (Stream responseStream = errorResponse.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                    String errorText = reader.ReadToEnd();
                    model = JsonConvert.DeserializeObject<Email>(errorText);
                    // log errorText
                }
                //throw;
            }

            return model;
        }
    }
}
