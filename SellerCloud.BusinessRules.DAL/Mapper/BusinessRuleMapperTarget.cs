using System;

namespace SellerCloud.BusinessRules.DAL.Mapper
{
    public class BusinessRuleMapperTarget
    {
        public Type Target { get; set; }
        public Type Source { get; set; }
        public Type Dest { get; set; }

        public BusinessRuleMapperTarget()
        {
        }

        public BusinessRuleMapperTarget(Type target, Type source, Type dest)
        {
            Target = target;
            Source = source;
            Dest = dest;
        }
    }

}
