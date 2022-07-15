using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System;
using System.IO;
using System.Resources;
using System.Text;

namespace ABIS.LogicBuilder.FlowBuilder.Services
{
    internal class FileIOHelper : IFileIOHelper
    {
        public void CopyFile(FileInfo source, FileInfo destination)
        {
            try
            {
                if (destination.Exists)
                    destination.IsReadOnly = false;

                source.CopyTo(destination.FullName, true);
            }
            catch (ArgumentException ex)
            {
                throw new LogicBuilderException(ex.Message, ex);
            }
            catch (UnauthorizedAccessException ex)
            {
                throw new LogicBuilderException(ex.Message, ex);
            }
            catch (IOException ex)
            {
                throw new LogicBuilderException(ex.Message, ex);
            }
            catch (System.Security.SecurityException ex)
            {
                throw new LogicBuilderException(ex.Message, ex);
            }
            catch (NotSupportedException ex)
            {
                throw new LogicBuilderException(ex.Message, ex);
            }
        }

        public void CopyFile(string sourceFullName, string destinationFullName)
        {
            try
            {
                CopyFile(GetNewFileInfo(sourceFullName), GetNewFileInfo(destinationFullName));
            }
            catch (ArgumentException ex)
            {
                throw new LogicBuilderException(ex.Message, ex);
            }
            catch (UnauthorizedAccessException ex)
            {
                throw new LogicBuilderException(ex.Message, ex);
            }
            catch (IOException ex)
            {
                throw new LogicBuilderException(ex.Message, ex);
            }
            catch (System.Security.SecurityException ex)
            {
                throw new LogicBuilderException(ex.Message, ex);
            }
            catch (NotSupportedException ex)
            {
                throw new LogicBuilderException(ex.Message, ex);
            }
        }

        public void CreateDirectory(string fullName)
        {
            try
            {
                Directory.CreateDirectory(fullName);
            }
            catch (ArgumentException ex)
            {
                throw new LogicBuilderException(ex.Message, ex);
            }
            catch (UnauthorizedAccessException ex)
            {
                throw new LogicBuilderException(ex.Message, ex);
            }
            catch (IOException ex)
            {
                throw new LogicBuilderException(ex.Message, ex);
            }
        }

        public void DeleteFile(string fullName)
        {
            try
            {
                SetWritable(fullName, true);
                File.Delete(fullName);
            }
            catch (ArgumentException ex)
            {
                throw new LogicBuilderException(ex.Message, ex);
            }
            catch (UnauthorizedAccessException ex)
            {
                throw new LogicBuilderException(ex.Message, ex);
            }
            catch (IOException ex)
            {
                throw new LogicBuilderException(ex.Message, ex);
            }
            catch (System.Security.SecurityException ex)
            {
                throw new LogicBuilderException(ex.Message, ex);
            }
        }

        public void DeleteFolder(string fullName, bool recursive)
        {
            try
            {
                DirectoryInfo directoryInfo = new(fullName);
                foreach (FileInfo fileInfo in directoryInfo.GetFiles("*", SearchOption.AllDirectories))
                    fileInfo.IsReadOnly = false;
                Directory.Delete(fullName, recursive);
            }
            catch (ArgumentException ex)
            {
                throw new LogicBuilderException(ex.Message, ex);
            }
            catch (UnauthorizedAccessException ex)
            {
                throw new LogicBuilderException(ex.Message, ex);
            }
            catch (IOException ex)
            {
                throw new LogicBuilderException(ex.Message, ex);
            }
            catch (System.Security.SecurityException ex)
            {
                throw new LogicBuilderException(ex.Message, ex);
            }
        }

        public byte[] GetFileBytes(string fullName)
        {
            try
            {
                FileInfo fileInfo = new(fullName);
                byte[] byteArray;
                using (FileStream fileStream = new(fileInfo.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    byteArray = new byte[fileInfo.Length];
                    fileStream.Read(byteArray, 0, (int)fileInfo.Length);
                    fileStream.Close();
                }

                return byteArray;
            }
            catch (ArgumentNullException ex)
            {
                throw new LogicBuilderException(ex.Message, ex);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                throw new LogicBuilderException(ex.Message, ex);
            }
            catch (ArgumentException ex)
            {
                throw new LogicBuilderException(ex.Message, ex);
            }
            catch (NotSupportedException ex)
            {
                throw new LogicBuilderException(ex.Message, ex);
            }
            catch (UnauthorizedAccessException ex)
            {
                throw new LogicBuilderException(ex.Message, ex);
            }
            catch (IOException ex)
            {
                throw new LogicBuilderException(ex.Message, ex);
            }
            catch (System.Security.SecurityException ex)
            {
                throw new LogicBuilderException(ex.Message, ex);
            }
        }

        public DirectoryInfo GetNewDirectoryInfo(string fullName)
        {
            try
            {
                return new DirectoryInfo(fullName);
            }
            catch (ArgumentException ex)
            {
                throw new LogicBuilderException(ex.Message, ex);
            }
            catch (IOException ex)
            {
                throw new LogicBuilderException(ex.Message, ex);
            }
            catch (System.Security.SecurityException ex)
            {
                throw new LogicBuilderException(ex.Message, ex);
            }
        }

        public FileInfo GetNewFileInfo(string fullName)
        {
            try
            {
                return new FileInfo(fullName);
            }
            catch (ArgumentException ex)
            {
                throw new LogicBuilderException(ex.Message, ex);
            }
            catch (UnauthorizedAccessException ex)
            {
                throw new LogicBuilderException(ex.Message, ex);
            }
            catch (IOException ex)
            {
                throw new LogicBuilderException(ex.Message, ex);
            }
            catch (System.Security.SecurityException ex)
            {
                throw new LogicBuilderException(ex.Message, ex);
            }
        }

        public IResourceReader GetResourceReader(string fullName)
        {
            try
            {
                return new ResourceReader(fullName);
            }
            catch (ArgumentNullException ex)
            {
                throw new LogicBuilderException(ex.Message, ex);
            }
            catch (ArgumentException ex)
            {
                throw new LogicBuilderException(ex.Message, ex);
            }
            catch (IOException ex)
            {
                throw new LogicBuilderException(ex.Message, ex);
            }
        }

        public void MoveFile(string source, string destination)
        {
            try
            {
                File.Move(source, destination);
            }
            catch (ArgumentException ex)
            {
                throw new LogicBuilderException(ex.Message, ex);
            }
            catch (UnauthorizedAccessException ex)
            {
                throw new LogicBuilderException(ex.Message, ex);
            }
            catch (IOException ex)
            {
                throw new LogicBuilderException(ex.Message, ex);
            }
            catch (System.Security.SecurityException ex)
            {
                throw new LogicBuilderException(ex.Message, ex);
            }
            catch (NotSupportedException ex)
            {
                throw new LogicBuilderException(ex.Message, ex);
            }
        }

        public void MoveFolder(string source, string destination)
        {
            try
            {
                Directory.Move(source, destination);
            }
            catch (ArgumentException ex)
            {
                throw new LogicBuilderException(ex.Message, ex);
            }
            catch (UnauthorizedAccessException ex)
            {
                throw new LogicBuilderException(ex.Message, ex);
            }
            catch (IOException ex)
            {
                throw new LogicBuilderException(ex.Message, ex);
            }
            catch (System.Security.SecurityException ex)
            {
                throw new LogicBuilderException(ex.Message, ex);
            }
        }

        public void SaveFile(string fileFullName, string text)
        {
            try
            {
                SetWritable(fileFullName, true);
                using StreamWriter streamWriter = new(fileFullName, false, Encoding.Unicode);
                streamWriter.Write(text);
            }
            catch (ArgumentException ex)
            {
                throw new LogicBuilderException(ex.Message, ex);
            }
            catch (IOException ex)
            {
                throw new LogicBuilderException(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                throw new LogicBuilderException(ex.Message);
            }
            catch (System.Security.SecurityException ex)
            {
                throw new LogicBuilderException(ex.Message);
            }
        }

        public void SaveNewFile(string fileFullName, string text)
        {
            try
            {
                using FileStream fileStream = new(fileFullName, FileMode.Create, FileAccess.Write);
                using StreamWriter streamWriter = new(fileStream);
                streamWriter.Write(text);
            }
            catch (ArgumentException ex)
            {
                throw new LogicBuilderException(ex.Message, ex);
            }
            catch (IOException ex)
            {
                throw new LogicBuilderException(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                throw new LogicBuilderException(ex.Message);
            }
            catch (System.Security.SecurityException ex)
            {
                throw new LogicBuilderException(ex.Message);
            }
        }

        public void SetWritable(string fullName, bool writable)
        {
            try
            {
                if (File.Exists(fullName))
                {
                    FileInfo fileInfo = new(fullName)
                    {
                        IsReadOnly = !writable
                    };
                }
            }
            catch (ArgumentException ex)
            {
                throw new LogicBuilderException(ex.Message, ex);
            }
            catch (UnauthorizedAccessException ex)
            {
                throw new LogicBuilderException(ex.Message, ex);
            }
            catch (IOException ex)
            {
                throw new LogicBuilderException(ex.Message, ex);
            }
            catch (System.Security.SecurityException ex)
            {
                throw new LogicBuilderException(ex.Message, ex);
            }
        }
    }
}
