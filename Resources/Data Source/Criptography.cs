using System.Security.Cryptography;
using System.Text;

namespace Movies_app.Resources
{
    public static class Criptography
    {
        private static string key = "xxxxxxxxxxxxxxxx";
        private static string iv = "0000000000000000";

        public static byte[] GetKey()
        {
            return Encoding.ASCII.GetBytes(key);
        }

        public static byte[] GetIv()
        {
            return Encoding.ASCII.GetBytes(iv);
        }

        public static byte[] Encripty(string simpleText, byte[] key, byte[] iv)
        {
            byte[] cipheredtext;

            using (Aes aes = Aes.Create())
            {
                ICryptoTransform encryptor = aes.CreateEncryptor(key, iv);

                using (MemoryStream memoryStream = new())
                {
                    using (CryptoStream cryptoStream = new(memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new(cryptoStream))
                        {
                            streamWriter.Write(simpleText);
                        }

                        cipheredtext = memoryStream.ToArray();
                    }
                }
            }

            return cipheredtext;
        }
    }
}
