using NUnit.Framework;

namespace SellerCloud.BusinessRules.Serializer.Utils.Tests
{
    [TestFixture]
    public class StringHelperTests
    {
        [Test]
        public void StringHelper_PascalCaseSplit_Should_Add_Empty_Space_Before_Every_Upper_Case_Letter()
        {
            var actualString = "IsNotInList";
            var expectedString = "Is Not In List";

            var resultString = actualString.CaseSplit();

            Assert.AreEqual(expectedString, resultString);

            actualString = "IsNotInTheHTML";
            expectedString = "Is Not In The HTML";

            resultString = actualString.CaseSplit();

            Assert.AreEqual(expectedString, resultString);

            actualString = "Agent007IsOutThere";
            expectedString = "Agent 007 Is Out There";

            resultString = actualString.CaseSplit();

            Assert.AreEqual(expectedString, resultString);
        }
    }
}
