namespace LibNitTest
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Libnit;
    using Xunit;

    public class TagTests
    {
        // Sha256 hash for "This is a test string."
        private readonly byte[] target = new byte[]
        {
            0x3E, 0xEC, 0x25, 0x6A, 0x58, 0x7C, 0xCC, 0xF7, 0x2F, 0x71, 0xD2, 0x34, 0x2B, 0x6D, 0xFA, 0xB0,
            0xBB, 0xCA, 0x01, 0x69, 0x7C, 0x7E, 0x70, 0x14, 0x54, 0x0B, 0xDD, 0x62, 0xB7, 0x21, 0x20, 0xDA,
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="TagTests"/> class.
        /// </summary>
        public TagTests()
        {
            NitPath.OverrideRootFolder(Path.Join(".", $"{nameof(TagTests)}"));

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
        public void CreateTagIndex()
        {
            var expectedFilePath = Path.Combine(".", $"{nameof(TagTests)}", "tag", "532E", "AABD9574880DBF76B9B8CC00832C20A6EC113D682299550D7A6E0F345E25");
            var tag = "Test";
            var tags = new List<string>();
            tags.Add(tag);

            Tag.CreateTags(this.target, tags.ToArray());
            var result = File.Exists(expectedFilePath);
            Assert.True(result);

            var output = File.ReadAllText(expectedFilePath);
            Assert.Equal("3EEC256A587CCCF72F71D2342B6DFAB0BBCA01697C7E7014540BDD62B72120DA" + Environment.NewLine, output);
        }
    }
}
