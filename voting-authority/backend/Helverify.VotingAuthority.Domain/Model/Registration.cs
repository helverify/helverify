using Helverify.VotingAuthority.DataAccess.Attributes;
using Helverify.VotingAuthority.DataAccess.Dao;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

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
    }
}
