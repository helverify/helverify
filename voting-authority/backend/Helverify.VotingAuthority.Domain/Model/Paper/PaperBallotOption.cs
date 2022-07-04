using Org.BouncyCastle.Math;

namespace Helverify.VotingAuthority.Domain.Model.Paper;

/// <summary>
/// Represents an option / candidate in the paper ballot.
/// </summary>
public sealed class PaperBallotOption
{
    /// <summary>
    /// Name of the option / candidate.
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// Short code of the first virtual ballot
    /// </summary>
    public string ShortCode1 { get; }

    /// <summary>
    /// Short code of the second virtual ballot.
    /// </summary>
    public string ShortCode2 { get; }

    /// <summary>
    /// Random values used to encrypt the first ballot
    /// </summary>
    public IList<BigInteger> RandomValues1 { get; }

    /// <summary>
    /// Random values used to encrypt the second ballot
    /// </summary>

    public IList<BigInteger> RandomValues2 { get; }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="name">Name of the option / candidate</param>
    /// <param name="shortCode1">Short code of the first virtual ballot</param>
    /// <param name="shortCode2">Short code of the second virtual ballot</param>
    public PaperBallotOption(string name, string shortCode1, string shortCode2, IList<BigInteger> randomValues1, IList<BigInteger> randomValues2)
    {
        Name = name;
        ShortCode1 = shortCode1;
        ShortCode2 = shortCode2;
        RandomValues1 = randomValues1;
        RandomValues2 = randomValues2;
    }

    /// <summary>
    /// Clears the plain text name of the option.
    /// </summary>
    public void ClearConfidential()
    {
        Name = string.Empty;
    }

    /// <summary>
    /// Returns the randomness used for encrypting the ballot with the specified index.
    /// </summary>
    /// <param name="ballotIndex">0 or 1</param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public IList<BigInteger> GetRandomness(int ballotIndex)
    {
        if (ballotIndex is not (0 or 1))
        {
            throw new ArgumentException("Index must be either 0 or 1", nameof(ballotIndex));
        }
        
        return ballotIndex == 0 ? RandomValues1 : RandomValues2;
    }

    /// <summary>
    /// Returns the short code of the specified ballot option.
    /// </summary>
    /// <param name="ballotIndex">0 or 1</param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public string GetShortCode(int ballotIndex)
    {
        if (ballotIndex is not (0 or 1))
        {
            throw new ArgumentException("Index must be either 0 or 1", nameof(ballotIndex));
        }

        return ballotIndex == 0 ? ShortCode1 : ShortCode2;
    }
}