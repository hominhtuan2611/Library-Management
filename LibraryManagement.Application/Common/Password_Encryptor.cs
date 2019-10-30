using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace LibraryManagement.Application.Common
{
    public class Password_Encryptor
    {
        public static string HashSHA1(string value)
        {
            var SHA1hash = SHA1.Create();
            var inputEncodingBytes = Encoding.ASCII.GetBytes(value);
            var hashString = SHA1hash.ComputeHash(inputEncodingBytes);

            var stringBuilder = new StringBuilder();
            for (var x = 0; x < hashString.Length; x++)
            {
                stringBuilder.Append(hashString[x].ToString("X2"));
            }
            return stringBuilder.ToString();
        }
    }
}
