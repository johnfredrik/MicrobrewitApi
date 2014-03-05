using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
namespace MicrobrewitApi.Util
{
    public class Encrypting
    {

        private static readonly byte[] Salt = Encoding.ASCII.GetBytes("05464545kM56Yc2");
        private static string _secretString;
        private const string Encrypted = "EAAAACZbrZRZ6i5ZzVbb8Ytu2RfFjvrXGmu6nwfUdNglwtN0";

        static void Main(string[] args)
        {
        
            Console.WriteLine("About to " + args[0]);
            Thread th;
            if (args[1] != null)
            {
                _secretString = args[1];
            }
            if(args[0] == "Login" )
            {
                th = new Thread(Login);
                th.SetApartmentState(ApartmentState.STA);
                th.Start();
                th.Join();
            } else if (args[0] == "Logout")
            {
                th = new Thread(Logout);
                th.SetApartmentState(ApartmentState.STA);
                th.Start();
                th.Join();
            } else
            {
                var encryptedPassword = Encrypt(args[0], args[1]);
                Console.WriteLine(encryptedPassword);
                Console.ReadLine();
            }




        }

        private static void Login()
        {
            var browser = new IE("https://gatsoft-hds.ihelse.net/mingat/frmLogin.aspx");
            
                LoginToMingat(browser);
                browser.Button(Find.ByName("btnIn")).Click();
            
        }

        private static void LoginToMingat(IE browser)
        {
            browser.Link(Find.ByName("overridelink")).Click();
            browser.TextField(Find.ByName("eUsername")).TypeText("jfa");
            try
            {
                browser.TextField(Find.ByName("ePassword")).TypeText(Decrypt(Encrypted,_secretString));
                browser.Button(Find.ByName("btLogin")).Click();
                browser.GoTo("https://gatsoft-hds.ihelse.net/MinGat/Flextime.aspx");
            }
            catch (CryptographicException cryptographicException)
            {
                Console.WriteLine("Wrong secret, try again");
                browser.Dispose();
            }
        }

        private static void Logout()
        {
            var browser = new IE("https://gatsoft-hds.ihelse.net/mingat/frmLogin.aspx");
            
            LoginToMingat(browser);
            browser.Button(Find.ByName("btnOut")).Click();
                
            
        }

        private static string Encrypt(string plainText, string sharedSecret)
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

        private static string Decrypt(string cipherText, string sharedSecret)
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
    }
}

