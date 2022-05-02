using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using Microsoft.Office.Interop.Visio;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace ABIS.LogicBuilder.FlowBuilder.RulesGenerator.ShapeValidators
{
    internal class MergeShapeValidatorUtility : ShapeValidatorUtility
    {
        private readonly IConfigurationService _configurationService;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IShapeHelper _shapeHelper;

        public MergeShapeValidatorUtility(
            string sourceFile,
            Page page,
            Shape shape,
            List<ResultMessage> validationErrors,
            IContextProvider contextProvider,
            IShapeHelper shapeHelper) : base(sourceFile, page, shape, validationErrors, contextProvider)
        {
            _configurationService = contextProvider.ConfigurationService;
            _exceptionHelper = contextProvider.ExceptionHelper;
           _shapeHelper = shapeHelper;
        }

        internal override void Validate()
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
                AddValidationMessage(Strings.mergeHasInAndOutAppConnectors);

            if (outgoingNonApplicationCount > 0 && incomingNonApplicationCount > 0)
                AddValidationMessage(Strings.mergeHasInAndOutNonAppConnectors);

            if (outgoingApplicationCount > 0 && outgoingNonApplicationCount > 0)
                AddValidationMessage(Strings.mergeHasAppAndNonAppOutConnectors);

            if (incomingApplicationCount > 0 && incomingNonApplicationCount > 0)
                AddValidationMessage(Strings.mergeHasAppAndNonAppInConnectors);

            bool splitting = outgoingApplicationCount > 1 && incomingApplicationCount == 0;
            bool merging = outgoingApplicationCount == 0 && incomingApplicationCount > 1;

            if (!(merging || splitting))
                AddValidationMessage(Strings.mergeAppConnectorComments);

            if (splitting)
            {
                HashSet<string> outgoingConnectors = new();
                foreach (Connect shapeFromConnect in this.Shape.FromConnects)
                {
                    if (shapeFromConnect.FromPart == (short)VisFromParts.visBegin)
                    {
                        if (outgoingConnectors.Contains(shapeFromConnect.FromSheet.Master.NameU))
                            AddValidationMessage(Strings.duplicateOutgoingConnector);
                        else
                            outgoingConnectors.Add(shapeFromConnect.FromSheet.Master.NameU);
                    }

                    if (shapeFromConnect.FromSheet.Master.NameU == UniversalMasterName.OTHERSCONNECTOBJECT
                        && _shapeHelper.GetUnusedApplications(this.Shape, splitting).Count == 0)
                    {
                        AddValidationMessage(Strings.othersConnectorInvalid);
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
                            AddValidationMessage(Strings.duplicateIncomingConnector);
                        else
                            incomingConnectors.Add(shapeFromConnect.FromSheet.Master.NameU);
                    }

                    if (shapeFromConnect.FromSheet.Master.NameU == UniversalMasterName.OTHERSCONNECTOBJECT
                        && _shapeHelper.GetUnusedApplications(this.Shape, splitting).Count == 0)
                    {
                        AddValidationMessage(Strings.othersConnectorInvalid);
                    }
                }
            }

            if (merging && outgoingNonApplicationCount != 1)
                AddValidationMessage(Strings.mergeMergingComments);

            if (splitting && incomingNonApplicationCount < 1)
                AddValidationMessage(Strings.mergeBranchingComments);

            if (merging || splitting)
            {
                IList<string> unusedApplications = _shapeHelper.GetUnusedApplications(this.Shape, splitting);
                foreach (string applicationName in unusedApplications)
                {
                    Configuration.Application? application = _configurationService.GetApplication(applicationName);
                    if (application == null)//unused applications do not inculde unconfigured applications.
                        throw _exceptionHelper.CriticalException("{1C7F083A-9A69-423F-B673-4A7B6173951C}");

                    if (!application.ExcludedModules.Contains(this.ModuleName))
                    {
                        AddValidationMessage
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
