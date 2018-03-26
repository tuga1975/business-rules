using System.Collections.Generic;

namespace SellerCloud.BusinessRules.DAL.Mapper
{
    public class BusinessRuleMapperConfiguration
    {
        public HashSet<BusinessRuleMapperTarget> Targets { get; set; }

        public BusinessRuleMapperConfiguration(params BusinessRuleMapperTarget[] mapperTargets)
        {
            this.Targets = new HashSet<BusinessRuleMapperTarget>(mapperTargets);
        }
    }

}
