namespace Helverify.VotingAuthority.Domain.Service;

/// <summary>
/// Provides ZIP file creation
/// </summary>
public interface IZipFileService
{
    /// <summary>
    /// Creates a ZIP file in memory.
    /// </summary>
    /// <param name="files">Files to be included in the ZIP</param>
    /// <returns></returns>
    byte[] CreateZip(IList<ArchiveFile> files);
}