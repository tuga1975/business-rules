using SellerCloud.BusinessRules.Attributes;

namespace SellerCloud.BusinessRules.Models
{
    public class Person
    {
        [BusinessRuleProperty(name: "First Name")]
        public string FirstName { get; set; }

        [BusinessRuleProperty(name: "Middle Name")]
        public string MiddleName { get; set; }

        [BusinessRuleProperty(name: "Last Name")]
        public string LastName { get; set; }

        [BusinessRuleProperty]
        public int Age { get; set; }

        [BusinessRuleProperty]
        public Gender Gender { get; set; }

        public string FullName => $"{ FirstName } { MiddleName } { LastName }";
    }
}
