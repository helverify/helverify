using Org.BouncyCastle.Math;

namespace Helverify.Cryptography.Encryption.Strategy
{
    /// <summary>
    /// Parallel iteration of the exponents of g to find correct decryption.
    /// </summary>
    public class ParallelDecryption: IDecryptionStrategy
    {
        /// <inheritdoc cref="IDecryptionStrategy.Decrypt"/>
        public int Decrypt(BigInteger cipher, BigInteger p, BigInteger g)
        {
            int exp = -1;

            bool found;

            CancellationTokenSource tokenSource = new CancellationTokenSource();

            try
            {
                Parallel.For(0, 10000000, new ParallelOptions { CancellationToken = tokenSource.Token }, (i, _) =>
                {
                    BigInteger exponent = new BigInteger(i.ToString(), 10);
                    BigInteger candidate = g.ModPow(exponent, p).Mod(p);

                    found = candidate.Equals(cipher);

                    if (found)
                    {
                        exp = i;
                        tokenSource.Cancel();
                    }

                    if (exp > 10000000)
                    {
                        throw new ArgumentOutOfRangeException(nameof(exp));
                    }
                });
            }
            catch (OperationCanceledException)
            {
                return exp;
            }

            return exp;
        }
    }
}
