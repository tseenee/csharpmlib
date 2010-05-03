using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace MLib.DataManipulation
{
    public static class Encryption
    { 
        #region DES

        static byte[] bytes = ASCIIEncoding.ASCII.GetBytes("ZeroCool");

        /// <summary>
        /// Uses DES encryption to encrypt a string
        /// </summary>
        /// <param name="String">String to be encrypted</param>
        /// <returns>Encrypted string</returns>
        public static string Encrypt(string Value)
        {
            if (String.IsNullOrEmpty(Value))
            {
                throw new ArgumentNullException("The string which needs to be encrypted can not be null.");
            }
            DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoProvider.CreateEncryptor(bytes, bytes), CryptoStreamMode.Write);

            StreamWriter writer = new StreamWriter(cryptoStream);
            writer.Write(Value);
            writer.Flush();
            cryptoStream.FlushFinalBlock();
            writer.Flush();

            return Convert.ToBase64String(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
        }

        /// <summary>
        /// Uses DES encryption to decrypt a string
        /// </summary>
        /// <param name="String">String to be decrypted</param>
        /// <returns>Decrypted string</returns>
        public static string Decrypt(string EncrypedString)
        {
            if (String.IsNullOrEmpty(EncrypedString))
            {
                throw new ArgumentNullException("The string which needs to be decrypted can not be null.");
            }

            DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
            MemoryStream memoryStream = new MemoryStream(Convert.FromBase64String(EncrypedString));
            CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoProvider.CreateDecryptor(bytes, bytes), CryptoStreamMode.Read);
            StreamReader reader = new StreamReader(cryptoStream);

            return reader.ReadToEnd();
        }
        #endregion

        #region MD5
        /// <summary>
        /// Encrypts a string with MD5 encryption
        /// </summary>
        /// <param name="Value">String to be encrypted</param>
        /// <returns>Encrypted string</returns>
        public static string MD5(string Value)
        {
            MD5CryptoServiceProvider x = new MD5CryptoServiceProvider();
            byte[] data = System.Text.Encoding.ASCII.GetBytes(Value);
            data = x.ComputeHash(data);
            string ret = "";
            for (int i = 0; i < data.Length; i++)
                ret += data[i].ToString("x2").ToLower();
            return ret;
        }

        /// <summary>
        /// Compares a string with MD5 encryption
        /// </summary>
        /// <param name="Value">Compare value</param>
        /// <param name="MD5Value">Encrypted value</param>
        /// <returns>Comparement result</returns>
        public static bool CompareMD5(string Value, string MD5Value)
        {
            MD5CryptoServiceProvider x = new MD5CryptoServiceProvider();
            byte[] data = System.Text.Encoding.ASCII.GetBytes(Value);
            data = x.ComputeHash(data);
            string ret = "";
            for (int i = 0; i < data.Length; i++)
                ret += data[i].ToString("x2").ToLower();


            if (MD5(Value) == MD5Value)
                return true;
            else
                return false;
        }

        #endregion

    }
}
