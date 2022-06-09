using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;

namespace Helverify.Cryptography.Encryption
{
    public abstract class ElGamalBase: IElGamal
    {
        private readonly SecureRandom _secureRandom;

        protected ElGamalBase()
        {
            _secureRandom = new SecureRandom();
        }

        /// <summary>
        /// https://stackoverflow.com/questions/11346200/reading-pem-rsa-public-key-only-using-bouncy-castle
        /// https://stackoverflow.com/questions/15629551/read-rsa-privatekey-in-c-sharp-and-bouncy-castle
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public AsymmetricKeyParameter GetKey(string path)
        {
            using StreamReader streamReader = File.OpenText(path);
            PemReader pemReader = new PemReader(streamReader);
            AsymmetricKeyParameter keyPair = (AsymmetricKeyParameter)pemReader.ReadObject();

            return keyPair;
        }

        public DHParameters GetParameters(BigInteger p, BigInteger g)
        {
            return new DHParameters(p, g);
        }

        public AsymmetricCipherKeyPair KeyGen(BigInteger p, BigInteger g, SecureRandom random = null)
        {
            random ??= new SecureRandom();
            random.GenerateSeed(64);

            KeyGenerationParameters keyGenParams = new DHKeyGenerationParameters(random, GetParameters(p, g));

            DHKeyPairGenerator kpGenerator = new DHKeyPairGenerator();

            kpGenerator.Init(keyGenParams);

            return kpGenerator.GenerateKeyPair();
        }

        public ElGamalCipher Encrypt(int message, AsymmetricKeyParameter publicKeyParameter, BigInteger randomness = null)
        {
            DHPublicKeyParameters publicKey = (publicKeyParameter as DHPublicKeyParameters);

            BigInteger pk = publicKey.Y;
            BigInteger p = publicKey.Parameters.P;
            BigInteger g = publicKey.Parameters.G;

            BigInteger q = p.Subtract(BigInteger.One).Multiply(BigInteger.Two.ModInverse(p)).Mod(p);

            BigInteger r = randomness ?? new BigInteger(publicKey.Y.BitLength, _secureRandom).Mod(q);

            BigInteger m = PrepareM(g, p, message);

            BigInteger c = g.ModPow(r, p).Mod(p);
            BigInteger d = m.Multiply(pk.ModPow(r, p).Mod(p)).Mod(p);

            return new ElGamalCipher(c, d, r);
        }

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

            return RestoreMessage(g, p, m);
        }

        public DHPublicKeyParameters CombinePublicKeys(IList<DHPublicKeyParameters> publicKeys, DHParameters elGamalParameters)
        {
            BigInteger combinedPublicKey = BigInteger.One;

            foreach (DHPublicKeyParameters publicKey in publicKeys)
            {
                combinedPublicKey = combinedPublicKey.Multiply(publicKey.Y);
            }

            return new DHPublicKeyParameters(combinedPublicKey.Mod(elGamalParameters.P), elGamalParameters);
        }

        public BigInteger DecryptShare(ElGamalCipher cipher, DHPrivateKeyParameters privateKey, BigInteger p)
        {
            return cipher.C.ModPow(privateKey.X, p).Mod(p);
        }

        public int CombineShares(IList<BigInteger> shares, BigInteger d, BigInteger p, BigInteger g)
        {
            BigInteger product = BigInteger.One;

            foreach (BigInteger share in shares)
            {
                product = product.Multiply(share);
            }

            BigInteger m = d.Multiply(product.ModInverse(p)).Mod(p);

            return RestoreMessage(g, p, m);
        }

        protected abstract BigInteger PrepareM(BigInteger generator, BigInteger p, int message);

        protected abstract int RestoreMessage(BigInteger generator, BigInteger p, BigInteger m);
    }
}
