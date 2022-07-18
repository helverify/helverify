using Helverify.Cryptography.Helper;
using Org.BouncyCastle.Math;

namespace Helverify.Cryptography.Tests.Common
{
    internal class HashHelperTests
    {
        [Test]
        public void TestGetHash()
        {
            // arrange, act
            BigInteger hashInteger = HashHelper.GetHash(new BigInteger("1024", 10), BigInteger.Three, BigInteger.Two, BigInteger.Ten);
            
            // assert
            Assert.That(hashInteger, Is.EqualTo(new BigInteger("724", 10)));
        }
    }
}
