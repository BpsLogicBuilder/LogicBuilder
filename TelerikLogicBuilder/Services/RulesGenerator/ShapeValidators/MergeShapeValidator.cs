using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.StructuresFactories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator.ShapeValidators;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using Microsoft.Office.Interop.Visio;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace ABIS.LogicBuilder.FlowBuilder.Services.RulesGenerator.ShapeValidators
{
    internal class MergeShapeValidator : IShapeValidator
    {
        private readonly IConfigurationService _configurationService;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IDiagramResultMessageHelper _resultMessageHelper;
        private readonly IShapeHelper _shapeHelper;

        public MergeShapeValidator(
            IConfigurationService configurationService,
            IExceptionHelper exceptionHelper,
            IPathHelper pathHelper,
            IShapeHelper shapeHelper,
            IStructuresFactory structuresFactory,
            string sourceFile,
            Page page,
            Shape shape,
            List<ResultMessage> validationErrors)
        {
            _configurationService = configurationService;
            _exceptionHelper = exceptionHelper;
            _resultMessageHelper = structuresFactory.GetDiagramResultMessageHelper
            (
                sourceFile,
                page,
                shape,
                validationErrors
            );
            _shapeHelper = shapeHelper;

            ModuleName = pathHelper.GetModuleName(sourceFile);
            Shape = shape;
        }

        private string ModuleName { get; }
        private Shape Shape { get; }

        public void Validate()
        {
            int outgoingNonApplicationCount = 0;
            int incomingNonApplicationCount = 0;
            int outgoingApplicationCount = 0;
            int incomingApplicationCount = 0;

            HashSet<string> applicationConnectors = ShapeCollections.ApplicationConnectors.ToHashSet();

            foreach (Connect fromConnect in this.Shape.FromConnects)
            {
                if (fromConnect.FromSheet.Master.NameU == UniversalMasterName.CONNECTOBJECT && fromConnect.FromPart == (short)VisFromParts.visBegin)
                    outgoingNonApplicationCount++;
                if (fromConnect.FromSheet.Master.NameU == UniversalMasterName.CONNECTOBJECT && fromConnect.FromPart == (short)VisFromParts.visEnd)
                    incomingNonApplicationCount++;
                if (applicationConnectors.Contains(fromConnect.FromSheet.Master.NameU) && fromConnect.FromPart == (short)VisFromParts.visBegin)
                    outgoingApplicationCount++;
                if (applicationConnectors.Contains(fromConnect.FromSheet.Master.NameU) && fromConnect.FromPart == (short)VisFromParts.visEnd)
                    incomingApplicationCount++;
            }

            if (outgoingApplicationCount > 0 && incomingApplicationCount > 0)
                _resultMessageHelper.AddValidationMessage(Strings.mergeHasInAndOutAppConnectors);

            if (outgoingNonApplicationCount > 0 && incomingNonApplicationCount > 0)
                _resultMessageHelper.AddValidationMessage(Strings.mergeHasInAndOutNonAppConnectors);

            if (outgoingApplicationCount > 0 && outgoingNonApplicationCount > 0)
                _resultMessageHelper.AddValidationMessage(Strings.mergeHasAppAndNonAppOutConnectors);

            if (incomingApplicationCount > 0 && incomingNonApplicationCount > 0)
                _resultMessageHelper.AddValidationMessage(Strings.mergeHasAppAndNonAppInConnectors);

            bool splitting = outgoingApplicationCount > 1 && incomingApplicationCount == 0;
            bool merging = outgoingApplicationCount == 0 && incomingApplicationCount > 1;

            if (!(merging || splitting))
                _resultMessageHelper.AddValidationMessage(Strings.mergeAppConnectorComments);

            if (splitting)
            {
                HashSet<string> outgoingConnectors = new();
                foreach (Connect shapeFromConnect in this.Shape.FromConnects)
                {
                    if (shapeFromConnect.FromPart == (short)VisFromParts.visBegin)
                    {
                        if (outgoingConnectors.Contains(shapeFromConnect.FromSheet.Master.NameU))
                            _resultMessageHelper.AddValidationMessage(Strings.duplicateOutgoingConnector);
                        else
                            outgoingConnectors.Add(shapeFromConnect.FromSheet.Master.NameU);
                    }

                    if (shapeFromConnect.FromSheet.Master.NameU == UniversalMasterName.OTHERSCONNECTOBJECT
                        && _shapeHelper.GetUnusedApplications(this.Shape, splitting).Count == 0)
                    {
                        _resultMessageHelper.AddValidationMessage(Strings.othersConnectorInvalid);
                    }
                }
            }

            if (merging)
            {
                HashSet<string> incomingConnectors = new();
                foreach (Connect shapeFromConnect in this.Shape.FromConnects)
                {
                    if (shapeFromConnect.FromPart == (short)VisFromParts.visEnd)
                    {
                        if (incomingConnectors.Contains(shapeFromConnect.FromSheet.Master.NameU))
                            _resultMessageHelper.AddValidationMessage(Strings.duplicateIncomingConnector);
                        else
                            incomingConnectors.Add(shapeFromConnect.FromSheet.Master.NameU);
                    }

                    if (shapeFromConnect.FromSheet.Master.NameU == UniversalMasterName.OTHERSCONNECTOBJECT
                        && _shapeHelper.GetUnusedApplications(this.Shape, splitting).Count == 0)
                    {
                        _resultMessageHelper.AddValidationMessage(Strings.othersConnectorInvalid);
                    }
                }
            }

            if (merging && outgoingNonApplicationCount != 1)
                _resultMessageHelper.AddValidationMessage(Strings.mergeMergingComments);

            if (splitting && incomingNonApplicationCount < 1)
                _resultMessageHelper.AddValidationMessage(Strings.mergeBranchingComments);

            if (merging || splitting)
            {
                IList<string> unusedApplications = _shapeHelper.GetUnusedApplications(this.Shape, splitting);
                foreach (string applicationName in unusedApplications)
                {
                    FlowBuilder.Configuration.Application? application = _configurationService.GetApplication(applicationName);
                    if (application == null)//unused applications do not inculde unconfigured applications.
                        throw _exceptionHelper.CriticalException("{1C7F083A-9A69-423F-B673-4A7B6173951C}");

                    if (!application.ExcludedModules.Contains(this.ModuleName))
                    {
                        _resultMessageHelper.AddValidationMessage
                        (
                            string.Format
                            (
                                CultureInfo.CurrentCulture,
                                Strings.applicationUnaccountedForFormat,
                                applicationName,
                                this.ModuleName
                            )
                        );
                    }
                }
            }
        }
    }
}
