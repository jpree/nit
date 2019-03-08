namespace LibNitTest
{
    using System.IO;
    using Libnit;
    using Xunit;

    /// <summary>
    /// NitPathTests.
    /// </summary>
    public class NitPathTests
    {
        public NitPathTests()
        {
            NitPath.OverrideRootFolder(Path.Join(".", $"{nameof(NitPathTests)}"));

            try
            {
                // make sure test area is clean
                Directory.Delete(NitPath.RootFolder, true);
            }
            catch
            {
                // folder probably didn't exist
            }
        }

        [Fact]
        public void GetBlobRootTest()
        {
            var result = NitPath.GetBlobRoot("obj");
            Assert.Equal(Path.Join(".", $"{nameof(NitPathTests)}", "obj"), result);
        }

        [Fact]
        public void GetDirTest()
        {
            var result = NitPath.GetDir(new byte[] { 0x01, 0x02, 0x03, 0x04 });
            Assert.Equal("0102", result);
        }

        [Fact]
        public void GetFileNameTest()
        {
            var result = NitPath.GetFileName(new byte[] { 0x01, 0x02, 0x03, 0x04 });
            Assert.Equal("0304", result);
        }

        [Fact]
        public void GetFullPathTest()
        {
            var result = NitPath.GetFullObjectPath(new byte[] { 0x01, 0x02, 0x03, 0x04 });
            var x = Path.Combine(".", $"{nameof(NitPathTests)}", "obj", "0102", "0304");
            Assert.Equal(Path.Combine(".", $"{nameof(NitPathTests)}", "obj", "0102", "0304"), result);
        }

        [Fact]
        public void GetDirectoryPathTest()
        {
            var result = NitPath.GetObjectDirectoryPath(new byte[] { 0x01, 0x02, 0x03, 0x04 });
            Assert.Equal(Path.Combine(".", $"{nameof(NitPathTests)}", "obj", "0102"), result);
        }
    }
}
