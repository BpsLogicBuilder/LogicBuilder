using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FindAndReplace
{
    internal class GetSourceFilesForDocumentSearch : IGetSourceFilesForDocumentSearch
    {
        private readonly IConfigurationService _configurationService;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IMainWindow _mainWindow;
        private readonly IPathHelper _pathHelper;

        public GetSourceFilesForDocumentSearch(IConfigurationService configurationService, IExceptionHelper exceptionHelper, IMainWindow mainWindow, IPathHelper pathHelper)
        {
            _configurationService = configurationService;
            _exceptionHelper = exceptionHelper;
            _mainWindow = mainWindow;
            _pathHelper = pathHelper;
        }

        public IList<string> GetSourceFiles(string searchPattern, SearchOptions searchOptions)
        {
            string[] patterns = searchPattern.Split(new char[] { MiscellaneousConstants.SEMICOLONCHAR });
            IMDIParent mdiParent = (IMDIParent)_mainWindow.Instance;
            string documentPath = _pathHelper.CombinePaths
            (
                _configurationService.ProjectProperties.ProjectPath,
                ProjectPropertiesConstants.SOURCEDOCUMENTFOLDER
            );

            try
            {
                if (searchOptions == SearchOptions.All)
                    return GetMatchingFiles(documentPath, patterns);

                if (searchOptions == SearchOptions.OpenDocuments)
                {
                    string? openDocument = GetOpenDocument(mdiParent);
                    if (openDocument == null)
                        return Array.Empty<string>();

                    return CheckOpenDocumentForPatternMatch
                    (
                        openDocument,
                        _pathHelper.GetFilePath(openDocument),
                        patterns
                    );
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

            throw _exceptionHelper.CriticalException("{305326BA-D332-44D3-A4FB-91D9DEF6E4F3}");
        }

        private static IList<string> CheckOpenDocumentForPatternMatch(string openDocument, string documentPath, string[] patterns)
        {
            return patterns.Aggregate(new HashSet<string>(), (list, next) =>
            {
                string? matchingFile = Directory.GetFiles
                (
                    documentPath,
                    next,
                    SearchOption.TopDirectoryOnly
                )
                .SingleOrDefault(f => f.ToLowerInvariant() == openDocument.ToLowerInvariant());

                if (matchingFile != null)
                    list.Add(matchingFile);

                return list;
            }).ToList();
        }

        private static IList<string> GetMatchingFiles(string documentPath, string[] patterns)
        {
            return patterns.Aggregate(new HashSet<string>(), (list, next) =>
            {
                Directory.GetFiles(documentPath, next, SearchOption.AllDirectories)
                    .Select(f => f.ToLowerInvariant())
                    .ToList()
                    .ForEach(file => list.Add(file));

                return list;
            }).ToArray();
        }

        private static string? GetOpenDocument(IMDIParent mdiParent)
        {
            if (mdiParent.EditControl != null)
                return mdiParent.EditControl.SourceFile.ToLowerInvariant();

            return null;
        }
    }
}
