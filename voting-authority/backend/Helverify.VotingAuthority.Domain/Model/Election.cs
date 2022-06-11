using Helverify.Cryptography.Encryption;
using Helverify.VotingAuthority.Domain.Extensions;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;

namespace Helverify.VotingAuthority.Domain.Model
{
    public class Election
    {
        private readonly IElGamal _elGamal;
        public Election()
        {
            _elGamal = new ExponentialElGamal();
        }

        public string? Id { get; set; }

        public string Name { get; set; }

        public string Question { get; set; }

        public IList<ElectionOption> Options { get; set; }

        public BigInteger P { get; set; }

        public BigInteger G { get; set; }

        public BigInteger? PublicKey { get; set; }

        public void CombinePublicKeys(IEnumerable<Registration> registrations)
        {
            IList<BigInteger> publicKeys = registrations.Select(r => new BigInteger(r.PublicKey, 16)).ToList();

            List<DHPublicKeyParameters> dhPublicKeys = publicKeys.Select(pk => new DHPublicKeyParameters(pk, DhParameters)).ToList();

            DHPublicKeyParameters electionPublicKey = _elGamal.CombinePublicKeys(dhPublicKeys, DhParameters);

            PublicKey = electionPublicKey.Y;
        }

        public ElGamalCipher Encrypt(int message)
        {
            return _elGamal.Encrypt(message, new DHPublicKeyParameters(PublicKey, DhParameters));
        }

        public int CombineShares(IList<string> decryptedShares, string cipherD)
        {
            IList<BigInteger> shares = decryptedShares.Select(s => s.ExportToBigInteger()).ToList();
            BigInteger d = cipherD.ExportToBigInteger();

            int message = _elGamal.CombineShares(shares, d, P, G);

            return message;
        }

        public DHParameters DhParameters => new (P, G);
    }
}
