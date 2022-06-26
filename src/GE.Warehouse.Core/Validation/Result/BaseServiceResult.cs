using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace GE.Warehouse.Core.Validation.Result
{
    public abstract class BaseServiceResult
    {
        [DebuggerStepThrough]
        protected BaseServiceResult(IEnumerable<RuleViolation> ruleViolations)
        {
            RuleViolations = new List<RuleViolation>(ruleViolations);
        }

        public IList<RuleViolation> RuleViolations
        {
            get;
            private set;
        }

        public string Error()
        {
            return RuleViolations.Aggregate("", (current, ruleViolation) => current + (ruleViolation.ErrorMessage + "\n"));
        }
    }
}
