using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace SellerCloud.BusinessRules.Rules.RuleModule
{
    public class RuleModule : IRuleModule, ICloneable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? CompanyID { get; set; }
        public string ContextType { get; set; }
        public int? ClientID { get; set; }
        public bool EnabledFlag { get; set; }

        public int? IdParentRuleModule { get; set; }
        [JsonIgnore]
        public virtual RuleModule ParentRuleModule { get; set; }
        [JsonIgnore]
        public virtual ICollection<RuleModule> RuleModuleParentLink { get; set; }

        public int? IdRootRuleModule { get; set; }
        [JsonIgnore]
        public virtual RuleModule RootRuleModule { get; set; }
        [JsonIgnore]
        public virtual ICollection<RuleModule> RuleModuleRootLink { get; set; }

        public virtual ICollection<RuleModuleCompanyLink> RuleModuleCompanyLinks { get; set; }

        public IEnumerable<int> IdCompanies
        {
            set
            {
                var ruleModuleCompanyLinks = RuleModuleCompanyLinks ?? new List<RuleModuleCompanyLink>();
                RuleModuleCompanyLinks = value.Select(id =>
                {
                    var companyLink = ruleModuleCompanyLinks.FirstOrDefault(cl => cl.IdRuleModule == Id && cl.IdCompany == id);
                    return companyLink ?? new RuleModuleCompanyLink
                    {
                        IdCompany = id,
                        IdRuleModule = Id
                    };
                }).ToArray();
            }
        }

        public RuleModulePath? PathType { get; set; }
        
        public virtual RuleModule IfTrue { get; set; }
        
        public virtual RuleModule IfFalse { get; set; }

        public virtual ICollection<IRuleCompilable> Rules { get; set; }

        public virtual RuleType RuleModuleType { get; set; }

        protected virtual bool IsValidRuleType(IRuleCompilable rule) => false;

        public RuleCompilableType Type => RuleCompilableType.Module;

        public BooleanRuleModuleEvalLogic? BooleanEvalLogic { get; set; }

        public RuleModule()
        {
            
        }

        public RuleModule(RuleType ruleModuleType, params IRuleCompilable[] rules) : this()
        {
            RuleModuleType = ruleModuleType;
            Rules = rules;
        }

        public object Clone()
        {
            var ruleModule = (RuleModule)MemberwiseClone();
            return ruleModule;
        }

        public object Select()
        {
            throw new NotImplementedException();
        }
    }
}