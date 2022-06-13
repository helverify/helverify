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

        public string Name { get; set; } = string.Empty;

        public string Question { get; set; } = string.Empty;

        public IList<ElectionOptionDao> Options { get; set; } = new List<ElectionOptionDao>();

        public string P { get; set; } = string.Empty;

        public string G { get; set; } = string.Empty;

        public string? PublicKey { get; set; }
    }
}
