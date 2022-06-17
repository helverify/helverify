using Helverify.VotingAuthority.DataAccess.Attributes;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Helverify.VotingAuthority.DataAccess.Dao
{
    [CollectionName("registration")]
    public class RegistrationDao : IEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public Uri? Endpoint { get; set; }
        public string? ElectionId { get; set; }
        public string? PublicKey { get; set; }
        public string AccountAddress { get; set; }
    }
}
