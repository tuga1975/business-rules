namespace SellerCloud.BusinessRules.Attributes
{
    public class BusinessRulePropertyAttribute : BusinessRuleAttribute
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Priority { get; set; }
        public bool IsValue { get; set; }

        public BusinessRulePropertyAttribute(string name = null, int priority = 1, string description = null, bool isValue = false)
        {
            this.Name = name;
            this.Description = description;
            this.Priority = priority;
            this.IsValue = isValue;
        }
    }
}
