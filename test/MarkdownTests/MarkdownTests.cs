namespace MarkdownTests
{
    using System;
    using System.IO;
    using Libnit;
    using Nit.Import;
    using Xunit;
    using System.Linq;

    public class MarkdownTests
    {
        public MarkdownTests()
        {
            NitPath.OverrideRootFolder(Path.Join(".", nameof(MarkdownTests)));

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
        public void SingleItemImportTest()
        {
            var testFile = Path.Combine(".", "Resources", "SingleItem.md");
            Markdown.Import(testFile);

            // see if we created the tags and hash
            var kw = Lookup.GetKeywordDictionary(new string[] { "My", "Markdown", "Document" });

            // should be one entry
            Assert.Single(kw.Keys);

            // should be 3 tag hits
            Assert.Equal<uint>(3, kw.First().Value);
        }

        [Fact]
        public void EmptyItemImportTest()
        {
            var testFile = Path.Combine(".", "Resources", "EmptyItem.markdown");
            Markdown.Import(testFile);

            // see if we created the tags and hash
            var kw = Lookup.GetKeywordDictionary(new string[] { "My", "Markdown", "Document" });

            // should be one entry
            Assert.Empty(kw.Keys);
        }

        [Fact]
        public void FileNotFoundExceptionImportTest()
        {
            Exception ex = Assert.Throws<FileNotFoundException>(() =>
            {
                var testFile = Path.Combine(".", "Resources", "Notexist.markdown");
                Markdown.Import(testFile);
            });
        }

        [Fact]
        public void FileNotMarkdownImportTest()
        {
            Exception ex = Assert.Throws<ArgumentException>(() =>
            {
                var testFile = Path.Combine(".", "Resources", "Empty.notmarkdown");
                Markdown.Import(testFile);
            });
        }

        [Fact]
        public void EmptyFileImportTest()
        {
            var testFile = Path.Combine(".", "Resources", "Empty.markdown");
            Markdown.Import(testFile);

            // see if we created the tags and hash
            var kw = Lookup.GetKeywordDictionary(new string[] { "My", "Markdown", "Document" });

            // should be one entry
            Assert.Empty(kw.Keys);
        }

        [Fact]
        public void MultipleItemImportTest()
        {
            var testFile = Path.Combine(".", "Resources", "MultipleItem.md");
            Markdown.Import(testFile);

            // see if we created the tags and hash
            var kw = Lookup.GetKeywordDictionary(new string[] { "Heading1" });

            Assert.Equal(5, kw.Count);
            Assert.Equal<uint>(1, kw.First().Value);

            kw = Lookup.GetKeywordDictionary(new string[] { "Heading3" });

            Assert.Single(kw);
            Assert.Equal<uint>(1, kw.First().Value);

            kw = Lookup.GetKeywordDictionary(new string[] { "Heading2.1" });

            Assert.Equal(2, kw.Count);
            Assert.Equal<uint>(1, kw.First().Value);
        }

        [Fact]
        public void MultipleItemTroubleImportTest()
        {
            var testFile = Path.Combine(".", "Resources", "MultipleItemTrouble.md");
            Markdown.Import(testFile);

            // see if we created the tags and hash
            var kw = Lookup.GetKeywordDictionary(new string[] { "Headingz2" });

            Assert.Single(kw);
            Assert.Equal<uint>(1, kw.First().Value);

            kw = Lookup.GetKeywordDictionary(new string[] { "Headingz1" });

            Assert.Equal(2, kw.Count);
            Assert.Equal<uint>(1, kw.First().Value);

            kw = Lookup.GetKeywordDictionary(new string[] { "Headingz3" });

            Assert.Single(kw);
            Assert.Equal<uint>(1, kw.First().Value);
        }
    }
}
