using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BOL.Helper
{
    public static class TripleDESCryptography
    {
        public static readonly string TripleDESKey = ConfigurationManager.AppSettings["TripleDES_Key"];
        public static readonly Encoding Encoder = Encoding.UTF8;

        public static string Generate3DESKey()
        {
            TripleDESCryptoServiceProvider TDES = new TripleDESCryptoServiceProvider();
            string key = Convert.ToBase64String(TDES.Key);

            return key;
        }
        public static string Encrypt(string plainText)
        {
            var des = CreateDes(TripleDESKey);
            var ct = des.CreateEncryptor();
            var input = Encoding.UTF8.GetBytes(plainText);
            var output = ct.TransformFinalBlock(input, 0, input.Length);
            return Convert.ToBase64String(output);
        }

        public static string Decrypt(string cypherText)
        {
            var des = CreateDes(TripleDESKey);
            var ct = des.CreateDecryptor();
            var input = Convert.FromBase64String(cypherText);
            var output = ct.TransformFinalBlock(input, 0, input.Length);
            return Encoding.UTF8.GetString(output);
        }

        public static TripleDES CreateDes(string key)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            TripleDES des = new TripleDESCryptoServiceProvider();
            var desKey = md5.ComputeHash(Encoding.UTF8.GetBytes(key));
            des.Key = desKey;
            des.IV = new byte[des.BlockSize / 8];
            des.Padding = PaddingMode.PKCS7;
            des.Mode = CipherMode.ECB;
            return des;
        }
    }
}
