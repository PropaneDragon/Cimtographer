using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mapper.Utilities;

namespace MapperTests
{
    [TestClass()]
    public class TaggerTests
    {
        [TestMethod()]
        public void CleanUpNameTests()
        {
            Assert.AreEqual<string>(Tagger.CleanUpName("12345678.Test ObjectName_Data87654321"), "Test Object Name");
            Assert.AreEqual<string>(Tagger.CleanUpName("Test ObjectName 2_Data87654321"), "Test Object Name 2_Data87654321");
            Assert.AreEqual<string>(Tagger.CleanUpName("12345678.Test ObjectName 3"), "12345678.Test Object Name 3");
            Assert.AreEqual<string>(Tagger.CleanUpName("Test ObjectName 4"), "Test Object Name 4");
            Assert.AreEqual<string>(Tagger.CleanUpName("TestObjectName 5"), "Test Object Name 5");
        }
    }
}
