using Org.BouncyCastle.Math;

namespace Helverify.Cryptography.Common
{
    /// <summary>
    /// Represents a Diffie-Hellman group
    /// </summary>
    public class DhGroup
    {
        /// <summary>
        /// Name of the group
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Public prime
        /// </summary>
        public BigInteger P { get; }

        /// <summary>
        /// Generator
        /// </summary>
        public BigInteger G { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="pHex">Hex string representing p</param>
        /// <param name="gHex">Hex string representing g</param>
        /// <param name="name">Name of the group</param>
        public DhGroup(string pHex, string gHex, string name)
        {
            P = new BigInteger(pHex, 16);
            G = new BigInteger(gHex, 16);
            Name = name;
        }
    }
}
