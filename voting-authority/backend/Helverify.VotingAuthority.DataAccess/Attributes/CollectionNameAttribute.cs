namespace Helverify.VotingAuthority.DataAccess.Attributes
{
    public class CollectionNameAttribute: Attribute
    {
        public string Name { get; }

        public CollectionNameAttribute(string name)
        {
            Name = name;
        }
    }
}
