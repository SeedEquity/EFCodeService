using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCodeService.Tests
{
    [TestClass]
    public class CryptoRandomTests
    {
        [TestMethod]
        public void CryptoRandomTest()
        {
            // When our CryptoRandom is instantiated in a tight loop, the results are *not* always the same.
            // This is good. It means that we can confidently use CryptoRandom to create unique Codes.
            var resultSet = new HashSet<int>();
            for (var i = 0; i < 100; i++)
            {
                var random = new CryptoRandom();
                var result = random.Next(1000);
                resultSet.Add(result);
            }
            if (resultSet.Count == 1)
            {
                Assert.Fail("ALL RESULTS WERE THE SAME. BAD TIMES.");
            }
        }

        [TestMethod]
        public void RandomTest()
        {
            // This is just a demonstratation that our CryptoRandom is better for our use-case than Random.
            // When Random is instantiated in a tight loop, the results are always the same.
            var resultSet = new HashSet<int>();
            for (var i = 0; i < 100; i++)
            {
                var random = new Random();
                var result = random.Next(1000);
                resultSet.Add(result);
            }
            Assert.AreEqual(1, resultSet.Count);
        }
    }
}
