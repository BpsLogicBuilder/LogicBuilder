﻿using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Reflection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security;

namespace ABIS.LogicBuilder.FlowBuilder.Services.Reflection
{
    internal class LoadAssemblyFromName : ILoadAssemblyFromName
    {
        private readonly IAssemblyLoadContextManager _assemblyLoadContextManager;
        private readonly IPathHelper _pathHelper;

        private readonly string activityAssemblyFullName;
        private readonly LogicBuilderAssemblyLoadContext assemblyLoadContext;
        private readonly string[] paths;

        public LoadAssemblyFromName(
            IAssemblyLoadContextManager assemblyLoadContextManager,
            IPathHelper pathHelper,
            string activityAssemblyFullName,
            string[] paths)
        {
            _assemblyLoadContextManager = assemblyLoadContextManager;
            _pathHelper = pathHelper;
            this.activityAssemblyFullName = activityAssemblyFullName;
            this.assemblyLoadContext = _assemblyLoadContextManager.GetAssemblyLoadContext();
            this.paths = paths;
        }

        private const string DOTDLL = ".DLL";
        private const string COMMA = ",";

        public Assembly? LoadAssembly(AssemblyName assemblyName)
        {
            if (assemblyName == null)
                return null;

            Assembly assembly;
            LinkedList<string> path = new(this.GetPaths());
            try
            {
                string name = assemblyName.FullName.Contains(COMMA) ? assemblyName.FullName[..assemblyName.FullName.IndexOf(COMMA, StringComparison.Ordinal)] : assemblyName.FullName;
                assembly = LoadAssembly
                (
                    path.First!,/*path.First is never null.  GetPaths() adds this.activityAssemblyFullName if the path arrays is empty.*/
                    string.Concat(name, DOTDLL)
                ) ?? this.assemblyLoadContext.LoadFromAssemblyName(assemblyName);
            }
            catch (IOException ex)
            {
                throw new LogicBuilderException(ex.Message, ex);
            }
            catch (ArgumentException ex)
            {
                throw new LogicBuilderException(ex.Message, ex);
            }
            catch (BadImageFormatException ex)
            {
                throw new LogicBuilderException(ex.Message, ex);
            }
            catch (SecurityException ex)
            {
                throw new LogicBuilderException(ex.Message, ex);
            }

            return assembly;
        }

        private List<string> GetPaths()
        {
            Dictionary<string, string> pathsDictionary = new();
            if (!string.IsNullOrEmpty(this.activityAssemblyFullName))
            {
                string path = _pathHelper.RemoveTrailingPathSeparator(_pathHelper.GetFilePath(this.activityAssemblyFullName));
                pathsDictionary.Add(path.ToLowerInvariant(), path);
            }

            if (this.paths?.Length > 0)
            {
                pathsDictionary = this.paths.Aggregate(pathsDictionary, (dic, next) =>
                {
                    string newPath = _pathHelper.RemoveTrailingPathSeparator(next);
                    if (!dic.ContainsKey(newPath.ToLowerInvariant()))
                        dic.Add(newPath.ToLowerInvariant(), newPath);
                    return dic;
                });
            }

            return pathsDictionary.Values.ToList();
        }

        private Assembly? LoadAssembly(LinkedListNode<string> path, string file)
        {
            string fullName = _pathHelper.CombinePaths(path.Value, file);
            if (File.Exists(fullName))
                return assemblyLoadContext.LoadFromFileStream(fullName);
            else if (path.Next != null)
                return LoadAssembly(path.Next, file);
            else
                return null;
        }
    }
}
