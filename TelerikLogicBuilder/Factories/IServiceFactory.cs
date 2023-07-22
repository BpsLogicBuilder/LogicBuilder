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
        IApplicationDropDownList GetApplicationDropDownList(IApplicationHostControl applicationHostControl);
        IConnectorObjectTypeAutoCompleteManager GetConnectorObjectTypeAutoCompleteManager(IApplicationHostControl applicationHostControl,
            ITypeAutoCompleteTextControl textControl);
        IProgressForm GetProgressForm(Progress<ProgressMessage> progress, CancellationTokenSource cancellationTokenSource);
        ISplashScreen GetSplashScreen();
        ITreeViewXmlDocumentHelper GetTreeViewXmlDocumentHelper(SchemaName schema);
        ITypeAutoCompleteManager GetTypeAutoCompleteManager(IApplicationHostControl applicationHostControl,
            ITypeAutoCompleteTextControl textControl);
        IUpdateGenericArguments GetUpdateGenericArguments(IApplicationHostControl applicationHostControl);
    }
}
