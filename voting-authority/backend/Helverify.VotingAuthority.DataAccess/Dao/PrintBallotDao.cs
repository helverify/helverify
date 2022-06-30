using Helverify.VotingAuthority.DataAccess.Attributes;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Helverify.VotingAuthority.DataAccess.Dao
{
    [CollectionName("printballot")]

    public class PrintBallotDao : IEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string BallotId { get; set; }

        public bool Printed { get; set; }

        public IList<PrintOptionDao> Options { get; set; } = new List<PrintOptionDao>();
    }
}
