using ABIS.LogicBuilder.FlowBuilder.StructuresFactories;
using ABIS.LogicBuilder.FlowBuilder.RulesGenerator.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator.ShapeValidators;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using Microsoft.Office.Interop.Visio;
using System.Collections.Generic;
using System.Globalization;

namespace ABIS.LogicBuilder.FlowBuilder.Services.RulesGenerator.ShapeValidators
{
    internal class ModuleShapeValidator : IShapeValidator
    {
        private readonly IModuleDataParser _moduleDataParser;
        private readonly IModuleNamesReader _moduleNamesReader;
        private readonly IDiagramResultMessageHelper _resultMessageHelper;
        private readonly IShapeHelper _shapeHelper;
        private readonly IShapeXmlHelper _shapeXmlHelper;
        private readonly IRulesGeneratorFactory _rulesGeneratorFactory;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        public ModuleShapeValidator(
            IModuleDataParser moduleDataParser,
            IModuleNamesReader moduleNamesReader,
            IRulesGeneratorFactory rulesGeneratorFactory,
            IShapeHelper shapeHelper,
            IShapeXmlHelper shapeXmlHelper,
            IStructuresFactory structuresFactory,
            IXmlDocumentHelpers xmlDocumentHelpers,
            string sourceFile,
            Page page,
            Shape shape,
            List<ResultMessage> validationErrors)
        {
            _moduleDataParser = moduleDataParser;
            _moduleNamesReader = moduleNamesReader;
            _resultMessageHelper = structuresFactory.GetDiagramResultMessageHelper
            (
                sourceFile,
                page,
                shape,
                validationErrors
            );
            _shapeHelper = shapeHelper;
            _rulesGeneratorFactory = rulesGeneratorFactory;
            _shapeXmlHelper = shapeXmlHelper;
            _xmlDocumentHelpers = xmlDocumentHelpers;

            Page = page;
            Shape = shape;
            SourceFile = sourceFile;
            ValidationErrors = validationErrors;
        }

        private Page Page { get; }
        private Shape Shape { get; }
        private string SourceFile { get; }
        private List<ResultMessage> ValidationErrors { get; }

        public void Validate()
        {
            if (_shapeHelper.CountIncomingConnectors(this.Shape) < 1)
                _resultMessageHelper.AddValidationMessage(Strings.moduleShapeIncoming);

            string moduleShapeXml = _shapeXmlHelper.GetXmlString(this.Shape);
            if (moduleShapeXml.Length == 0)
            {
                _resultMessageHelper.AddValidationMessage(Strings.moduleShapeDataRequired);
                return;
            }

            string moduleName = _moduleDataParser.Parse(_xmlDocumentHelpers.ToXmlElement(moduleShapeXml));
            if (moduleName.Length == 0)
            {
                _resultMessageHelper.AddValidationMessage(Strings.moduleShapeDataRequired);
                return;
            }

            if (!_moduleNamesReader.GetNames().ContainsKey(moduleName))
            {
                _resultMessageHelper.AddValidationMessage
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
                _resultMessageHelper.AddValidationMessage(Strings.allConnectorsSameStencil);
                return;
            }

            if (allNonApplication)
                ValidateModuleShapeNonApplicationSpecific();

            if (allApplication)
            {
                _rulesGeneratorFactory.GetApplicationSpecificFlowShapeValidator
                (
                    this.SourceFile,
                    this.Page,
                    this.Shape,
                    this.ValidationErrors
                ).Validate();
            }
        }

        /// <summary>
        /// Ensures there is only one out going connector and that it is blank.
        /// </summary>
        private void ValidateModuleShapeNonApplicationSpecific()
        {
            if (_shapeHelper.CountOutgoingBlankConnectors(this.Shape) != 1
                || _shapeHelper.CountOutgoingConnectors(this.Shape) != 1)
                _resultMessageHelper.AddValidationMessage(Strings.moduleShapeOutgoing);
        }
    }
}
