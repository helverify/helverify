using Helverify.VotingAuthority.DataAccess.Attributes;
using Helverify.VotingAuthority.DataAccess.Dao;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Helverify.VotingAuthority.Domain.Model
{
    [CollectionName("election")]
    public class Election: IEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public string Name { get; set; }

        public string Question { get; set; }

        public IList<ElectionOption> Options { get; set; }

        public string P { get; set; }

        public string G { get; set; }
    }
}
