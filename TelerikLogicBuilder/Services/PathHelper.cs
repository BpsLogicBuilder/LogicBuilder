using ABIS.LogicBuilder.FlowBuilder.Structures;
using System;
using System.Globalization;
using System.IO;
using System.Linq;

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

            return Path.Combine(RemoveTrailingPathSeparator(path1), TrimPathSeparators(path2));
        }

        public string CombinePaths(string path1, string path2, string path3)
        {
            if (string.IsNullOrEmpty(path1) || string.IsNullOrEmpty(path2) || string.IsNullOrEmpty(path3))
                throw _exceptionHelper.CriticalException("{D804D2BE-0D03-4FC3-A2C8-BC0E1A412483}");

            return Path.Combine(RemoveTrailingPathSeparator(path1), TrimPathSeparators(path2), TrimPathSeparators(path3));
        }

        public string CombinePaths(params string[] partialPaths)
        {
            if (partialPaths == null)
                return string.Empty;

            if (partialPaths.Length == 0)
                return string.Empty;

            return Path.Combine
            (
                new string[] { RemoveTrailingPathSeparator(partialPaths[0]) }.Concat
                (
                    partialPaths.Skip(1).Select(s => TrimPathSeparators(s))
                ).ToArray()
            );
        }

        internal static string TrimPathSeparators(string path)
        {
            path = RemoveTrailingPathSeparator(path);
            return RemoveLeadingPathSeparator(path);
        }

        public string GetExtension(string fileName) => fileName[fileName.LastIndexOf(".")..];

        public string GetFileName(string fileName) => Path.GetFileName(fileName);

        public string GetFileNameNoExtention(string fileName) => Path.GetFileNameWithoutExtension(fileName);

        public string GetFilePath(string fileName) => Path.GetDirectoryName(fileName);

        public string GetFolderName(string folderPath)
        {
            string fPath = folderPath.EndsWith(FileConstants.DIRECTORYSEPARATOR) ? folderPath[..folderPath.LastIndexOf(FileConstants.DIRECTORYSEPARATOR)] : folderPath;
            return fPath[(fPath.LastIndexOf(FileConstants.DIRECTORYSEPARATOR) + 1)..];
        }

        public string GetModuleName(string fileName)
        {
            if (!fileName.Contains(FileConstants.DIRECTORYSEPARATOR))
                return fileName[..fileName.LastIndexOf(".")].Replace(Strings.spaceString, Strings.underscore).ToLower(CultureInfo.InvariantCulture);
            else
            {
                string moduleName = fileName[(fileName.LastIndexOf(FileConstants.DIRECTORYSEPARATOR) + 1)..];
                return moduleName[..moduleName.LastIndexOf(".")].Replace(Strings.spaceString, Strings.underscore).ToLower(CultureInfo.InvariantCulture);
            }
        }

        private static string RemoveTrailingPathSeparator(string path)
        {
            return RemoveTrailingSeparator(path, FileConstants.DIRECTORYSEPARATOR);
        }

        private static string RemoveLeadingPathSeparator(string path)
        {
            return RemoveLeadingSeparator(path, FileConstants.DIRECTORYSEPARATOR);
        }

        private static string RemoveTrailingSeparator(string path, string separator)
        {
            if (path == null)
                return null;

            return path.Trim().EndsWith(separator, StringComparison.InvariantCulture)
                ? path.Trim()[..path.Trim().LastIndexOf(separator)]
                : path.Trim();
        }

        private static string RemoveLeadingSeparator(string path, string separator)
        {
            if (path == null)
                return null;

            return path.Trim().StartsWith(separator, StringComparison.InvariantCulture)
                ? path.Trim()[1..]
                : path.Trim();
        }
    }
}
