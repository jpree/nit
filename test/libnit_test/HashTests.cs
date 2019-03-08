namespace LibNitTest
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Libnit;
    using Xunit;

    /// <summary>
    /// Hash tests.
    /// </summary>
    public class HashTests
    {
        // Sha256 hash for "This is a test string."
        private readonly byte[] target = new byte[]
        {
            0x3E, 0xEC, 0x25, 0x6A, 0x58, 0x7C, 0xCC, 0xF7, 0x2F, 0x71, 0xD2, 0x34, 0x2B, 0x6D, 0xFA, 0xB0,
            0xBB, 0xCA, 0x01, 0x69, 0x7C, 0x7E, 0x70, 0x14, 0x54, 0x0B, 0xDD, 0x62, 0xB7, 0x21, 0x20, 0xDA,
        };

        public HashTests()
        {
            NitPath.OverrideRootFolder(Path.Join(".", $"{nameof(HashTests)}"));

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

        /// <summary>
        /// Test hash binary.
        /// </summary>
        [Fact]
        public void HashBinarySingle()
        {
            var inputBytes = Encoding.UTF8.GetBytes("This is a test string.");
            var hash = Hash.HashObject(inputBytes);
            Assert.True(hash.SequenceEqual(this.target));
        }

        [Fact]
        public void HashFileSingle()
        {
            var hash = Hash.HashFile(Path.Join(".", "resources", "HashFileSingle.txt"));
            Assert.True(hash.SequenceEqual(this.target));
        }
  }
}
