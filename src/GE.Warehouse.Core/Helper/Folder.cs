using System.IO;

namespace GE.Warehouse.Core.Helper
{
    public static class Folder
    {
        public static void CreateFolder(string nameFolder, string dir)
        {
            string folder = dir + "\\" + nameFolder;
            if (Directory.Exists(folder))
            {
                return;
            }
            Directory.CreateDirectory(folder);
        }
    }
}
