using System;
using System.Collections.Generic;

namespace SellerCloud.BusinessRules.EntityTracking
{
    public interface IEntityChangeInformation
    {
        int IdRule { get; set; }
        string Member { get; set; }
        string Operation { get; set; }
        IEnumerable<object> Values { get; set; }
        IEntityChangeInformation Filter { get; set; }
        IEnumerable<int> EvaluationPath { get; set; }
    }
}