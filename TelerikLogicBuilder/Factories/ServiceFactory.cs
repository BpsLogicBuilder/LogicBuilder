using ABIS.LogicBuilder.FlowBuilder.Components.Helpers;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Forms;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using ABIS.LogicBuilder.FlowBuilder.UserControls;
using System;
using System.Threading;

namespace ABIS.LogicBuilder.FlowBuilder.Factories
{
    internal class ServiceFactory : IServiceFactory
    {
        private readonly Func<IApplicationHostControl, IApplicationDropDownList> _getApplicationDropDownList;
        private readonly Func<Progress<ProgressMessage>, CancellationTokenSource, IProgressForm> _getProgressForm;
        private readonly Func<SchemaName, ITreeViewXmlDocumentHelper> _getTreeViewXmlDocumentHelper;
        private readonly Func<IApplicationHostControl, ITypeAutoCompleteTextControl, ITypeAutoCompleteManager> _getTypeAutoCompleteManager;
        private readonly Func<IApplicationHostControl, IUpdateGenericArguments> _getUpdateGenericArguments;

        public ServiceFactory(
            Func<IApplicationHostControl, IApplicationDropDownList> getApplicationDropDownList,
            Func<Progress<ProgressMessage>, CancellationTokenSource, IProgressForm> getProgressForm,
            Func<SchemaName, ITreeViewXmlDocumentHelper> getTreeViewXmlDocumentHelper,
            Func<IApplicationHostControl, ITypeAutoCompleteTextControl, ITypeAutoCompleteManager> getTypeAutoCompleteManager,
            Func<IApplicationHostControl, IUpdateGenericArguments> getUpdateGenericArguments)
        {
            _getApplicationDropDownList = getApplicationDropDownList;
            _getProgressForm = getProgressForm;
            _getTreeViewXmlDocumentHelper = getTreeViewXmlDocumentHelper;
            _getTypeAutoCompleteManager = getTypeAutoCompleteManager;
            _getUpdateGenericArguments= getUpdateGenericArguments;
        }

        public IApplicationDropDownList GetApplicationDropDownList(IApplicationHostControl applicationHostControl)
            => _getApplicationDropDownList(applicationHostControl);

        public IProgressForm GetProgressForm(Progress<ProgressMessage> progress, CancellationTokenSource cancellationTokenSource)
            => _getProgressForm(progress, cancellationTokenSource);

        public ITreeViewXmlDocumentHelper GetTreeViewXmlDocumentHelper(SchemaName schema)
            => _getTreeViewXmlDocumentHelper(schema);

        public ITypeAutoCompleteManager GetTypeAutoCompleteManager(IApplicationHostControl applicationHostControl, ITypeAutoCompleteTextControl textControl)
            => _getTypeAutoCompleteManager(applicationHostControl, textControl);

        public IUpdateGenericArguments GetUpdateGenericArguments(IApplicationHostControl applicationHostControl)
            => _getUpdateGenericArguments(applicationHostControl);
    }
}
