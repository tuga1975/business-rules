using NUnit.Framework;
using SellerCloud.BusinessRules.Logging;

namespace SellerCloud.BusinessRules.OperationsSerializer.Tests
{
    [Parallelizable(ParallelScope.All)]
    [TestFixture]
    public class BusinessRuleOperationsSerializerTests
    {
        private void LogJson(string json)
        {
            var logger = new FileJsonLogger();
            logger.Log(json);
        }

        [Test]
        public void Operations_Should_Be_Serialized_Without_Errors()
        {
            var serializer = new BusinessRuleOperationsJsonSerializer();

            var json = serializer.Serialize();

            LogJson(json);

            Assert.IsFalse(string.IsNullOrEmpty(json));
        }
    }
}
