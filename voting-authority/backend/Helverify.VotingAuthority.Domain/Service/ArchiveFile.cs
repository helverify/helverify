namespace Helverify.VotingAuthority.Domain.Service;

/// <summary>
/// Represents a file to be zipped.
/// </summary>
public struct ArchiveFile
{
    /// <summary>
    /// File name including extension
    /// </summary>
    public string FileName { get; }

    /// <summary>
    /// Content of the file
    /// </summary>
    public byte[] Data { get; }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="fileName">File name including extension</param>
    /// <param name="data">Content of the file</param>
    public ArchiveFile(string fileName, byte[] data)
    {
        FileName = fileName;
        Data = data;
    }
}