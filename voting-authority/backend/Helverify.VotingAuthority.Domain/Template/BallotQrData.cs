namespace Helverify.VotingAuthority.Domain.Template;

/// <summary>
/// Contains the data needed for generating a QR code.
/// </summary>
public struct BallotQrData
{
    /// <summary>
    /// Identifier of the election
    /// </summary>
    public string ElectionId { get; set; }

    /// <summary>
    /// Identifier of the ballot
    /// </summary>
    public string BallotId { get; set; }

    /// <summary>
    /// Address of the election smart contract
    /// </summary>
    public string ContractAddress { get; set; }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="electionId">Identifier of the election</param>
    /// <param name="ballotId">Identifier of the ballot</param>
    public BallotQrData(string electionId, string ballotId, string contractAddress)
    {
        ElectionId = electionId;
        BallotId = ballotId;
        ContractAddress = contractAddress;
    }
}