using ABIS.LogicBuilder.FlowBuilder.Structures;
using System.Globalization;
using System.IO;

namespace ABIS.LogicBuilder.FlowBuilder.Services
{
    internal class PathHelper : IPathHelper
    {
        private readonly IExceptionHelper _exceptionHelper;

        public PathHelper(IExceptionHelper exceptionHelper)
        {
            _exceptionHelper = exceptionHelper;
        }

        public string CombinePaths(string path1, string path2)
        {
            if (string.IsNullOrEmpty(path1) || string.IsNullOrEmpty(path2))
                throw _exceptionHelper.CriticalException("{4342018D-6C1A-4BE7-BAEE-7ABFB9021948}");

            return Path.Combine(path1, path2);
        }

        public string CombinePaths(string path1, string path2, string path3)
        {
            if (string.IsNullOrEmpty(path1) || string.IsNullOrEmpty(path2) || string.IsNullOrEmpty(path3))
                throw _exceptionHelper.CriticalException("{D804D2BE-0D03-4FC3-A2C8-BC0E1A412483}");

            return Path.Combine(path1, path2, path3);
        }

        public string CombinePaths(params string[] partialPaths)
        {
            if (partialPaths == null)
                return string.Empty;

            if (partialPaths.Length == 0)
                return string.Empty;

            return Path.Combine(partialPaths);
        }

        public string Extension(string fileName) => fileName.Substring(fileName.LastIndexOf("."));

        public string FileName(string fileName) => Path.GetFileName(fileName);

        public string FileNameNoExtention(string fileName) => Path.GetFileNameWithoutExtension(fileName);

        public string FilePath(string fileName) => Path.GetDirectoryName(fileName);

        public string FolderName(string folderPath)
        {
            string fPath = folderPath.EndsWith(FileConstants.DIRECTORYSEPARATOR) ? folderPath.Substring(0, folderPath.LastIndexOf(FileConstants.DIRECTORYSEPARATOR)) : folderPath;
            return fPath.Substring(fPath.LastIndexOf(FileConstants.DIRECTORYSEPARATOR) + 1);
        }

        public string ModuleName(string fileName)
        {
            if (!fileName.Contains(FileConstants.DIRECTORYSEPARATOR))
                return fileName.Substring(0, fileName.LastIndexOf(".")).Replace(Strings.spaceString, Strings.underscore).ToLower(CultureInfo.InvariantCulture);
            else
            {
                string moduleName = fileName.Substring(fileName.LastIndexOf(FileConstants.DIRECTORYSEPARATOR) + 1);
                return moduleName.Substring(0, moduleName.LastIndexOf(".")).Replace(Strings.spaceString, Strings.underscore).ToLower(CultureInfo.InvariantCulture);
            }
        }
    }
}
