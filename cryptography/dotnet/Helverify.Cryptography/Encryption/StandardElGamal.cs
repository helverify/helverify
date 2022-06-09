using Org.BouncyCastle.Math;

namespace Helverify.Cryptography.Encryption
{
    /// <summary>
    /// Standard (multiplicative) ElGamal implementation.
    /// </summary>
    public class StandardElGamal: ElGamalBase
    {
        /// <inheritdoc cref="ElGamalBase.PrepareMessage"/>
        protected override BigInteger PrepareMessage(int message, BigInteger p, BigInteger generator)
        {
            return new BigInteger(message.ToString(), 10);
        }

        /// <inheritdoc cref="ElGamalBase.RestoreMessage"/>
        protected override int RestoreMessage(BigInteger m, BigInteger p, BigInteger generator)
        {
            return m.IntValue;
        }
    }
}
