namespace SellerCloud.BusinessRules.Attributes
{
    public class BusinessRuleItemsEndpointAttribute : BusinessRuleAttribute
    {
        public string Endpoint { get; set; }

        public BusinessRuleItemsEndpointAttribute(string endpoint)
        {
            if (string.IsNullOrWhiteSpace(endpoint))
            {
                throw new System.ArgumentException("Parameter cannot be null or empty", nameof(endpoint));
            }

            Endpoint = endpoint;
        }
    }
}
