using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator.ShapeValidators;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using Microsoft.Office.Interop.Visio;
using System.Collections.Generic;
using System.Globalization;

namespace ABIS.LogicBuilder.FlowBuilder.RulesGenerator.ShapeValidators
{
    internal class ModuleShapeValidatorUtility : ShapeValidatorUtility
    {
        private readonly IApplicationSpecificFlowShapeValidator _applicationSpecificFlowShapeValidator;
        private readonly IModuleDataParser _moduleDataParser;
        private readonly IModuleNamesReader _moduleNamesReader;
        private readonly IShapeHelper _shapeHelper;
        private readonly IShapeXmlHelper _shapeXmlHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        public ModuleShapeValidatorUtility(
            string sourceFile,
            Page page,
            Shape shape,
            List<ResultMessage> validationErrors,
            IContextProvider contextProvider,
            IApplicationSpecificFlowShapeValidator applicationSpecificFlowShapeValidator,
            IModuleDataParser moduleDataParser,
            IShapeHelper shapeHelper,
            IShapeXmlHelper shapeXmlHelper) : base(sourceFile, page, shape, validationErrors, contextProvider)
        {
            _applicationSpecificFlowShapeValidator = applicationSpecificFlowShapeValidator;
            _moduleDataParser = moduleDataParser;
            _shapeHelper = shapeHelper;
            _shapeXmlHelper = shapeXmlHelper;
            _moduleNamesReader = contextProvider.ModuleNamesReader;
            _xmlDocumentHelpers = contextProvider.XmlDocumentHelpers;
        }

        internal override void Validate()
        {
            if (_shapeHelper.CountIncomingConnectors(this.Shape) < 1)
                AddValidationMessage(Strings.moduleShapeIncoming);

            string moduleShapeXml = _shapeXmlHelper.GetXmlString(this.Shape);
            if (moduleShapeXml.Length == 0)
            {
                AddValidationMessage(Strings.moduleShapeDataRequired);
                return;
            }

            string moduleName = _moduleDataParser.Parse(_xmlDocumentHelpers.ToXmlElement(moduleShapeXml));
            if (moduleName.Length == 0)
            {
                AddValidationMessage(Strings.moduleShapeDataRequired);
                return;
            }

            if (!_moduleNamesReader.GetNames().ContainsKey(moduleName))
            {
                AddValidationMessage
                (
                    string.Format(CultureInfo.CurrentCulture, Strings.invalidModuleName, moduleName)
                );

                return;
            }

            if (this.Shape.FromConnects.Count < 1)
                return;

            bool allApplication = _shapeHelper.HasAllApplicationConnectors(this.Shape);
            bool allNonApplication = _shapeHelper.HasAllNonApplicationConnectors(this.Shape);

            if (!(allApplication || allNonApplication))
            {
                AddValidationMessage(Strings.allConnectorsSameStencil);
                return;
            }

            if (allNonApplication)
                ValidateModuleShapeNonApplicationSpecific();

            if (allApplication)
            {
                _applicationSpecificFlowShapeValidator.Validate
                (
                    this.SourceFile,
                    this.Page,
                    this.Shape,
                    this.ValidationErrors
                );
            }
        }

        /// <summary>
        /// Ensures there is only one out going connector and that it is blank.
        /// </summary>
        private void ValidateModuleShapeNonApplicationSpecific()
        {
            if (_shapeHelper.CountOutgoingBlankConnectors(this.Shape) != 1 
                || _shapeHelper.CountOutgoingConnectors(this.Shape) != 1)
                AddValidationMessage(Strings.moduleShapeOutgoing);
        }
    }
}
