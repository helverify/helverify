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
    /// Constructor
    /// </summary>
    public BallotQrData()
    {
        ElectionId = string.Empty;
        BallotId = string.Empty;
    }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="electionId">Identifier of the election</param>
    /// <param name="ballotId">Identifier of the ballot</param>
    public BallotQrData(string electionId, string ballotId)
    {
        ElectionId = electionId;
        BallotId = ballotId;
    }
}