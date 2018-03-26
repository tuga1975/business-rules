using System.Collections.Generic;
using System.Linq;

namespace SellerCloud.BusinessRules.Rules
{
    public abstract class RuleBase : IRule
    {
        public int Id { get; set; }

        public int IdRuleModule { get; set; }

        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public virtual ICollection<RuleArgument> Arguments { get; set; }
        public virtual ICollection<RuleArgument> SaveArguments { get; set; }

        public string Expression { get; set; } = "";
        public RuleType RuleType { get; set; }
        public RuleCompilableType Type => RuleCompilableType.Rule;

        protected RuleBase()
        {
            Arguments = new List<RuleArgument>();
        }

        public object Clone()
        {
            var rule = this.MemberwiseClone() as IRule;
            rule.Arguments = this.Arguments.Select(arg => (RuleArgument)arg.Clone()).ToArray();
            return rule;
        }

        public T ConvertSaveArguments<T>() where T : RuleBase
        {
            if (SaveArguments != null)
            {
                Arguments = RuleArgumentConvertor.ConvertArguments(SaveArguments);
                SaveArguments = null;
            }

            return (T)this;
        }
    }
}
