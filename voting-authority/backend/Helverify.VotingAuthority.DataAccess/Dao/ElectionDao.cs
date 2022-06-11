using Helverify.VotingAuthority.DataAccess.Attributes;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Helverify.VotingAuthority.DataAccess.Dao
{
    [CollectionName("election")]
    public class ElectionDao: IEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public string Name { get; set; }

        public string Question { get; set; }

        public IList<ElectionOptionDao> Options { get; set; }

        public string P { get; set; }

        public string G { get; set; }

        public string? PublicKey { get; set; }
    }
}
