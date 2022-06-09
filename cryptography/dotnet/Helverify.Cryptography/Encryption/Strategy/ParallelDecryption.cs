using Org.BouncyCastle.Math;

namespace Helverify.Cryptography.Encryption.Strategy
{
    /// <summary>
    /// Parallel iteration of the exponents of g to find correct decryption.
    /// </summary>
    internal class ParallelDecryption: IDecryptionStrategy
    {
        /// <inheritdoc cref="IDecryptionStrategy.Decrypt"/>
        public int Decrypt(BigInteger cipher, BigInteger p, BigInteger g)
        {
            int exp = -1;

            bool found;

            CancellationTokenSource tokenSource = new CancellationTokenSource();

            try
            {
                Parallel.For(0, 10000000, new ParallelOptions { MaxDegreeOfParallelism = 4, CancellationToken = tokenSource.Token }, (i, _) =>
                {
                    if (i % 10000 == 0)
                    {
                        Console.WriteLine($"Thread: {Task.CurrentId} Exponent: {i}");
                    }

                    BigInteger exponent = new BigInteger(i.ToString(), 10);
                    BigInteger candidate = g.ModPow(exponent, p).Mod(p);

                    found = candidate.Equals(cipher);

                    if (found)
                    {
                        exp = i;
                        Console.WriteLine($"Result found: {exp}");
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
