using System.Collections;
using Helverify.Cryptography.Encryption;
using Helverify.VotingAuthority.DataAccess.Attributes;
using Helverify.VotingAuthority.DataAccess.Dao;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;

namespace Helverify.VotingAuthority.Domain.Model
{
    [CollectionName("election")]
    public class Election: IEntity
    {
        private readonly IElGamal _elGamal;
        public Election()
        {
            _elGamal = new ExponentialElGamal();
        }

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public string Name { get; set; }

        public string Question { get; set; }

        public IList<ElectionOption> Options { get; set; }

        public string P { get; set; }

        public string G { get; set; }

        public string? PublicKey { get; set; }

        public void CombinePublicKeys(IEnumerable<Registration> registrations)
        {
            IList<BigInteger> publicKeys = registrations.Select(r => new BigInteger(r.PublicKey, 16)).ToList();

            List<DHPublicKeyParameters> dhPublicKeys = publicKeys.Select(pk => new DHPublicKeyParameters(pk, DhParameters)).ToList();

            DHPublicKeyParameters electionPublicKey = _elGamal.CombinePublicKeys(dhPublicKeys, DhParameters);

            PublicKey = electionPublicKey.Y.ToString(16);
        }

        public ElGamalCipher Encrypt(int message)
        {
            return _elGamal.Encrypt(message, new DHPublicKeyParameters(new BigInteger(PublicKey, 16), DhParameters));
        }

        public int CombineShares(IList<string> decryptedShares, string cipherD)
        {
            IList<BigInteger> shares = decryptedShares.Select(StringToBigInteger).ToList();
            BigInteger d = StringToBigInteger(cipherD);

            int message = _elGamal.CombineShares(shares, d, StringToBigInteger(P), StringToBigInteger(G));

            return message;
        }

        public DHParameters DhParameters => new (new BigInteger(P, 16), new BigInteger(G, 16));

        private BigInteger StringToBigInteger(string str) => new BigInteger(str, 16);
    }
}
