﻿using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using System.Globalization;
using System.Linq;

namespace ABIS.LogicBuilder.FlowBuilder.Services.Configuration
{
    internal class ConfigurationService : IConfigurationService
    {
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IPathHelper _pathHelper;
        private ProjectProperties? _projectProperties;
        private ConstructorList? _constructorList;
        private FragmentList? _fragmentList;
        private FunctionList? _functionList;
        private VariableList? _variableList;
        private string? _selectedApplication;

        public ConfigurationService(IExceptionHelper exceptionHelper, IPathHelper pathHelper)
        {
            _exceptionHelper = exceptionHelper;
            _pathHelper = pathHelper;
        }

        public ProjectProperties ProjectProperties
        {
            get
            {
                if (_projectProperties == null)
                    throw _exceptionHelper.CriticalException("{2F01EE09-8764-432E-846B-22D86A3860A9}");

                return _projectProperties;
            }

            set => _projectProperties = value;
        }

        public ConstructorList ConstructorList
        {
            get 
            {
                if (_constructorList == null)
                    throw _exceptionHelper.CriticalException("{F6D7B17D-11BE-4A30-A81D-90AF959CD58D}");

                return _constructorList; 
            }
            set => _constructorList = value;
        }

        public FragmentList FragmentList
        {
            get
            {
                if (_fragmentList == null)
                    throw _exceptionHelper.CriticalException("{26389294-6669-4969-B825-3464C85A7BBC}");

                return _fragmentList;
            }

            set => _fragmentList = value;
        }

        public FunctionList FunctionList
        {
            get
            {
                if (_functionList == null)
                    throw _exceptionHelper.CriticalException("{11085490-1C7B-47DF-88E5-97DCC64602EE}");

                return _functionList;
            }

            set => _functionList = value;
        }

        public VariableList VariableList
        {
            get
            {
                if (_variableList == null)
                    throw _exceptionHelper.CriticalException("{7B8C0DAE-E0DC-4FA2-8338-C0AB684E669D}");

                return _variableList;
            }

            set => _variableList = value;
        }

        public void ClearConfigurationData()
        {
            _projectProperties = null;
            _constructorList = null;
            _fragmentList = null;
            _functionList = null;
            _variableList = null;
            _selectedApplication = null;
        }

        public Application? GetApplication(string applicationName) 
            => ProjectProperties.ApplicationList.TryGetValue(applicationName.ToLower(CultureInfo.InvariantCulture), out Application? application) 
            ? application 
            : null;

        public Application GetApplicationFromPath(string path)
        {
            string ruleFolderFullName = _pathHelper.CombinePaths(ProjectProperties.ProjectPath, ProjectPropertiesConstants.RULESFOLDER);

            if (path.Length <= ruleFolderFullName.Length + 1)
                throw _exceptionHelper.CriticalException("{9C760EA5-3747-4B56-AD97-E2F5839CA4A9}");

            if (!path.StartsWith(ruleFolderFullName, true, CultureInfo.InvariantCulture))
                throw _exceptionHelper.CriticalException("{53959DC0-9AD2-4C6E-939A-C8C4CED59181}");

            string applicationName = path.Remove(0, ruleFolderFullName.Length + 1);
            if (applicationName.Contains(FileConstants.DIRECTORYSEPARATOR))
                applicationName = applicationName[..applicationName.IndexOf(FileConstants.DIRECTORYSEPARATOR)];

            if (ProjectProperties.ApplicationList.TryGetValue(applicationName.ToLower(CultureInfo.InvariantCulture), out Application? application))
                return application;

            throw _exceptionHelper.CriticalException("{AE753220-85DB-4F14-A131-2B87BB40EB92}");
        }

        public Application GetSelectedApplication()
        {
            if (_selectedApplication == null)
                return ProjectProperties.ApplicationList.First().Value;

            return ProjectProperties.ApplicationList[_selectedApplication.ToLowerInvariant()];
        }

        public string GetSelectedApplicationKey()
            => ProjectProperties.ApplicationList.Keys.First();

        public void SetSelectedApplication(string applicationName)
        {
            _selectedApplication = applicationName;
        }
    }
}
