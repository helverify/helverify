using Org.BouncyCastle.Math;

namespace Helverify.Cryptography.Encryption
{
    public class StandardElGamal: ElGamalBase
    {
        protected override BigInteger PrepareM(BigInteger generator, BigInteger p, int message)
        {
            return new BigInteger(message.ToString(), 10);
        }

        protected override int RestoreMessage(BigInteger generator, BigInteger p, BigInteger m)
        {
            return m.IntValue;
        }
    }
}
