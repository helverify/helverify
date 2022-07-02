namespace Helverify.VotingAuthority.Domain.Service;

/// <summary>
/// Represents a file to be zipped.
/// </summary>
public struct ArchiveFile
{
    /// <summary>
    /// File name including extension
    /// </summary>
    public string FileName { get; set; }

    /// <summary>
    /// Content of the file
    /// </summary>
    public byte[] Data { get; set; }
}