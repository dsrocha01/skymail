using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Skymail_PortalCorretor.Util
{
    public class TokenManager
    {
        public static string EncodeToken(JwtPayload jwtPayload, string secret)
        {
            const string algorithm = SecurityAlgorithms.HmacSha256;

            var header = new JwtHeader
            {
                { "alg" , algorithm },
                { "typ", "JWT" }
            };

            var jwt = Base64Encode(JsonConvert.SerializeObject(header)) + "." + Base64Encode(JsonConvert.SerializeObject(jwtPayload));

            jwt += "." + Sign(jwt, secret);

            return jwt;
        }

        public static JwtPayload DecodeToken(string token, string secret)
        {
            var segments = token.Split('.');

            if (segments.Length != 3)
                throw new Exception("Token structure is incorrect!");

            JwtHeader header = JsonConvert.DeserializeObject(Encoding.UTF8.GetString(Base64Decode(segments[0])), typeof(JwtHeader));
            JwtPayload jwtPayload = JsonConvert.DeserializeObject(Encoding.UTF8.GetString(Base64Decode(segments[1])), typeof(JwtPayload));

            var rawSignature = segments[0] + '.' + segments[1];

            if (!Verify(rawSignature, secret, segments[2]))
                throw new Exception("Verification Failed");

            return jwtPayload;
        }

        private static bool Verify(string rawSignature, string secret, string signature)
        {
            return signature == Sign(rawSignature, secret);
        }

        private static string Sign(string str, string key)
        {
            //var encoding = new ASCIIEncoding();

            //SHA256Managed hash = new SHA256Managed();
            //byte[] signingBytes = hash.ComputeHash(encoding.GetBytes(key));

            //byte[] signature;
            //using (var crypto = new HMACSHA256(signingBytes))
            //{
            //    signature = crypto.ComputeHash(encoding.GetBytes(str));
            //}

            //return Base64Encode(signature);
            string message = str;
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] keyByte = Base64Decode(key);

            HMACSHA256 hmacsha256 = new HMACSHA256(keyByte);

            byte[] messageBytes = encoding.GetBytes(message);
            byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);
            return Base64Encode(hashmessage);
        }

        private static byte[] getBytes(string value)
        {
            return Encoding.UTF8.GetBytes(value);
        }

        public static string Base64Encode(dynamic obj)
        {
            Type strType = obj.GetType();

            var base64EncodedValue = Convert.ToBase64String(strType.Name.ToLower() == "string" ? Encoding.UTF8.GetBytes(obj) : obj);
            base64EncodedValue = base64EncodedValue.Split('=')[0]; // Remove any trailing '='s
            base64EncodedValue = base64EncodedValue.Replace('+', '-'); // 62nd char of encoding
            base64EncodedValue = base64EncodedValue.Replace('/', '_'); // 63rd char of encoding

            return base64EncodedValue;
        }

        public static dynamic Base64Decode(string str)
        {
            var base64DecodedValue = Convert.FromBase64String(str);

            return base64DecodedValue;
        }
    }
}
