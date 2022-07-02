using System.IO.Compression;

namespace Helverify.VotingAuthority.Domain.Service
{
    internal class ZipFileService : IZipFileService
    {
        public byte[] CreateZip(IList<ArchiveFile> files)
        {
            // inspired by https://stackoverflow.com/questions/51740673/building-a-corrupted-zip-file-using-asp-net-core-and-angular-6
            using (MemoryStream zipStream = new MemoryStream())
            {
                using (ZipArchive zipFile = new ZipArchive(zipStream, ZipArchiveMode.Create, true))
                {
                    foreach (ArchiveFile file in files)
                    {
                        ZipArchiveEntry archiveEntry = zipFile.CreateEntry(file.FileName);

                        using (Stream fileStream = new MemoryStream(file.Data))
                        using (Stream entryStream = archiveEntry.Open())
                        {
                            fileStream.CopyTo(entryStream);
                        }
                    }
                }

                return zipStream.ToArray();
            }
        }
    }
}
