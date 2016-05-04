using System.IO;
using System.Linq;
using System.Runtime;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data.Entity;
using Moq;

namespace EFCodeService.Tests
{
    [TestClass]
    public class EFCodeServiceTests
    {
        [TestMethod]
        public void EFCodeServiceTest()
        {
            var mockDb = new Mock<EfDbContext>();

            mockDb.Setup(s => s.Set(It
                .IsAny<Type>()))
                .Returns(It.IsAny<DbSet<EFCode>>);

            mockDb.Setup(s => s.SaveChanges())
                .Returns(0);
            
            var svc = new EFCodeService(mockDb.Object, 5);

            for (var i = 0; i < 100; i++)
            {
                var mycode = svc.GenerateCode();
                if (ContainsThreeOrMoreConsecutiveLetters(mycode))
                {
                    Assert.Fail();
                }
            }
            var code = "";
            var slug = "";
            svc.Populate("Светлана" + " Савкина",
                     c => code = c,
                     s => slug = s);
            Assert.IsNotNull(slug);
            Assert.IsFalse(string.IsNullOrWhiteSpace(slug));
            Assert.AreEqual("светлана-савкина", slug);

            svc.Populate("aaron mottern | tech entrepreneur",
                c => code = c,
                s => slug = s);
            Assert.IsNotNull(slug);
            Assert.IsFalse(string.IsNullOrWhiteSpace(slug));
            foreach (var c in Path.GetInvalidPathChars())
            {
                Assert.IsFalse(slug.ToCharArray().Contains(c));
            }
            Assert.AreEqual("aaron-mottern-tech-entrepreneur", slug);
        }

        private static bool ContainsThreeOrMoreConsecutiveLetters(string code)
        {
            var consecutiveLetters = 0;
            foreach (var c in code)
            {
                if (consecutiveLetters >= 3)
                {
                    return true;
                }
                if (Char.IsLetter(c))
                {
                    consecutiveLetters++;
                }
                else
                {
                    consecutiveLetters = 0;
                }
            }
            return false;
        }
    }

    public class EfDbContext : DbContext
    {
        public virtual IDbSet<EFCode> Codes { get; set; }

        public new virtual IDbSet<EFCode> Set(Type t)
        {
            return Set<EFCode>();
        } 
    }
}
