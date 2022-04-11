using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace TaoStore.Code
{
    public class Encode
    {
        public static string EncodePassword(string originalPassword)
        {
            //Declarations
            Byte[] originalBytes;
            Byte[] encodedBytes;
            MD5 md5;

            //Instantiate MD5CryptoServiceProvider, get bytes for original password and compute hash (encoded password)
            md5 = new MD5CryptoServiceProvider();
            originalBytes = ASCIIEncoding.ASCII.GetBytes(originalPassword);
            encodedBytes = md5.ComputeHash(originalBytes);
            byte[] result = md5.Hash;
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                stringBuilder.Append(result[i].ToString("x2"));
            }

            //Convert encoded bytes back to a 'readable' string

            return stringBuilder.ToString();
        }
    }
}