using System.Diagnostics;

namespace GE.Warehouse.Core.Validation
{
    public class RuleViolation
    {
        [DebuggerStepThrough]
        public RuleViolation(string parameterName, string errorMessage)
        {                        
            ParameterName = parameterName;
            ErrorMessage = errorMessage;
        }

        public string ParameterName
        {
            get;
            private set;
        }

        public string ErrorMessage
        {
            get;
            private set;
        }
    }
}
