using Helverify.Cryptography.ZeroKnowledge;
using Helverify.VotingAuthority.DataAccess.Attributes;
using Helverify.VotingAuthority.DataAccess.Dao;
using Helverify.VotingAuthority.DataAccess.Dto;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Org.BouncyCastle.Math;


namespace Helverify.VotingAuthority.Domain.Model
{
    [CollectionName("registration")]
    public class Registration: IEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public string Name { get; set; }
        public Uri Endpoint { get; set; }
        public string? ElectionId { get; set; }
        public string? PublicKey { get; set; }

        public void SetPublicKey(PublicKeyDto publicKeyDto, Election election)
        {
            ProofOfPrivateKeyOwnership proof = new ProofOfPrivateKeyOwnership(
                new BigInteger(publicKeyDto.ProofOfPrivateKey.C, 16), new BigInteger(publicKeyDto.ProofOfPrivateKey.D, 16));

            bool isValid = proof.Verify(new BigInteger(publicKeyDto.PublicKey, 16), new BigInteger(election.P, 16),
                new BigInteger(election.G, 16));

            if (!isValid)
            {
                throw new Exception("Consensus node has no private key matching the specified public key");
            }

            PublicKey = publicKeyDto.PublicKey;
        }
    }
}
