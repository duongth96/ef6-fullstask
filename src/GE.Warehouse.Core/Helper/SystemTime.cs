using System;

namespace GE.Warehouse.Core.Helper
{
    public static class SystemTime
    {
        public static Func<DateTime> Now = () => DateTime.UtcNow;
    }
}