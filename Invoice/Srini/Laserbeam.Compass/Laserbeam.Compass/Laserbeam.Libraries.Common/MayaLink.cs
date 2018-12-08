using System;
using System.Security.Cryptography;
using System.Text;

namespace Laserbeam.Libraries.Common
{
    public class MayaLink
    {
        private const string SECRET = "Laserbeam-Compass";

        public static bool TryDecrypt(string message, out string decrypted)
        {
            var keyArray = new byte[24];
            var secretBytes = Encoding.UTF8.GetBytes(SECRET);
            try
            {
                var toDecryptArray = Convert.FromBase64String(message);
                Array.Copy(secretBytes, keyArray, secretBytes.Length);

                var tdes = new TripleDESCryptoServiceProvider
                {
                    Key = keyArray,
                    Mode = CipherMode.ECB,
                    Padding = PaddingMode.PKCS7
                };

                var cTransform = tdes.CreateDecryptor();
                var resultArray = cTransform.TransformFinalBlock(toDecryptArray, 0, toDecryptArray.Length);

                tdes.Clear();

                decrypted = Encoding.UTF8.GetString(resultArray);
            }
            catch //(Exception e)
            {
                //Console.Out.WriteLine("The string: {0} could not be decrypted because {1}", message, e);
                decrypted = null;
                return false;
            }

            return true;
        }

        public static bool TryEncrypt(string toEncrypt, out string encrypted)
        {
            var keyArray = new byte[24];
            var secretBytes = Encoding.UTF8.GetBytes(SECRET);

            try
            {
                var toEncryptArray = Encoding.UTF8.GetBytes(toEncrypt);
                Array.Copy(secretBytes, keyArray, secretBytes.Length);

                var tdes = new TripleDESCryptoServiceProvider
                {
                    Key = keyArray,
                    Mode = CipherMode.ECB,
                    Padding = PaddingMode.PKCS7
                };

                var cTransform = tdes.CreateEncryptor();
                var resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
                tdes.Clear();
                encrypted = Convert.ToBase64String(resultArray, 0, resultArray.Length);
            }
            catch (Exception)
            {
                //Console.Out.WriteLine("The string: {0} could not be encrypted because {1}", toEncrypt, e);
                encrypted = null;
                return false;
            }

            return true;
        }

        public static object TryEncrypt(string secretKey)
        {
            throw new NotImplementedException();
        }
    }
}
