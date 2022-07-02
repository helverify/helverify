namespace Helverify.VotingAuthority.Domain.Service;

public interface IZipFileService
{
    byte[] CreateZip(IList<ArchiveFile> files);
}