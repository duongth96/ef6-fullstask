using System;

namespace GE.Warehouse.Core.Data
{
    public partial class DataSettingsHelper
    {
        private static bool? _databaseIsInstalled;
        public static bool DatabaseIsInstalled()
        {
            if (!_databaseIsInstalled.HasValue)
            {
                var manager = new DataSettingsManager();
                var settings = manager.LoadSettings();
                _databaseIsInstalled = settings != null && !String.IsNullOrEmpty(settings.DataConnectionString);
            }
            return _databaseIsInstalled.Value;

            //return true;
        }

        public static void ResetCache()
        {
            _databaseIsInstalled = null;
        }
    }
}
