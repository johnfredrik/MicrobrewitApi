using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.IdentityModel.Tokens;
using System.Security.Claims;
using log4net;
using System.ServiceModel.Security.Tokens;
using Microbrewit.Model;
using System.IdentityModel.Protocols.WSTrust;
using ServiceStack.Redis;
using System.Configuration;

namespace Microbrewit.Api.Util
{
    public class Encrypting
    {
        #region Private Fields
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly byte[] Salt = Encoding.ASCII.GetBytes(@"Èõó¬7SýÉ•wäÚx:þÞ]^ì¶—~9a§");
        private static readonly JwtSecurityTokenHandler tokenhandler = new JwtSecurityTokenHandler();
        private static readonly int expire = int.Parse(ConfigurationManager.AppSettings["expire"]);
        private static readonly string redisStore = ConfigurationManager.AppSettings["redis"];
        #endregion

        public static string Encrypt(string plainText, string sharedSecret)
        {

            string outString = null;
            RijndaelManaged aesAlg = null;

            try
            {
                Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(sharedSecret, Salt);

                aesAlg = new RijndaelManaged();
                aesAlg.Key = key.GetBytes(aesAlg.KeySize/8);

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    msEncrypt.Write(BitConverter.GetBytes(aesAlg.IV.Length), 0, sizeof (int));
                    msEncrypt.Write(aesAlg.IV, 0, aesAlg.IV.Length);
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                    }
                    outString = Convert.ToBase64String(msEncrypt.ToArray());
                }


            }
            finally
            {
                if (aesAlg != null)
                {
                    aesAlg.Clear();
                }
            }

            return outString;

        }

        public static string Decrypt(string cipherText, string sharedSecret)
        {

            Rijndael aesAlg = null;
            string plainText = null;

            try
            {
                Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(sharedSecret, Salt);

                byte[] bytes = Convert.FromBase64String(cipherText);
                using (MemoryStream msDecrypt = new MemoryStream(bytes))
                {
                    aesAlg = new RijndaelManaged();
                    aesAlg.Key = key.GetBytes(aesAlg.KeySize/8);
                    aesAlg.IV = ReadByteArray(msDecrypt);

                    ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            plainText = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
            finally
            {
                if (aesAlg != null)
                {
                    aesAlg.Clear();
                }
            }
            return plainText;
        }

        private static byte[] ReadByteArray(Stream s)
    {
        byte[] rawLength = new byte[sizeof(int)];
        if (s.Read(rawLength, 0, rawLength.Length) != rawLength.Length)
        {
            throw new SystemException("Stream did not contain properly formatted byte array");
        }

        byte[] buffer = new byte[BitConverter.ToInt32(rawLength, 0)];
        if (s.Read(buffer, 0, buffer.Length) != buffer.Length)
        {
            throw new SystemException("Did not read byte array properly");
        }

        return buffer;
    }

        public static byte[] GenerateRandomBytes(int length)
        {
            // Create a buffer
            byte[] randBytes;

            if (length >= 1)
            {
                randBytes = new byte[length];
            }
            else
            {
                randBytes = new byte[1];
            }

            // Create a new RNGCryptoServiceProvider.
            System.Security.Cryptography.RNGCryptoServiceProvider rand =
                 new System.Security.Cryptography.RNGCryptoServiceProvider();

            // Fill the buffer with random bytes.
            rand.GetBytes(randBytes);

            // return the bytes.
            return randBytes;
        }

        public static string JWTDecrypt(User user)
        {
            Log.Debug("Username:" + user.Username);
            //var symmetricKey = GenerateRandomBytes(256 / 8);
          
            var now = DateTime.UtcNow;
            var signingCred = new SigningCredentials(new InMemorySymmetricSecurityKey(Salt),
                                    "http://www.w3.org/2001/04/xmldsig-more#hmac-sha256",
                                    "http://www.w3.org/2001/04/xmlenc#sha256");

            var jwtHeader = new JwtHeader(signingCred);
                                           
                         
            JwtSecurityToken jwtToken = new JwtSecurityToken
            (issuer: "http://issuer.com", audience: "http://localhost"
            , claims: new List<Claim>() { new Claim("username", user.Username) }
            , lifetime: new Lifetime(DateTime.UtcNow, DateTime.UtcNow.AddMinutes(expire))
            , signingCredentials: signingCred);
          
            string tokenString = tokenhandler.WriteToken(jwtToken);
            return tokenString;
        }
       
        public static void JWTValidation(string tokenString)
        {
            
            var validationParameters = new TokenValidationParameters()
            {
                AllowedAudience = "http://localhost",
                SigningToken = new BinarySecretSecurityToken(Salt),
                ValidIssuer = "http://issuer.com",                                
            };
            Log.Debug("Now time: " + DateTime.UtcNow.ToString());
            var principal = tokenhandler.ValidateToken(tokenString, validationParameters);
            using (var redisClient = new RedisClient(redisStore))
            {
                if (!principal.Identities.First().Claims.Any(c => c.Type == "username" && c.Value == redisClient.GetValue(tokenString)))
                {
                    throw new SecurityTokenValidationException("No token found in redis store");
                }
               
            }
        }

        public static void JWTInvalidateToken(string tokenString)
        {
            using (var redisClient = new RedisClient(redisStore))
            {
                redisClient.Del(tokenString);
            } 
        }


    }
}

