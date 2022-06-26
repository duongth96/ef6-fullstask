namespace GE.Warehouse.Core.IO
{
    public interface IFile
    {
        string ReadAllText(string path);

        string[] ReadAllLine(string path);

        void WriteAllText(string path, string content);

        byte[] ReadAllBytes(string path);
    }
}