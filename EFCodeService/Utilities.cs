using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace EFCodeService
{
    public static class Utilities
    {
        public static string GenerateRandomString(int size, CryptoRandom random)
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

        public static string RemoveControlCharacters(this string inString)
        {
            if (inString == null) return null;
            var newString = new StringBuilder();
            foreach (var ch in inString.Where(ch => !char.IsControl(ch) && !Path.GetInvalidPathChars().Contains(ch)))
            {
                newString.Append(ch);
            }
            return newString.ToString();
        }

        public static string GenerateSlug(this string phrase)
        {
            string str;
            try
            {
                //invalid chars
                str = phrase.RemoveControlCharacters().ToLower();
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
