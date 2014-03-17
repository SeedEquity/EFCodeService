using System;
using System.Text.RegularExpressions;

namespace EFCodeService
{
    public static class Utilities
    {
        public static string GenerateRandomString(int size, Random random)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            const string numbers = "0123456789";
            var stringChars = new char[size];

            var consecutiveLetters = 0;
            for (var i = 0; i < stringChars.Length; i++)
            {
                int index;
                if (consecutiveLetters < 2)
                {
                    index = random.Next(chars.Length);
                    stringChars[i] = chars[index];
                    if (Char.IsLetter(chars[index]))
                    {
                        consecutiveLetters++;
                    }
                    else
                    {
                        consecutiveLetters = 0;
                    }
                }
                else
                {
                    index = random.Next(numbers.Length);
                    stringChars[i] = numbers[index];
                    consecutiveLetters = 0;
                }
            }

            return new String(stringChars);

        }

        public static string RemoveAccent(this string txt)
        {
            byte[] bytes = System.Text.Encoding.GetEncoding("Cyrillic").GetBytes(txt);
            return System.Text.Encoding.ASCII.GetString(bytes);
        }

        public static string GenerateSlug(this string phrase)
        {
            string str;
            try
            {
                str = phrase.RemoveAccent().ToLower();
                // invalid chars           
                str = Regex.Replace(str, @"[^a-z0-9\s-]", "");
                // convert multiple spaces into one space   
                str = Regex.Replace(str, @"\s+", " ").Trim();
                // cut and trim 
                str = str.Substring(0, str.Length <= 45 ? str.Length : 45).Trim();
                str = Regex.Replace(str, @"\s", "-"); // hyphens   
            }
            catch
            {
                str = "default";
            }
            return str;
        }

    }
}
