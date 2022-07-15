using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.IO;
using System.IO.IsolatedStorage;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using System.Linq;
using System.Diagnostics.CodeAnalysis;

namespace ABIS.LogicBuilder.FlowBuilder
{
    internal static class RecentFilesList
    {
        static RecentFilesList()
        {
            Refresh();
        }
        #region Constants
        private const string RECENTFILESFILENAME = "RecentFiles.txt";
        private const int MAXIMUMFILES = 10;
        private const int MAXFULLNAMELENGTH = 60;
        private const string ABBREVIATIONTEXT = "...";
        #endregion Constants

        #region Variables
        private static List<string> fileList;
        private static Dictionary<string, string> fileDisplayList;
        #endregion Variables

        #region Properties
        internal static Dictionary<string, string> FileList
        {
            get
            {
                if (fileDisplayList.Count != fileList.Count 
                    || !fileDisplayList.Keys.SequenceEqual(fileList))
                {
                    fileDisplayList = BuildDisplayList();
                }

                return fileDisplayList;
            }
        }
        #endregion Properties

        #region Methods
        [MemberNotNull(nameof(fileList),
            nameof(fileDisplayList))]
        internal static void Refresh()
        {
            fileList = LoadIsolatedFileContents();
            fileDisplayList = BuildDisplayList();

            //remove none exist files from storage
            SaveIsolatedFileContents();
        }
        private static Dictionary<string, string> BuildDisplayList()
        {
            Dictionary<string, string> displayList = new();
            foreach (string file in fileList)
            {
                if (!string.IsNullOrEmpty(file) && file.Length > 2 && file.StartsWith(FileConstants.DIRECTORYSEPARATOR + FileConstants.DIRECTORYSEPARATOR) || file[1] == Path.VolumeSeparatorChar)//make sure it is the type of path we are expecting
                    displayList.Add(file.Trim(), BuildDisplayName(file));
            }
            return displayList;
        }

        private static string BuildDisplayName(string file)
        {
            string[] pathItems = file.Split(new char[] { Path.DirectorySeparatorChar }, StringSplitOptions.RemoveEmptyEntries);

            //Can't abbreviate if file is in the root directory
            if (pathItems.Length < 3)
                return file;

            //No need to abbreviate
            if (file.Length < MAXFULLNAMELENGTH)
                return file;

            //Can't abbreviate further
            if (pathItems.Length == 3 && pathItems[1] == ABBREVIATIONTEXT)
                return file;

            StringBuilder stringBuilder = new();

            stringBuilder.Append(file.StartsWith(FileConstants.DIRECTORYSEPARATOR + FileConstants.DIRECTORYSEPARATOR) ? string.Format(CultureInfo.InvariantCulture, FileConstants.DIRECTORYSEPARATOR + FileConstants.DIRECTORYSEPARATOR + "{0}", pathItems[0]) : pathItems[0]);
            if (pathItems[1] != ABBREVIATIONTEXT)
            {//replace the first subdirectory with "..."
                stringBuilder.Append(FileConstants.DIRECTORYSEPARATOR);
                stringBuilder.Append(ABBREVIATIONTEXT);
                for (int i = 2; i < pathItems.Length; i++)
                {
                    stringBuilder.Append(FileConstants.DIRECTORYSEPARATOR);
                    stringBuilder.Append(pathItems[i]);
                }
            }
            else
            {//replace "...\<nextSubdirectory>" with "..."
                stringBuilder.Append(FileConstants.DIRECTORYSEPARATOR);
                stringBuilder.Append(ABBREVIATIONTEXT);
                for (int i = 3; i < pathItems.Length; i++)
                {
                    stringBuilder.Append(FileConstants.DIRECTORYSEPARATOR);
                    stringBuilder.Append(pathItems[i]);
                }
            }

            return BuildDisplayName(stringBuilder.ToString());
        }


        private static void SaveIsolatedFileContents()
        {
            IsolatedStorageFile isolatedStorageFile = IsolatedStorageFile.GetUserStoreForAssembly();
            IsolatedStorageFileStream fileStream = new(RECENTFILESFILENAME, FileMode.Create, isolatedStorageFile);

            using StreamWriter streamWriter = new(fileStream, Encoding.Unicode);
            foreach (string file in fileList)
                streamWriter.WriteLine(file);
        }

        /// <summary>
        /// Add file to the top of tthe recently used files list
        /// </summary>
        /// <param name="file"></param>
        internal static void Add(string file)
        {
            int fileIndex = fileList.FindIndex(f => string.Compare(f, file, StringComparison.CurrentCultureIgnoreCase) == 0);

            if (fileIndex == -1)
            {
                fileList.Insert(0, file);
                while (fileList.Count > MAXIMUMFILES)
                    fileList.RemoveAt(fileList.Count - 1);
            }
            else
            {
                fileList.RemoveAt(fileIndex);
                fileList.Insert(0, file);
            }
            SaveIsolatedFileContents();
        }

        private static List<string> LoadIsolatedFileContents()
        {
            List<string> files = new();
            IsolatedStorageFile isolatedStorageFile = IsolatedStorageFile.GetUserStoreForAssembly();
            if (isolatedStorageFile.GetFileNames(RECENTFILESFILENAME).Length < 1)
                return files;

            IsolatedStorageFileStream fileStream = new(RECENTFILESFILENAME, FileMode.Open, isolatedStorageFile);
            using (StreamReader inStream = new(fileStream, Encoding.Unicode))
            {
                while (inStream.Peek() >= 0)
                {
                    string? file = inStream.ReadLine();
                    if (file == null || !File.Exists(file))
                        continue;

                    files.Add(file);
                }
            }
            return files;
        }
        #endregion Methods

        #region EventHandlers
        #endregion EventHandlers
    }
}
