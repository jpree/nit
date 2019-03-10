namespace MarkdownTests
{
    using Nit.Import;
    using Xunit;

    public class OutlineTests
    {
        [Fact]
        public void TestDepth()
        {
            var outline = new Outline();
            Assert.Equal(2, outline.Depth("###Test"));
            Assert.Equal(1, outline.Depth(" ##Test"));
            Assert.Equal(0, outline.Depth("  # Test"));
        }

        [Fact]
        public void TesIsHeader()
        {
            var outline = new Outline();
            Assert.True(outline.IsHeader("###Test"));
            Assert.True(outline.IsHeader(" ##Test"));
            Assert.True(outline.IsHeader("  # Test"));
            Assert.False(outline.IsHeader("   Test#"));
        }

        [Fact]
        public void TestIsNormalize()
        {
            var outline = new Outline();
            Assert.Equal("Test", outline.Normalize("###Test"));
            Assert.Equal("Test text", outline.Normalize("   #Test text"));
            Assert.Equal("Test#", outline.Normalize("Test#"));
            Assert.Equal("Test", outline.Normalize("### Test"));
        }

        [Fact]
        public void TestIsOneHeaderOneTag()
        {
            var outline = new Outline();
            outline.SetHeader("# One ");
            var result = outline.GetTags();
            Assert.Single(result.ToArray());
        }

        [Fact]
        public void TestIsOneHeaderTwoTag()
        {
            var outline = new Outline();
            outline.SetHeader("# One Two");
            var result = outline.GetTags();
            Assert.Equal(2, result.Length);

            Assert.Equal("One", result[0]);
            Assert.Equal("Two", result[1]);
        }

        [Fact]
        public void TestIsOneHeaderTwoTagReplace()
        {
            var outline = new Outline();
            outline.SetHeader("# One Two");
            outline.SetHeader("# OneA TwoA");
            var result = outline.GetTags();

            Assert.Equal(2, result.Length);
            Assert.Equal("OneA", result[0]);
            Assert.Equal("TwoA", result[1]);
        }

        [Fact]
        public void TestIsTwoHeaderTwo()
        {
            var outline = new Outline();
            outline.SetHeader("# One Two");
            outline.SetHeader("## OneB TwoB");
            var result = outline.GetTags();

            Assert.Equal(4, result.Length);
            Assert.Equal("One", result[0]);
            Assert.Equal("Two", result[1]);
            Assert.Equal("OneB", result[2]);
            Assert.Equal("TwoB", result[3]);
        }

        [Fact]
        public void TestIsTwoHeaderTwoReplace()
        {
            var outline = new Outline();
            outline.SetHeader("# One Two");
            outline.SetHeader("## OneB TwoB");
            outline.SetHeader("# OneC TwoC");
            var result = outline.GetTags();

            Assert.Equal(2, result.Length);
            Assert.Equal("OneC", result[0]);
            Assert.Equal("TwoC", result[1]);
        }

        [Fact]
        public void TestIsLevelThreeOnly()
        {
            var outline = new Outline();
            outline.SetHeader(" ### One Two");

            var result = outline.GetTags();

            Assert.Equal(2, result.Length);
            Assert.Equal("One", result[0]);
            Assert.Equal("Two", result[1]);
        }

        [Fact]
        public void TestIsLevelThreeBackToOne()
        {
            var outline = new Outline();
            outline.SetHeader(" ### One Two");
            outline.SetHeader(" # OneA TwoB");

            var result = outline.GetTags();

            Assert.Equal(2, result.Length);
            Assert.Equal("OneA", result[0]);
            Assert.Equal("TwoB", result[1]);
        }
    }
}
