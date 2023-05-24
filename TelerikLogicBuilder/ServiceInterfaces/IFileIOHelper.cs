using System.IO;
using System.Resources;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces
{
    internal interface IFileIOHelper
    {
        void CopyFile(FileInfo source, FileInfo destination);
        void CopyFile(string sourceFullName, string destinationFullName);
        void CreateDirectory(string fullName);
        void DeleteFile(string fullName);
        void DeleteFolder(string fullName, bool recursive);
        byte[] GetFileBytes(string fullName);
        DirectoryInfo GetNewDirectoryInfo(string fullName);
        FileInfo GetNewFileInfo(string fullName);
        IResourceReader GetResourceReader(string fullName);
        void MoveFile(string source, string destination);
        void MoveFolder(string source, string destination);
        string ReadFromFile(string fileFullName);
        void SaveFile(string fileFullName, string text);
        void SaveNewFile(string fileFullName, string text);
        void SetWritable(string fullName, bool writable);
    }
}
