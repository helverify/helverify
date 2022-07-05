using Helverify.VotingAuthority.Domain.Model;
using Helverify.VotingAuthority.Domain.Model.Decryption;

namespace Helverify.VotingAuthority.Application.Services;

/// <summary>
/// Facade for using Election domain logic.
/// </summary>
public interface IElectionService
{
    /// <summary>
    /// Persist a new election.
    /// </summary>
    /// <param name="election">Election to be persisted</param>
    /// <returns></returns>
    Task<Election> CreateAsync(Election election);

    /// <summary>
    /// Retrieves a stored election by its id.
    /// </summary>
    /// <param name="electionId">Election identifier</param>
    /// <returns></returns>
    Task<Election> GetAsync(string electionId);

    /// <summary>
    /// Retrieves all stored elections.
    /// </summary>
    /// <returns></returns>
    Task<IList<Election>> GetAsync();

    /// <summary>
    /// Updates an existing election.
    /// </summary>
    /// <param name="electionId">Election identifier</param>
    /// <param name="election">Current election</param>
    /// <returns></returns>
    Task<Election> UpdateAsync(string electionId, Election election);

    /// <summary>
    /// Deletes an election.
    /// </summary>
    /// <param name="electionId">Election identifier</param>
    /// <returns></returns>
    Task DeleteAsync(string electionId);

    /// <summary>
    /// Generates a new composite public key by calling the consensus nodes to generate a new keypair each and then combining the public keys into one.
    /// </summary>
    /// <param name="electionId">Election identifier</param>
    /// <returns></returns>
    Task<Election> GeneratePublicKey(string electionId);

    /// <summary>
    /// Deploys the smart contract for storing evidence about the specified election.
    /// </summary>
    /// <param name="electionId">Election identifier</param>
    /// <returns></returns>
    Task<Election> DeployElectionContract(string electionId);

    /// <summary>
    /// Calculates the final tally and publishes the evidence on IPFS and the smart contract.
    /// </summary>
    /// <param name="electionId">Election identifier</param>
    /// <returns></returns>
    Task<IList<DecryptedValue>> CalculateTally(string electionId);
}