using GE.Warehouse.Core.Configuration;

namespace GE.Warehouse.Core.IO
{
    public class FileSystemSettings : ISettings
    {
        public string DirectoryName { get; set; }
    }
}