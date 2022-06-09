using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;

namespace Helverify.Cryptography.Encryption
{
    /// <summary>
    /// Base implementation for the ElGamal cryptosystem.
    /// </summary>
    public abstract class ElGamalBase: IElGamal
    {
        private readonly SecureRandom _secureRandom;

        /// <summary>
        /// Constructor
        /// </summary>
        protected ElGamalBase()
        {
            _secureRandom = new SecureRandom();
        }

        /// <inheritdoc cref="IElGamal.GetKey"/>
        public AsymmetricKeyParameter GetKey(string path)
        {
            using StreamReader streamReader = File.OpenText(path);
            PemReader pemReader = new PemReader(streamReader);
            AsymmetricKeyParameter key = (AsymmetricKeyParameter)pemReader.ReadObject();

            return key;
        }

        /// <inheritdoc cref="IElGamal.GetParameters"/>
        public DHParameters GetParameters(BigInteger p, BigInteger g)
        {
            return new DHParameters(p, g);
        }

        /// <inheritdoc cref="IElGamal.KeyGen"/>
        public AsymmetricCipherKeyPair KeyGen(BigInteger p, BigInteger g, SecureRandom random = null)
        {
            random ??= new SecureRandom();
            random.GenerateSeed(64);

            KeyGenerationParameters keyGenParams = new DHKeyGenerationParameters(random, GetParameters(p, g));

            DHKeyPairGenerator kpGenerator = new DHKeyPairGenerator();

            kpGenerator.Init(keyGenParams);

            return kpGenerator.GenerateKeyPair();
        }

        /// <inheritdoc cref="IElGamal.Encrypt"/>
        public ElGamalCipher Encrypt(int message, AsymmetricKeyParameter publicKeyParameter, BigInteger randomness = null)
        {
            DHPublicKeyParameters publicKey = (publicKeyParameter as DHPublicKeyParameters);

            BigInteger pk = publicKey.Y;
            BigInteger p = publicKey.Parameters.P;
            BigInteger g = publicKey.Parameters.G;

            BigInteger q = p.Subtract(BigInteger.One).Multiply(BigInteger.Two.ModInverse(p)).Mod(p);

            BigInteger r = randomness ?? new BigInteger(publicKey.Y.BitLength, _secureRandom).Mod(q);

            BigInteger m = PrepareMessage(message, p, g);

            BigInteger c = g.ModPow(r, p).Mod(p);
            BigInteger d = m.Multiply(pk.ModPow(r, p).Mod(p)).Mod(p);

            return new ElGamalCipher(c, d, r);
        }

        /// <inheritdoc cref="IElGamal.Decrypt"/>
        public int Decrypt(ElGamalCipher cipher, AsymmetricKeyParameter privateKeyParameter)
        {
            DHPrivateKeyParameters privateKey = privateKeyParameter as DHPrivateKeyParameters;

            BigInteger sk = privateKey?.X;
            BigInteger p = privateKey?.Parameters.P;
            BigInteger g = privateKey?.Parameters.G;

            BigInteger c = cipher.C;
            BigInteger d = cipher.D;

            BigInteger cPowSkInverse = c.ModPow(sk, p).ModInverse(p);

            BigInteger m = d.Multiply(cPowSkInverse).Mod(p);

            return RestoreMessage(m, p, g);
        }

        /// <inheritdoc cref="IElGamal.CombinePublicKeys"/>
        public DHPublicKeyParameters CombinePublicKeys(IList<DHPublicKeyParameters> publicKeys, DHParameters elGamalParameters)
        {
            BigInteger combinedPublicKey = BigInteger.One;

            foreach (DHPublicKeyParameters publicKey in publicKeys)
            {
                combinedPublicKey = combinedPublicKey.Multiply(publicKey.Y);
            }

            return new DHPublicKeyParameters(combinedPublicKey.Mod(elGamalParameters.P), elGamalParameters);
        }

        /// <inheritdoc cref="IElGamal.DecryptShare"/>
        public BigInteger DecryptShare(ElGamalCipher cipher, DHPrivateKeyParameters privateKey, BigInteger p)
        {
            return cipher.C.ModPow(privateKey.X, p).Mod(p);
        }

        /// <inheritdoc cref="IElGamal.CombineShares"/>
        public int CombineShares(IList<BigInteger> shares, BigInteger d, BigInteger p, BigInteger g)
        {
            BigInteger product = BigInteger.One;

            foreach (BigInteger share in shares)
            {
                product = product.Multiply(share);
            }

            BigInteger m = d.Multiply(product.ModInverse(p)).Mod(p);

            return RestoreMessage(m, p, g);
        }

        /// <summary>
        /// Transforms the message for encryption.
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="p">Prime</param>
        /// <param name="generator">Generator</param>
        /// <returns></returns>
        protected abstract BigInteger PrepareMessage(int message, BigInteger p, BigInteger generator);

        /// <summary>
        /// Restores the original message.
        /// </summary>
        /// <param name="m">Message</param>
        /// <param name="p">Prime</param>
        /// <param name="generator">Generator</param>
        /// <returns></returns>
        protected abstract int RestoreMessage(BigInteger m, BigInteger p, BigInteger generator);
    }
}
