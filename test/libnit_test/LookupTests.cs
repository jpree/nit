namespace LibNitTest
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Libnit;
    using Xunit;

    /// <summary>
    /// Lookup tests.
    /// </summary>
    public class LookupTests
    {
        // Sha256 hash for "This is a test string."
        private readonly byte[] target1 = new byte[]
        {
            0x3E, 0xEC, 0x25, 0x6A, 0x58, 0x7C, 0xCC, 0xF7, 0x2F, 0x71, 0xD2, 0x34, 0x2B, 0x6D, 0xFA, 0xB0,
            0xBB, 0xCA, 0x01, 0x69, 0x7C, 0x7E, 0x70, 0x14, 0x54, 0x0B, 0xDD, 0x62, 0xB7, 0x21, 0x20, 0xDA,
        };

        private readonly byte[] target2 = new byte[]
        {
            0x3B, 0xEC, 0x25, 0x6A, 0x58, 0x7C, 0xCC, 0xF7, 0x2F, 0x71, 0xD2, 0x34, 0x2B, 0x6D, 0xFA, 0xB0,
            0xBB, 0xCA, 0x01, 0x69, 0x7C, 0x7E, 0x70, 0x14, 0x54, 0x0B, 0xDD, 0x62, 0xB7, 0x21, 0x20, 0xDA,
        };

        private readonly byte[] target3 = new byte[]
        {
            0x3C, 0xEC, 0x25, 0x6A, 0x58, 0x7C, 0xCC, 0xF7, 0x2F, 0x71, 0xD2, 0x34, 0x2B, 0x6D, 0xFA, 0xB0,
            0xBB, 0xCA, 0x01, 0x69, 0x7C, 0x7E, 0x70, 0x14, 0x54, 0x0B, 0xDD, 0x62, 0xB7, 0x21, 0x20, 0xDA,
        };

        public LookupTests()
        {
            NitPath.OverrideRootFolder(Path.Join(".", $"{nameof(LookupTests)}"));

            try
            {
                // make sure test area is clean
                Directory.Delete(NitPath.RootFolder, true);
            }
            catch
            {
                // folder probably didn't exist
            }

            // create test tags in bulk
            var testSet = File.ReadAllText(Path.Join(".", "Resources", "Poem.txt"));
            var tagSet = testSet.Split(' ');
            Tag.CreateTags(this.target2, tagSet);
            Tag.CreateTags(this.target3, tagSet);

            var tags = new List<string>();
            tags.Add("Scoobie");
            tags.Add("Doo");
            tags.Add("And");
            tags.Add("Shaggy");
            Tag.CreateTags(this.target1, tags.ToArray());
        }

        [Fact]
        public void TestLookupTwoTags()
        {
            var tags = new List<string>();
            tags.Add("Scoobie");
            tags.Add("Doo");

            // use query to get suggested blobs
            var dict = Lookup.GetKeywordDictionary(tags.ToArray());

            // one entry
            Assert.Single(dict);

            // should be two hits
            Assert.Equal<uint>(2, dict[this.target1]);
        }

        [Fact]
        public void TestLookupNullTags()
        {
            Exception ex = Assert.Throws<ArgumentNullException>(() =>
            {
                Lookup.GetKeywordDictionary(null);
            });
        }

        [Fact]
        public void TestLookupEmptyTags()
        {
            var tags = new List<string>();
            Exception ex = Assert.Throws<ArgumentException>(() =>
            {
                Lookup.GetKeywordDictionary(tags.ToArray());
            });
        }

        [Fact]
        public void TestLookupFourTags()
        {
            var tags = new List<string>();
            tags.Add("Scoobie");
            tags.Add("Doo");
            tags.Add("And");
            tags.Add("Shaggy");

            // use query to get suggested blobs
            var dict = Lookup.GetKeywordDictionary(tags.ToArray());

            // one entry
            Assert.Single(dict);

            // should be two hits
            Assert.Equal<uint>(4, dict[this.target1]);
        }

        [Fact]
        public void TestLookupTwoResults()
        {
            var tags = new List<string>();
            tags.Add("Laugh");
            tags.Add("pain");

            // use query to get suggested blobs
            var dict = Lookup.GetKeywordDictionary(tags.ToArray());

            // one entry
            Assert.Equal(2, dict.Count);

            // should be two hits
            Assert.False(dict.ContainsKey(this.target1));
            Assert.True(dict.ContainsKey(this.target3));
            Assert.True(dict.ContainsKey(this.target2));
            Assert.Equal<uint>(2, dict[this.target2]);
            Assert.Equal<uint>(2, dict[this.target3]);
        }
    }
}
