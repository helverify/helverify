using Org.BouncyCastle.Math;

namespace Helverify.Cryptography.Common
{
    public class DhGroup
    {
        public string Name { get; }
        public BigInteger P { get; }
        public BigInteger G { get; }
        public DhGroup(string pHex, string gHex, string name)
        {
            P = new BigInteger(pHex, 16);
            G = new BigInteger(gHex, 16);
            Name = name;
        }
    }
}
