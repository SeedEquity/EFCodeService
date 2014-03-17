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
        public void Test()
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
