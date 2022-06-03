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
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IJumpDataParser _jumpDataParser;
        private readonly IShapeHelper _shapeHelper;
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
            IJumpDataParser jumpDataParser,
            IShapeHelper shapeHelper,
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
            _jumpDataParser = jumpDataParser;
            _shapeHelper = shapeHelper;
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

                UsedConnectors.Add(fromConnect.FromSheet);

                ruleShapes.Clear();
                ruleConnectors.Clear();
                ruleConnectors.Add(fromConnect.FromSheet);
                ruleShapes.Add(shapeBag);

                GetShapes(fromConnect.FromSheet, ruleShapes, ruleConnectors);
                GenerateRules(ruleShapes, ruleConnectors);

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

        private Shape GetNextApplicationSpecificConnector(Shape lastShape, string connectorMaterNameU)
        {
            foreach (Connect lastShapeFromConnect in lastShape.FromConnects)
            {
                if (lastShapeFromConnect.FromPart == (short)VisFromParts.visEnd)
                    continue;

                if (lastShapeFromConnect.FromSheet.Master.NameU == connectorMaterNameU)
                    return lastShapeFromConnect.FromSheet;
            }

            throw _exceptionHelper.CriticalException("{B5381FEA-3521-4B80-B96D-4E49E328FA6E}");
        }

        private ShapeBag GetShapeBag(Shape toShape, Shape connector, ShapeBag fromShapeBag)
        {
            if (connector.Master.NameU == UniversalMasterName.CONNECTOBJECT)
                return new ShapeBag(toShape);

            if (connector.Master.NameU != UniversalMasterName.CONNECTOBJECT && toShape.Master.NameU == UniversalMasterName.MERGEOBJECT)
                return new ShapeBag(toShape);

            if (ShapeCollections.ApplicationConnectors.ToHashSet().Contains(connector.Master.NameU))
                return new ShapeBag(toShape, _shapeHelper.GetOtherApplicationsList(connector, fromShapeBag));

            throw _exceptionHelper.CriticalException("{5F8D5C31-0CBA-4B97-AA0E-06FEBFD26876}");
        }

        private void GetShapes(Shape connector, IList<ShapeBag> ruleShapes, IList<Shape> ruleConnectors)
        {
            Shape lastShape = _shapeHelper.GetToShape(connector);

            switch (lastShape.Master.NameU)
            {
                case UniversalMasterName.ACTION:
                    ruleShapes.Add(GetShapeBag(lastShape, connector, ruleShapes[ruleShapes.Count - 1]));
                    break;
                case UniversalMasterName.JUMPOBJECT:
                    string jumpShapeText = _jumpDataParser.Parse
                    (
                        _xmlDocumentHelpers.ToXmlElement(_shapeXmlHelper.GetXmlString(lastShape))
                    );
                    Shape jumpToShape = JumpToShapes[jumpShapeText];
                    Shape jumpToShapeConnector = jumpToShape.FromConnects[1].FromSheet;
                    if (!UsedConnectors.Contains(jumpToShapeConnector))
                        UsedConnectors.Add(jumpToShapeConnector);

                    GetShapes(jumpToShapeConnector, ruleShapes, ruleConnectors);
                    return;//If lastShape is a jump object it can only have a visEnd connector attached
                           //so no reason to continue.
                default:
                    ruleShapes.Add(GetShapeBag(lastShape, connector, ruleShapes[ruleShapes.Count - 1]));
                    return;
            }

            if (lastShape.Master.NameU == UniversalMasterName.ACTION && ShapeCollections.ApplicationConnectors.ToHashSet().Contains(connector.Master.NameU))
            {//In this case the next connector selected is dependent on the previous connector
                Shape nextApplicationSpecificConnector = GetNextApplicationSpecificConnector(lastShape, connector.Master.NameU);

                if (!UsedConnectors.Contains(nextApplicationSpecificConnector))
                    UsedConnectors.Add(nextApplicationSpecificConnector);
                if (!ruleConnectors.Contains(nextApplicationSpecificConnector))
                    ruleConnectors.Add(nextApplicationSpecificConnector);

                GetShapes(nextApplicationSpecificConnector, ruleShapes, ruleConnectors);
                return;
            }

            foreach (Connect lastShapeFromConnect in lastShape.FromConnects)
            {
                if (lastShapeFromConnect.FromPart == (short)VisFromParts.visEnd)
                    continue;

                if (!UsedConnectors.Contains(lastShapeFromConnect.FromSheet))
                    UsedConnectors.Add(lastShapeFromConnect.FromSheet);
                if (!ruleConnectors.Contains(lastShapeFromConnect.FromSheet))//last shape is always Action at this point
                    ruleConnectors.Add(lastShapeFromConnect.FromSheet);     //because Jump Shape can't have a visBegin connector
                                                                            //and also be a last shape

                GetShapes(lastShapeFromConnect.FromSheet, ruleShapes, ruleConnectors);
                //Shape nextShape = _shapeHelper.GetToShape(lastShapeFromConnect.FromSheet);

                //switch (nextShape.Master.NameU)
                //{
                //    /*looks the same as doing this step on 208 to 218 above*/
                //    case UniversalMasterName.JUMPOBJECT:
                //        string jumpShapeText = _jumpDataParser.Parse
                //        (
                //            _xmlDocumentHelpers.ToXmlElement(_shapeXmlHelper.GetXmlString(nextShape))
                //        );

                //        Shape jumpToShape = JumpToShapes[jumpShapeText];
                //        Shape jumpToShapeConnector = jumpToShape.FromConnects[1].FromSheet;
                //        if (!UsedConnectors.Contains(jumpToShapeConnector))
                //            UsedConnectors.Add(jumpToShapeConnector);

                //        GetShapes(jumpToShapeConnector, ruleShapes, ruleConnectors);
                //        break;
                //    default:
                //        GetShapes(lastShapeFromConnect.FromSheet, ruleShapes, ruleConnectors);
                //        break;
                //}
            }
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
