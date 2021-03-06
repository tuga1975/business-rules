//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SellerCloud.BusinessRules.DAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class RuleModule
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public RuleModule()
        {
            this.Rules = new HashSet<Rule>();
            this.RuleModuleParentLink = new HashSet<RuleModule>();
            this.RuleModuleRootLink = new HashSet<RuleModule>();
            this.RuleModuleCompanyLinks = new HashSet<RuleModuleCompanyLink>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public Nullable<int> IdParentRuleModule { get; set; }
        public Nullable<int> IdRootRuleModule { get; set; }
        public Nullable<int> PathType { get; set; }
        public int RuleModuleType { get; set; }
        public Nullable<int> BooleanEvalLogic { get; set; }
        public string ContextType { get; set; }
        public Nullable<int> ClientID { get; set; }
        public Nullable<int> IfTrueId { get; set; }
        public Nullable<int> IfFalseId { get; set; }
        public Nullable<bool> EnabledFlag { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Rule> Rules { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RuleModule> RuleModuleParentLink { get; set; }
        public virtual RuleModule ParentRuleModule { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RuleModule> RuleModuleRootLink { get; set; }
        public virtual RuleModule RootRuleModule { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RuleModuleCompanyLink> RuleModuleCompanyLinks { get; set; }
    }
}
