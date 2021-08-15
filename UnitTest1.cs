using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace TestProject01
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            Assert.AreEqual(songDecoder("AWUBBWUBC"), "A B C", "WUB should be replaced by 1 space");
        }

        [TestMethod]
        public void TestMethod2()
        {
            Assert.AreEqual(songDecoder("AWUBWUBWUBBWUBWUBWUBC"), "A B C", "multiples WUB should be replaced by only 1 space");
        }

        [TestMethod]
        public void TestMethod3()
        {
            Assert.AreEqual(songDecoder("WUBAWUBBWUBCWUB"), "A B C", "heading or trailing spaces should be removed");
        }

        private string songDecoder(string song)
        {
            song = song.Replace("WUB", " ");
            var wordList = song.Split(" ").Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();//remove all space
            return string.Join(" ", wordList);//add 1 space
        }
    }
}