using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator.ShapeValidators;
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
    internal class DiagramValidatorUtility
    {
        private readonly IContextProvider _contextProvider;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IGetRuleShapes _getRuleShapes;
        private readonly IJumpDataParser _jumpDataParser;
        private readonly IShapeHelper _shapeHelper;
        private readonly IShapeXmlHelper _shapeXmlHelper;
        private readonly IShapeValidator _shapeValidator;
        private readonly IResultMessageBuilder _resultMessageBuilder;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        public DiagramValidatorUtility(
            string sourceFile,
            Document document,
            ApplicationTypeInfo application,
            IProgress<ProgressMessage> progress,
            CancellationTokenSource cancellationTokenSource,
            IContextProvider contextProvider,
            IGetRuleShapes getRuleShapes,
            IJumpDataParser jumpDataParser,
            IShapeHelper shapeHelper,
            IShapeXmlHelper shapeXmlHelper,
            IShapeValidator shapeValidator)
        {
            SourceFile = sourceFile;
            FileName = contextProvider.PathHelper.GetFileName(SourceFile);
            Document = document;
            Application = application;
            Progress = progress;
            CancellationTokenSource = cancellationTokenSource;
            _exceptionHelper = contextProvider.ExceptionHelper;
            _resultMessageBuilder = contextProvider.ResultMessageBuilder;
            _xmlDocumentHelpers = contextProvider.XmlDocumentHelpers;
            _contextProvider = contextProvider;
            _getRuleShapes = getRuleShapes;
            _jumpDataParser = jumpDataParser;
            _shapeHelper = shapeHelper;
            _shapeXmlHelper = shapeXmlHelper;
            _shapeValidator = shapeValidator;
        }

        #region Properties
        private string SourceFile { get; }
        private string FileName { get; }
        private Document Document { get; }
        private ApplicationTypeInfo Application { get; }
        private List<ResultMessage> ValidationErrors { get; } = new List<ResultMessage>();
        private IProgress<ProgressMessage> Progress { get; }
        private CancellationTokenSource CancellationTokenSource { get; }
        private IDictionary<string, Shape>? JumpToShapes { get; set; }
        private IList<Shape> UsedConnectors { get; } = new List<Shape>();
        private int connectorCount;
        #endregion Properties

        internal async Task<IList<ResultMessage>> Validate()
        {
            await Task.Run(PreValidate, CancellationTokenSource.Token);
            if (ValidationErrors.Count > 0)
                return ValidationErrors;

            JumpToShapes = GetJumpToShapes();

            await Task.Run(ValidateShapes, CancellationTokenSource.Token);
            return ValidationErrors;
        }

        private void PreValidate()
        {
            Dictionary<string, IList<ShapeIdInfo>> jumpFromShapeTexts = new();
            Dictionary<string, ShapeIdInfo> jumpToShapeTexts = new();
            Shape? beginShape = null;
            int beginShapeCount = 0;
            List<ShapeIdInfo> endShapes = new();

            foreach (Page page in Document.Pages)
            {
                foreach (Shape shape in page.Shapes)
                {
                    if (shape.Master == null)
                    {
                        AddValidationMessage(Strings.invalidMaster, GetVisioFileSource(page, shape));
                        continue;
                    }

                    switch (shape.Master.NameU)
                    {
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
                            if (shape.Connects.Count != 2)
                            {
                                AddValidationMessage(Strings.connectorRequires2Shapes, GetVisioFileSource(page, shape));
                            }
                            break;
                        case UniversalMasterName.ACTION:
                        case UniversalMasterName.DIALOG:
                        case UniversalMasterName.CONDITIONOBJECT:
                        case UniversalMasterName.DECISIONOBJECT:
                            break;
                        case UniversalMasterName.JUMPOBJECT:
                            List<ResultMessage> jumpErrors = new();
                            _shapeValidator.ValidateShape(SourceFile, page, new ShapeBag(shape), jumpErrors, Application);
                            if (jumpErrors.Any())
                            {
                                jumpErrors.ForEach(error => ValidationErrors.Add(error));
                                break;
                            }

                            UpdateJumpDictionaries(page, shape, jumpFromShapeTexts, jumpToShapeTexts);
                            break;
                        case UniversalMasterName.MODULE:
                        case UniversalMasterName.WAITCONDITIONOBJECT:
                        case UniversalMasterName.WAITDECISIONOBJECT:
                            break;
                        case UniversalMasterName.BEGINFLOW:
                        case UniversalMasterName.MODULEBEGIN:
                            beginShape = shape;
                            beginShapeCount++;
                            break;
                        case UniversalMasterName.ENDFLOW:
                        case UniversalMasterName.MODULEEND:
                        case UniversalMasterName.TERMINATE:
                            endShapes.Add(new(shape.Master.NameU, shape.Master.Name, page.Index, shape.Index, page.ID, shape.ID));
                            break;
                        case UniversalMasterName.MERGEOBJECT:
                        case UniversalMasterName.COMMENT:
                            break;
                        default:
                            AddValidationMessage(Strings.invalidMaster, GetVisioFileSource(page, shape));
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

            if (beginShape == null)
                ValidationErrors.Add(new ResultMessage(string.Format(CultureInfo.CurrentCulture, Strings.beginFlowShapeRequired, this.FileName)));

            if (beginShapeCount > 1)
                ValidationErrors.Add(new ResultMessage(string.Format(CultureInfo.CurrentCulture, Strings.beginShapeCount, this.FileName)));

            ValidateJumpsMatching(jumpFromShapeTexts, jumpToShapeTexts);
            ValidateForModuleEndInBeginPolicy(beginShape, endShapes);
        }

        private void ValidateShapes()
        {
            ValidateShapes(new ShapeBag(FindBeginShape()));
        }

        private void ValidateShapes(ShapeBag shapeBag)
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

                _getRuleShapes.GetShapes(fromConnect.FromSheet, ruleShapes, ruleConnectors, JumpToShapes!);
                Validate(ruleShapes, ruleConnectors);

                foreach (Shape connector in ruleConnectors)
                {
                    if (!UsedConnectors.Contains(connector))
                        UsedConnectors.Add(connector);
                }

                Progress.Report
                (
                    new ProgressMessage
                    (
                        (int)((float)UsedConnectors.Count / (float)connectorCount * 100),
                        string.Format(CultureInfo.CurrentCulture, Strings.progressFormTaskValidatingFormat, FileName)
                    )
                );

                ShapeBag lastShapeBag = ruleShapes[^1];
                if (!ShapeCollections.EndModuleShapes.ToHashSet().Contains(lastShapeBag.Shape.Master.NameU))
                {
                    ValidateShapes(lastShapeBag);
                }
            }
        }

        private void Validate(IList<ShapeBag> ruleShapes, List<Shape> ruleConnectors)
        {
            foreach (Shape connector in ruleConnectors)
            {
                _shapeValidator.ValidateConnector(SourceFile, connector.ContainingPage, connector, ValidationErrors, Application);
            }

            foreach (ShapeBag shapeBag in ruleShapes)
            {
                ValidateOutgoingBlankConnectors(shapeBag.Shape, shapeBag.Shape.ContainingPage);
                _shapeValidator.ValidateShape(SourceFile, shapeBag.Shape.ContainingPage, shapeBag, ValidationErrors, Application);
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

            throw _exceptionHelper.CriticalException("{31C4473A-86DD-4F6C-AB6B-475E138AFC88}");
        }

        private IDictionary<string, Shape> GetJumpToShapes()
        {
            var jumpToShapes = new Dictionary<string, Shape>();
            foreach (Page page in Document.Pages)
            {
                foreach (Shape shape in page.Shapes)
                {
                    if (shape.Master.NameU == UniversalMasterName.JUMPOBJECT
                        && shape.FromConnects.Count > 0
                        && shape.FromConnects[1].FromPart == (short)VisFromParts.visBegin)
                    {
                        jumpToShapes.Add
                        (
                            _jumpDataParser.Parse
                            (
                                _xmlDocumentHelpers.ToXmlElement(_shapeXmlHelper.GetXmlString(shape))
                            ),
                            shape
                        );
                    }
                }
            }

            return jumpToShapes;
        }

        private VisioFileSource GetVisioFileSource(Page page, Shape shape)
            => new
            (
                SourceFile,
                page.ID,
                page.Index,
                shape.Master.Name,
                shape.ID,
                shape.Index,
                _contextProvider
            );

        private VisioFileSource GetVisioFileSource(ShapeIdInfo shapeIdInfo)
            => new
            (
                SourceFile,
                shapeIdInfo.PageId,
                shapeIdInfo.PageIndex,
                shapeIdInfo.ShapeMasterName,
                shapeIdInfo.ShapeId,
                shapeIdInfo.ShapeIndex,
                _contextProvider
            );

        private void AddValidationMessage(string message, VisioFileSource visioFileSource)
            => ValidationErrors.Add(GetResultMessage(message, visioFileSource));

        private ResultMessage GetResultMessage(string message, VisioFileSource visioFileSource)
            => _resultMessageBuilder.BuilderMessage(visioFileSource, message);

        private void UpdateJumpDictionaries(Page page, Shape shape, Dictionary<string, IList<ShapeIdInfo>> jumpFromShapeTexts, Dictionary<string, ShapeIdInfo> jumpToShapeTexts)
        {
            string jumpShapeXml = _shapeXmlHelper.GetXmlString(shape);
            if (jumpShapeXml.Length == 0)
            {//we exit on any error from _shapeValidator.Validate() so this doesn't run
                AddValidationMessage(Strings.jumpShapeDataRequired, GetVisioFileSource(page, shape));
                return;
            }

            string jumpShapeText = _jumpDataParser.Parse(_xmlDocumentHelpers.ToXmlElement(jumpShapeXml));
            if (jumpShapeText.Length == 0)
            {//we exit on any error from _shapeValidator.Validate() so this doesn't run
                AddValidationMessage(Strings.jumpShapeDataRequired, GetVisioFileSource(page, shape));
                return;
            }

            if (ShapeIsToJumpShape() && jumpToShapeTexts.ContainsKey(jumpShapeText))
            {
                AddValidationMessage(Strings.jumpShapeUniqueTextForToShape, GetVisioFileSource(page, shape));
                return;
            }

            ShapeIdInfo shapeData = new(shape.Master.NameU, shape.Master.Name, page.Index, shape.Index, page.ID, shape.ID);
            if (ShapeIsToJumpShape())
            {//(only one can exist)
                jumpToShapeTexts.Add(jumpShapeText, shapeData);
            }
            else if (ShapeIsFromJumpShape())
            {
                if (jumpFromShapeTexts.TryGetValue(jumpShapeText, out IList<ShapeIdInfo>? shapeList))
                    shapeList.Add(shapeData);
                else
                    jumpFromShapeTexts.Add(jumpShapeText, new List<ShapeIdInfo> { shapeData });
            }

            bool ShapeIsFromJumpShape()
                => shape.FromConnects[1].FromPart == (short)VisFromParts.visEnd;

            bool ShapeIsToJumpShape()
                => shape.FromConnects[1].FromPart == (short)VisFromParts.visBegin;
        }

        /// <summary>
        /// "To" Jump Shape requires matching "From" Jump Shape and vice versa
        /// </summary>
        /// <param name="jumpFromShapeTexts"></param>
        /// <param name="jumpToShapeTexts"></param>
        private void ValidateJumpsMatching(Dictionary<string, IList<ShapeIdInfo>> jumpFromShapeTexts, Dictionary<string, ShapeIdInfo> jumpToShapeTexts)
        {
            foreach (string jumpText in jumpFromShapeTexts.Keys)
            {
                IList<ShapeIdInfo> fromShapes = jumpFromShapeTexts[jumpText];
                if (!jumpToShapeTexts.ContainsKey(jumpText))
                {
                    foreach (ShapeIdInfo shapeIdInfo in fromShapes)
                    {
                        AddValidationMessage(Strings.jumpShapeNoMatchingToShape, GetVisioFileSource(shapeIdInfo));
                    }
                }
            }

            foreach (string jumpText in jumpToShapeTexts.Keys)
            {
                ShapeIdInfo toShapeInfo = jumpToShapeTexts[jumpText];
                if (!jumpFromShapeTexts.ContainsKey(jumpText))
                {
                    AddValidationMessage(Strings.jumpShapeNoMatchingFromShape, GetVisioFileSource(toShapeInfo));
                }
            }
        }

        /// <summary>
        /// Ensures that MODULEEND and BEGINFLOW do not exist in the same document 
        /// </summary>
        /// <param name="beginShape"></param>
        /// <param name="endShapes"></param>
        private void ValidateForModuleEndInBeginPolicy(Shape? beginShape, List<ShapeIdInfo> endShapes)
        {
            if (beginShape == null)
                return;

            if (beginShape.Master.NameU != UniversalMasterName.BEGINFLOW)
                return;

            foreach (ShapeIdInfo shapeIdInfo in endShapes)
            {
                if (shapeIdInfo.ShapeMasterNameU == UniversalMasterName.MODULEEND)
                    AddValidationMessage(Strings.moduleEndIsInvalidForBeginFlow, GetVisioFileSource(shapeIdInfo));
            }
        }

        /// <summary>
        /// Ensures that only one blank connector exits a shape except when application logic diverges
        /// </summary>
        /// <param name="shape"></param>
        /// <param name="page"></param>
        private void ValidateOutgoingBlankConnectors(Shape shape, Page page)
        {
            switch (shape.Master.NameU)
            {
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
                    break;
                case UniversalMasterName.DIALOG:
                    if (_shapeHelper.CountOutgoingNonApplicationConnectors(shape) > 1 && _shapeHelper.CountDialogFunctions(shape) == 0)
                        AddValidationMessage(Strings.dialogBoxOutgoingCount, GetVisioFileSource(page, shape));
                    break;
                case UniversalMasterName.ACTION:
                case UniversalMasterName.MERGEOBJECT:
                case UniversalMasterName.MODULE:
                case UniversalMasterName.WAITCONDITIONOBJECT:
                case UniversalMasterName.WAITDECISIONOBJECT:
                    if (_shapeHelper.CountOutgoingNonApplicationConnectors(shape) > 1)
                        AddValidationMessage(Strings.shapeBoxOutgoingBlanks, GetVisioFileSource(page, shape));
                    break;
                default:
                    if (_shapeHelper.CountOutgoingBlankConnectors(shape) > 1)
                        AddValidationMessage(Strings.shapeBoxOutgoingBlanks, GetVisioFileSource(page, shape));
                    break;
            }
        }
    }
}
