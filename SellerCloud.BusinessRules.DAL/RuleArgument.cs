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
    
    public partial class RuleArgument
    {
        public int Id { get; set; }
        public int IdRule { get; set; }
        public string ValueXml { get; set; }
        public string ValueType { get; set; }
        public int Position { get; set; }
        public string ValueKey { get; set; }
        public string Description { get; set; }
    
        public virtual Rule Rule { get; set; }
    }
}
