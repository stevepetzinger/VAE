using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VAE_Bigram.UnitTests
{
    /*
     *                case-insensitive
                basic functionality, 1,2,3 counts
                punctuation removed
                assert the number of distinct pairs
                assert the total number of pairs
                multi-line
                */
    [TestFixture]
    public class BigramTests
    {
        [Test]
        public void CleanupString_ContainsUpperCase_ConvertToLowerCase()
        {
            var input = "AaBb Cc";
            var expected = "aabb cc";
            var actual = VAE_Bigram.Program.CleanUpString(input);

            Assert.AreEqual(actual, expected);
        }
        [Test]
        public void CleanupString_ContainsLineBreaks_ConvertToSpaces()
        {
            var input = $"hello{Environment.NewLine}there";
            var expected = "hello there";
            var actual = Program.CleanUpString(input);
            Assert.AreEqual(actual, expected);
        }
        [Test]
        public void CleanupString_ContainsPunctuation_RemovePunctuation()
        {
            var input = "this is a test! passed?";
            var expected = "this is a test passed";
            var actual = Program.CleanUpString(input);
            Assert.AreEqual(actual, expected);
        }
        [Test]
        public void ParseString_CountWords()
        {
            var input = "one small step for man";
            var expected = 5; // word count
            var actual = Program.ParseString(input).Count();
            Assert.AreEqual(actual, expected);
        }
        [Test]
        public void PerformCounts_CountDistinctPairs()
        {
            var input = new List<string> { "the", "quick", "brown", "fox", "and", "the", "quick", "blue", "hare" };
            var expected = 7;

            var actual = Program.PerformCounts(input).Count();
            Assert.AreEqual(actual, expected);
        }
    }
}
