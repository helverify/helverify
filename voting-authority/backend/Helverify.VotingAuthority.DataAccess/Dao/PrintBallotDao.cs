using Helverify.VotingAuthority.DataAccess.Attributes;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Helverify.VotingAuthority.DataAccess.Dao
{
    [CollectionName("registration")]

    public class PrintBallotDao : IEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

    }
}
