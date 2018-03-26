namespace SellerCloud.BusinessRules.Rules.RuleModule
{
    public class RuleModuleCompanyLink : IEntity
    {
        public int Id { get; set; }
        public int IdRuleModule { get; set; }
        public int IdCompany { get; set; }
    }
}