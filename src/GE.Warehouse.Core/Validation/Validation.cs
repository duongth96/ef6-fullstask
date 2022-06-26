using System;
using GE.Warehouse.Core.Validation.Result;

namespace GE.Warehouse.Core.Validation
{
    public static class Validation
    {
        public static Validation<TServiceResult> Validate<TServiceResult>(Func<bool> condition, string parameterName, string errorMessage) where TServiceResult : BaseServiceResult
        {
            return new Validation<TServiceResult>(condition, parameterName, errorMessage);
        }
    }
}