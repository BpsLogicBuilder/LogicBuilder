namespace ABIS.LogicBuilder.FlowBuilder.Services
{
    internal interface IPathHelper
    {
        string CombinePaths(string path1, string path2);
        string CombinePaths(string path1, string path2, string path3);
        string CombinePaths(params string[] partialPaths);
        string Extension(string fileName);
        string FileName(string fileName);
        string FileNameNoExtention(string fileName);
        string FilePath(string fileName);
        string FolderName(string folderPath);
        string ModuleName(string fileName);
    }
}
