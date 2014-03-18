using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EFCodeService
{
    public class CryptoRandom
    {
        private readonly RNGCryptoServiceProvider _provider = new RNGCryptoServiceProvider();

        public int Next(int maxValue)
        {
            var randomBytes = new byte[sizeof(int)];
            _provider.GetBytes(randomBytes);
            var result = BitConverter.ToInt32(randomBytes, 0) % maxValue;
            if (result < 0)
            {
                result *= -1;
            }
            return result;
        }
    }
}
