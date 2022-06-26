using System.Collections.Generic;

namespace GE.Warehouse.Core.Validation.Result
{
    public class ServiceResult<T> : BaseServiceResult
    {
        public ServiceResult():this(new List<RuleViolation>())
        {            
        }

        public ServiceResult(T result):this()
        {
            Result = result;
        }

        public ServiceResult(IEnumerable<RuleViolation> ruleViolations)
            : base(ruleViolations)
        {
        }        

        public T Result
        {
            get; set;
        }
    }    
}
