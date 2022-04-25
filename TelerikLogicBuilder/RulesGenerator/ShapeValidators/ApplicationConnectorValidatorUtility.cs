using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using Microsoft.Office.Interop.Visio;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace ABIS.LogicBuilder.FlowBuilder.RulesGenerator.ShapeValidators
{
    internal class ApplicationConnectorValidatorUtility : ShapeValidatorUtility
    {
        private readonly IConfigurationService _configurationService;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IModuleDataParser _moduleDataParser;
        private readonly IShapeHelper _shapeHelper;
        private readonly IShapeXmlHelper _shapeXmlHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        public ApplicationConnectorValidatorUtility(
            string sourceFile,
            Page page,
            Shape shape,
            List<ResultMessage> validationErrors,
            IContextProvider contextProvider,
            IModuleDataParser moduleDataParser,
            IShapeHelper shapeHelper,
            IShapeXmlHelper shapeXmlHelper)
            : base(sourceFile, page, shape, validationErrors, contextProvider)
        {
            _configurationService = contextProvider.ConfigurationService;
            _exceptionHelper = contextProvider.ExceptionHelper;
            _xmlDocumentHelpers = contextProvider.XmlDocumentHelpers;
            _moduleDataParser = moduleDataParser;
            _shapeHelper = shapeHelper;
            _shapeXmlHelper = shapeXmlHelper;
        }

        internal override void Validate()
        {
            if (!ShapeCollections.ApplicationConnectors.ToHashSet().Contains(this.Shape.Master.NameU))
                throw _exceptionHelper.CriticalException("{260E54DC-EF66-4FEA-81E8-34BEFFE7A621}");

            if (this.Shape.Connects.Count != 2)
            {
                AddValidationMessage(Strings.connectorRequires2Shapes);
                return;
            }

            if (_configurationService.ProjectProperties.ApplicationList.Count < 2)
            {
                AddValidationMessage(Strings.twoApplicationsMinimum);
                return;
            }

            Shape fromShape = _shapeHelper.GetFromShape(this.Shape);
            Shape toShape = _shapeHelper.GetToShape(this.Shape);

            if ((fromShape.Master.NameU == UniversalMasterName.ACTION || fromShape.Master.NameU == UniversalMasterName.JUMPOBJECT) && fromShape == toShape)
                AddValidationMessage(Strings.shapeConnectedToBothEnds);

            if (!ShapeCollections.AllowedApplicationFlowShapes.ToHashSet().Contains(fromShape.Master.NameU))
                AddValidationMessage(Strings.validApplicationSpecificShapes);

            if (!ShapeCollections.AllowedApplicationFlowShapes.ToHashSet().Contains(toShape.Master.NameU))
                AddValidationMessage(Strings.validApplicationSpecificShapes);

            if (this.Shape.Master.NameU != UniversalMasterName.OTHERSCONNECTOBJECT)
                ValidateApplicationSpecificConnectors(fromShape, toShape);
        }

        /// <summary>
        /// Validate connectors excluding 
        /// </summary>
        private void ValidateApplicationSpecificConnectors(Shape fromShape, Shape toShape)
        {
            string applicationName = _shapeHelper.GetApplicationName(this.Shape);
            Configuration.Application? application = _configurationService.GetApplication(applicationName);

            if (application == null)
            {
                AddValidationMessage(Strings.applicationNotConfigured);
                return;
            }

            if (application.ExcludedModules.Contains(this.ModuleName))
            {
                AddValidationMessage(string.Format(CultureInfo.CurrentCulture, Strings.moduleIsExcludedFormat, ModuleName, applicationName));
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
                    AddValidationMessage(Strings.moduleShapeDataRequired);
                    return;
                }

                string externalModule = _moduleDataParser.Parse
                (
                    _xmlDocumentHelpers.ToXmlElement(moduleXml)
                );

                if (application.ExcludedModules.Contains(externalModule))
                    AddValidationMessage(string.Format(CultureInfo.CurrentCulture, Strings.moduleIsExcludedFormat, externalModule, applicationName));
            }
        }
    }
}
