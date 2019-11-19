using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryManagement.Application.Common
{
    public class Random_Generator
    {
        public static string GenerateString(int size)
        {
            Random rand = new Random();

            const string Alphabet = "0123456789";

            char[] chars = new char[size];
            for (int i = 0; i < size; i++)
            {
                chars[i] = Alphabet[rand.Next(Alphabet.Length)];
            }
            return new string(chars);
        }
    }
}
