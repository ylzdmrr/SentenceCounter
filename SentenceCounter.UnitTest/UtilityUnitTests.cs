using Microsoft.VisualStudio.TestTools.UnitTesting;
using SentenceCounter.Core;

namespace SentenceCounter.UnitTest
{
    [TestClass]
    public class UtilityUnitTests
    {
        [TestMethod]
        public void Read_Text_From_File()
        {
            string text = Utility.ReadTextFromFile(@"Documents/SampleText.txt");

            Assert.IsTrue(!string.IsNullOrWhiteSpace(text));
        }
    }
}
