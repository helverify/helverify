namespace Helverify.VotingAuthority.DataAccess.Attributes
{
    /// <summary>
    /// Custom attribute to denote an entity's MongoDB collection.
    /// </summary>
    public class CollectionNameAttribute: Attribute
    {
        /// <summary>
        /// MongoDB collection name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">MongoDB collection name.</param>
        public CollectionNameAttribute(string name)
        {
            Name = name;
        }
    }
}
