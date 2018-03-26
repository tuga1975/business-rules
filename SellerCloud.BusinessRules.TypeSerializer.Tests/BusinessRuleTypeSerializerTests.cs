using NUnit.Framework;
using SellerCloud.BusinessRules.Logging;
using SellerCloud.BusinessRules.Models;
using System;

namespace SellerCloud.BusinessRules.TypeSerializer.Tests
{
    [TestFixture]
    public class BusinessRuleTypeSerializerTests
    {
        private void LogJson<T>(string json)
        {
            var logger = new FileJsonLogger();
            var type = typeof(T);
            
            var logMessage = $"{ type.FullName }{ Environment.NewLine }{ json }";
            logger.Log(logMessage);
        }

        [Test]
        public void Type_Should_Be_Serialized_Without_Errors()
        {
            var serializer = new BusinessRuleTypeJsonSerializer();
            var json = serializer.Serialize<Order>();

            LogJson<Order>(json);

            Assert.IsFalse(string.IsNullOrEmpty(json));

            Assert.IsTrue(json.Contains("\"ShippingStatus\": {"));
            Assert.IsTrue(json.Contains("\"DisplayName\": \"Shipping Status\""));
            Assert.IsTrue(json.Contains("\"DisplayName\": \"Shipping Address\""));
            Assert.IsTrue(json.Contains("\"OtherInfo\": {"));
            Assert.IsTrue(json.Contains("\"DisplayName\": \"Tenantry\""));
            Assert.IsTrue(json.Contains("\"Type\": \"Collection\""));
            Assert.IsTrue(json.Contains("\"Type\": \"SellerCloud.BusinessRules.Models.Person, SellerCloud.BusinessRules.Models, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\""));
            Assert.IsTrue(json.Contains("\"EnumQualifiedName\": \"SellerCloud.BusinessRules.Models.Gender, SellerCloud.BusinessRules.Models, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\""));
        }
    }
}
