using ABIS.LogicBuilder.FlowBuilder.Components.Helpers;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Forms;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using ABIS.LogicBuilder.FlowBuilder.UserControls;
using System.Threading;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Factories
{
    internal interface IServiceFactory
    {
        IApplicationDropDownList GetApplicationDropDownList(IApplicationForm applicationForm);
        IProgressForm GetProgressForm(Progress<ProgressMessage> progress, CancellationTokenSource cancellationTokenSource);
        ITreeViewXmlDocumentHelper GetTreeViewXmlDocumentHelper(SchemaName schema);
        ITypeAutoCompleteManager GetTypeAutoCompleteManager(IApplicationForm applicationForm,
            ITypeAutoCompleteTextControl textControl);
        IUpdateGenericArguments GetUpdateGenericArguments(IApplicationForm applicationForm);
    }
}
