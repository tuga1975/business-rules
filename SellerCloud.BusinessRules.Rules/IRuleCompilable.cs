namespace SellerCloud.BusinessRules.Rules
{
    public interface IRuleCompilable : IEntity
    {
        RuleCompilableType Type { get; }
    }
}