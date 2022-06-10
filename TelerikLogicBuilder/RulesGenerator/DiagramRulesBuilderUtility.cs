using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.RulesGenerator.RuleBuilders;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator.RuleBuilders;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using Microsoft.Office.Interop.Visio;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ABIS.LogicBuilder.FlowBuilder.RulesGenerator
{
    internal class DiagramRulesBuilderUtility
    {
        private readonly IDiagramValidator _diagramValidator;
        private readonly IGetRuleShapes _getRuleShapes;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IJumpDataParser _jumpDataParser;
        private readonly IShapeSetRuleBuilder _shapeSetRuleBuilder;
        private readonly IShapeXmlHelper _shapeXmlHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        public DiagramRulesBuilderUtility(
            string sourceFile,
            Document document,
            ApplicationTypeInfo application,
            IProgress<ProgressMessage> progress,
            CancellationTokenSource cancellationTokenSource,
            IContextProvider contextProvider,
            IDiagramValidator diagramValidator,
            IGetRuleShapes getRuleShapes,
            IJumpDataParser jumpDataParser,
            IShapeSetRuleBuilder shapeSetRuleBuilder,
            IShapeXmlHelper shapeXmlHelper)
        {
            SourceFile = sourceFile;
            FileName = contextProvider.PathHelper.GetFileName(SourceFile);
            ModuleName = contextProvider.PathHelper.GetModuleName(FileName);
            Document = document;
            Application = application;
            Progress = progress;
            CancellationTokenSource = cancellationTokenSource;
            _exceptionHelper = contextProvider.ExceptionHelper;
            _xmlDocumentHelpers = contextProvider.XmlDocumentHelpers;
            _diagramValidator = diagramValidator;
            _getRuleShapes = getRuleShapes;
            _jumpDataParser = jumpDataParser;
            _shapeSetRuleBuilder = shapeSetRuleBuilder;
            _shapeXmlHelper = shapeXmlHelper; 
        }

        #region Properties
        private int connectorCount;
        private string SourceFile { get; }
        private string FileName { get; }
        private string ModuleName { get; }
        private Document Document { get; }
        private ApplicationTypeInfo Application { get; }
        private IList<ResultMessage> BuildErrors { get; } = new List<ResultMessage>();
        private IDictionary<string, Shape> JumpToShapes { get; } = new Dictionary<string, Shape>();
        private IList<Shape> UsedConnectors { get; } = new List<Shape>();
        private IDictionary<string, string> ResourceStrings { get; } = new Dictionary<string, string>();
        private List<RuleBag> Rules { get; } = new List<RuleBag>();
        private IProgress<ProgressMessage> Progress { get; }
        private CancellationTokenSource CancellationTokenSource { get; }
        #endregion Properties

        internal async Task<BuildRulesResult> BuildRules()
        {
            var validationErrors = await _diagramValidator.Validate(SourceFile, Document, Application, Progress, CancellationTokenSource);
            if (validationErrors.Count > 0)
                return new BuildRulesResult(validationErrors, Rules, ResourceStrings);

            InitializeBuild();

            Progress.Report
            (
                new ProgressMessage
                (
                    0,
                    string.Format(CultureInfo.CurrentCulture, Strings.progressFormTaskBuildingFormat, FileName)
                )
            );

            await Task.Run
            (
                () => Build(new ShapeBag(FindBeginShape())), CancellationTokenSource.Token
            );

            return new BuildRulesResult(BuildErrors, Rules, ResourceStrings);
        }

        private void Build(ShapeBag shapeBag)
        {
            List<ShapeBag> ruleShapes = new();
            List<Shape> ruleConnectors = new();

            foreach (Connect fromConnect in shapeBag.Shape.FromConnects)
            {
                if (fromConnect.FromPart == (short)VisFromParts.visEnd)
                    continue;

                if (UsedConnectors.Contains(fromConnect.FromSheet))
                    continue;

                ruleShapes.Clear();
                ruleConnectors.Clear();
                ruleConnectors.Add(fromConnect.FromSheet);
                ruleShapes.Add(shapeBag);

                _getRuleShapes.GetShapes(fromConnect.FromSheet, ruleShapes, ruleConnectors, JumpToShapes);

                foreach (Shape connector in ruleConnectors)
                {
                    if (!UsedConnectors.Contains(connector))
                        UsedConnectors.Add(connector);
                }

                GenerateRules(ruleShapes, new Shape[] { fromConnect.FromSheet });

                Progress.Report
                (
                    new ProgressMessage
                    (
                        (int)((float)UsedConnectors.Count / (float)connectorCount * 100),
                        string.Format(CultureInfo.CurrentCulture, Strings.progressFormTaskBuildingFormat, FileName)
                    )
                );

                ShapeBag lastShapeBag = ruleShapes[^1];
                if (!ShapeCollections.EndModuleShapes.ToHashSet().Contains(lastShapeBag.Shape.Master.NameU))
                {
                    Build(lastShapeBag);
                }
            }
        }

        private Shape FindBeginShape()
        {
            foreach (Page page in Document.Pages)
            {
                foreach (Shape shape in page.Shapes)
                {
                    if (shape.Master.NameU == UniversalMasterName.MODULEBEGIN
                        || shape.Master.NameU == UniversalMasterName.BEGINFLOW)
                        return shape;
                }
            }

            throw _exceptionHelper.CriticalException("{0F0DF93F-4803-42C2-98D2-1DE0897B3C2E}Z");
        }

        private void GenerateRules(IList<ShapeBag> ruleShapes, IList<Shape> ruleConnectors)
        {
            Rules.AddRange
            (
                _shapeSetRuleBuilder.GenerateRules
                (
                    ruleShapes[0].Shape.Master.NameU,
                    ruleShapes,
                    ruleConnectors,
                    ModuleName,
                    Rules.Count,
                    Application,
                    ResourceStrings
                )
            );
        }

        /// <summary>
        /// Iterates though all shapes in the document and collects neccessary information
        /// </summary>
        private void InitializeBuild()
        {
            foreach (Page page in Document.Pages)
            {
                foreach (Shape shape in page.Shapes)
                {
                    switch (shape.Master.NameU)
                    {
                        case UniversalMasterName.JUMPOBJECT:
                            if (shape.FromConnects.Count > 0 && shape.FromConnects[1].FromPart == (short)VisFromParts.visBegin)
                            {
                                string jumpShapeText = _jumpDataParser.Parse
                                (
                                    _xmlDocumentHelpers.ToXmlElement(_shapeXmlHelper.GetXmlString(shape))
                                );

                                JumpToShapes.Add(jumpShapeText, shape);
                            }
                            break;
                        case UniversalMasterName.APP01CONNECTOBJECT:
                        case UniversalMasterName.APP02CONNECTOBJECT:
                        case UniversalMasterName.APP03CONNECTOBJECT:
                        case UniversalMasterName.APP04CONNECTOBJECT:
                        case UniversalMasterName.APP05CONNECTOBJECT:
                        case UniversalMasterName.APP06CONNECTOBJECT:
                        case UniversalMasterName.APP07CONNECTOBJECT:
                        case UniversalMasterName.APP08CONNECTOBJECT:
                        case UniversalMasterName.APP09CONNECTOBJECT:
                        case UniversalMasterName.APP10CONNECTOBJECT:
                        case UniversalMasterName.OTHERSCONNECTOBJECT:
                        case UniversalMasterName.CONNECTOBJECT:
                            connectorCount++;
                            break;
                        default:
                            break;
                    }

                    Progress.Report
                    (
                        new ProgressMessage
                        (
                            (int)(((float)shape.Index / (float)page.Shapes.Count) * 100),
                            string.Format(CultureInfo.CurrentCulture, Strings.progressFormTaskInitializingPageFormat, page.Index)
                        )
                    );
                }
            }
        }
    }
}
