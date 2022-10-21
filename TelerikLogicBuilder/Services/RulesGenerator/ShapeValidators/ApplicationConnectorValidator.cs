using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator.ShapeValidators;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using Microsoft.Office.Interop.Visio;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace ABIS.LogicBuilder.FlowBuilder.Services.RulesGenerator.ShapeValidators
{
    internal class ApplicationConnectorValidator : IConnectorValidator
    {
        private readonly IConfigurationService _configurationService;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IModuleDataParser _moduleDataParser;
        private readonly IResultMessageHelper _resultMessageHelper;
        private readonly IShapeHelper _shapeHelper;
        private readonly IShapeXmlHelper _shapeXmlHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        public ApplicationConnectorValidator(
            IConfigurationService configurationService,
            IExceptionHelper exceptionHelper,
            IModuleDataParser moduleDataParser,
            IPathHelper pathHelper,
            IResultMessageHelperFactory resultMessageHelperfactory,
            IShapeHelper shapeHelper,
            IShapeXmlHelper shapeXmlHelper,
            IXmlDocumentHelpers xmlDocumentHelpers,
            string sourceFile,
            Page page,
            Shape shape,
            List<ResultMessage> validationErrors)
        {
            _configurationService = configurationService;
            _exceptionHelper = exceptionHelper;
            _moduleDataParser = moduleDataParser;
            _resultMessageHelper = resultMessageHelperfactory.GetResultMessageHelper
            (
                sourceFile,
                page,
                shape,
                validationErrors
            );
            _shapeHelper = shapeHelper;
            _shapeXmlHelper = shapeXmlHelper;
            _xmlDocumentHelpers = xmlDocumentHelpers;

            ModuleName = pathHelper.GetModuleName(sourceFile);
            Shape = shape;
        }

        private string ModuleName { get; }
        private Shape Shape { get; }

        public void Validate()
        {
            if (!ShapeCollections.ApplicationConnectors.ToHashSet().Contains(this.Shape.Master.NameU))
                throw _exceptionHelper.CriticalException("{61E2FE3E-A796-4422-BD23-DA1DE74A7145}");

            if (this.Shape.Connects.Count != 2)
            {
                _resultMessageHelper.AddValidationMessage(Strings.connectorRequires2Shapes);
                return;
            }

            if (_configurationService.ProjectProperties.ApplicationList.Count < 2)
            {
                _resultMessageHelper.AddValidationMessage(Strings.twoApplicationsMinimum);
                return;
            }

            Shape fromShape = _shapeHelper.GetFromShape(this.Shape);
            Shape toShape = _shapeHelper.GetToShape(this.Shape);

            if ((fromShape.Master.NameU == UniversalMasterName.ACTION || fromShape.Master.NameU == UniversalMasterName.JUMPOBJECT) && fromShape == toShape)
                _resultMessageHelper.AddValidationMessage(Strings.shapeConnectedToBothEnds);

            if (!ShapeCollections.AllowedApplicationFlowShapes.ToHashSet().Contains(fromShape.Master.NameU))
                _resultMessageHelper.AddValidationMessage(Strings.validApplicationSpecificShapes);

            if (!ShapeCollections.AllowedApplicationFlowShapes.ToHashSet().Contains(toShape.Master.NameU))
                _resultMessageHelper.AddValidationMessage(Strings.validApplicationSpecificShapes);

            if (this.Shape.Master.NameU != UniversalMasterName.OTHERSCONNECTOBJECT)
                ValidateApplicationSpecificConnectors(fromShape, toShape);
        }

        /// <summary>
        /// Validate connectors excluding 
        /// </summary>
        private void ValidateApplicationSpecificConnectors(Shape fromShape, Shape toShape)
        {
            string applicationName = _shapeHelper.GetApplicationName(this.Shape);
            FlowBuilder.Configuration.Application? application = _configurationService.GetApplication(applicationName);

            if (application == null)
            {
                _resultMessageHelper.AddValidationMessage(Strings.applicationNotConfigured);
                return;
            }

            if (application.ExcludedModules.Contains(this.ModuleName))
            {
                _resultMessageHelper.AddValidationMessage(string.Format(CultureInfo.CurrentCulture, Strings.moduleIsExcludedFormat, ModuleName, applicationName));
            }

            if (toShape.Master.NameU == UniversalMasterName.MODULE)
                ValidateExternalModule(toShape);

            if (fromShape.Master.NameU == UniversalMasterName.MODULE)
                ValidateExternalModule(fromShape);

            void ValidateExternalModule(Shape moduleShape)
            {
                string moduleXml = _shapeXmlHelper.GetXmlString(moduleShape);
                if (moduleXml.Length == 0)
                {
                    _resultMessageHelper.AddValidationMessage(Strings.moduleShapeDataRequired);
                    return;
                }

                string externalModule = _moduleDataParser.Parse
                (
                    _xmlDocumentHelpers.ToXmlElement(moduleXml)
                );

                if (application.ExcludedModules.Contains(externalModule))
                    _resultMessageHelper.AddValidationMessage(string.Format(CultureInfo.CurrentCulture, Strings.moduleIsExcludedFormat, externalModule, applicationName));
            }
        }
    }
}
