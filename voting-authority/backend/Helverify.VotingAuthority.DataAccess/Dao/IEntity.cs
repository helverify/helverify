namespace Helverify.VotingAuthority.DataAccess.Dao;

/// <summary>
/// Common interface for all DB entities to enforce the specification of an Id attribute.
/// </summary>
public interface IEntity
{
    /// <summary>
    /// Unique identifier
    /// </summary>
    string Id { get; set; }
}