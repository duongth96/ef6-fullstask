using System.Collections.Generic;

namespace GE.Warehouse.Services.Configuration
{
    public interface IConfigurationService
    {
        /// <summary>
        /// Get settings
        /// </summary>
        /// <param name="nodeName"></param>
        /// <returns></returns>
        Dictionary<string, string> GetSettings(string nodeName);

        /// <summary>
        /// Get custom settings
        /// </summary>
        /// <param name="nodeName"></param>
        /// <returns></returns>
        List<Dictionary<string, string>> GetGroupSettings(string nodeName);
    }
}
