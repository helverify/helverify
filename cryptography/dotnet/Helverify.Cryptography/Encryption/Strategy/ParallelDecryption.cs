using Org.BouncyCastle.Math;

namespace Helverify.Cryptography.Encryption.Strategy
{
    internal class ParallelDecryption: IDecryptionStrategy
    {
        public int Decrypt(BigInteger generator, BigInteger p, BigInteger m)
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
                    BigInteger candidate = generator.ModPow(exponent, p).Mod(p);

                    found = candidate.Equals(m);

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
