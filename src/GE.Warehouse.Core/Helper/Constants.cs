using System;
using System.Globalization;

namespace GE.Warehouse.Core.Helper
{
    public static class Constants
    {
        public static readonly DateTime ProductionDate = new DateTime(2008, 1, 11);

        public static CultureInfo CurrentCulture
        {
            get
            {
                return CultureInfo.CurrentCulture;
            }
        }
    }
}