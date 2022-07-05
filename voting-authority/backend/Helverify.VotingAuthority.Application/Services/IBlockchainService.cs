using Helverify.VotingAuthority.Domain.Model.Blockchain;

namespace Helverify.VotingAuthority.Application.Services;

/// <summary>
/// Facade for Blockchain domain logic.
/// </summary>
public interface IBlockchainService
{
    /// <summary>
    /// Initializes a new Proof-of-Authority Blockchain with the specified consensus nodes.
    /// </summary>
    /// <param name="blockchain">Blockchain to be initialized</param>
    /// <returns></returns>
    Task<Blockchain> Initialize(Blockchain blockchain);

    /// <summary>
    /// Returns the currently active Blockchain instance.
    /// </summary>
    /// <returns></returns>
    Task<Blockchain?> GetInstance();
}