using System.Text;
using Helverify.VotingAuthority.Domain.Service;

namespace Helverify.VotingAuthority.Domain.Tests.Service
{
    internal class ZipFileServiceTests
    {
        [Test]
        public void TestCreateZip()
        {
            // arrange
            IList<ArchiveFile> files = new List<ArchiveFile>
            {
                new ("test1.txt", Encoding.UTF8.GetBytes("test1")),
                new ("test2.txt", Encoding.UTF8.GetBytes("test2")),
            };

            IZipFileService zipper = new ZipFileService();

            // act
            byte[] zipFile = zipper.CreateZip(files);

            // assert
            Assert.That(zipFile.Length, Is.GreaterThan(0));
        }
    }
}
