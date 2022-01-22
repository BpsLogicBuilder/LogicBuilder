namespace ABIS.LogicBuilder.FlowBuilder.Services
{
    internal interface IPathHelper
    {
        string CombinePaths(string path1, string path2);
        string CombinePaths(string path1, string path2, string path3);
        string CombinePaths(params string[] partialPaths);
        string GetExtension(string fileName);
        string GetFileName(string fileName);
        string GetFileNameNoExtention(string fileName);
        string GetFilePath(string fileName);
        string GetFolderName(string folderPath);
        string GetModuleName(string fileName);
    }
}
