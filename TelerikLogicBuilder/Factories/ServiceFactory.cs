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
        private readonly Func<IApplicationForm, IApplicationDropDownList> _getApplicationDropDownList;
        private readonly Func<Progress<ProgressMessage>, CancellationTokenSource, IProgressForm> _getProgressForm;
        private readonly Func<SchemaName, ITreeViewXmlDocumentHelper> _getTreeViewXmlDocumentHelper;
        private readonly Func<IApplicationForm, ITypeAutoCompleteTextControl, ITypeAutoCompleteManager> _getTypeAutoCompleteManager;
        private readonly Func<IApplicationForm, IUpdateGenericArguments> _getUpdateGenericArguments;

        public ServiceFactory(
            Func<IApplicationForm, IApplicationDropDownList> getApplicationDropDownList,
            Func<Progress<ProgressMessage>, CancellationTokenSource, IProgressForm> getProgressForm,
            Func<SchemaName, ITreeViewXmlDocumentHelper> getTreeViewXmlDocumentHelper,
            Func<IApplicationForm, ITypeAutoCompleteTextControl, ITypeAutoCompleteManager> getTypeAutoCompleteManager,
            Func<IApplicationForm, IUpdateGenericArguments> getUpdateGenericArguments)
        {
            _getApplicationDropDownList = getApplicationDropDownList;
            _getProgressForm = getProgressForm;
            _getTreeViewXmlDocumentHelper = getTreeViewXmlDocumentHelper;
            _getTypeAutoCompleteManager = getTypeAutoCompleteManager;
            _getUpdateGenericArguments= getUpdateGenericArguments;
        }

        public IApplicationDropDownList GetApplicationDropDownList(IApplicationForm applicationForm)
            => _getApplicationDropDownList(applicationForm);

        public IProgressForm GetProgressForm(Progress<ProgressMessage> progress, CancellationTokenSource cancellationTokenSource)
            => _getProgressForm(progress, cancellationTokenSource);

        public ITreeViewXmlDocumentHelper GetTreeViewXmlDocumentHelper(SchemaName schema)
            => _getTreeViewXmlDocumentHelper(schema);

        public ITypeAutoCompleteManager GetTypeAutoCompleteManager(IApplicationForm applicationForm, ITypeAutoCompleteTextControl textControl)
            => _getTypeAutoCompleteManager(applicationForm, textControl);

        public IUpdateGenericArguments GetUpdateGenericArguments(IApplicationForm applicationForm)
            => _getUpdateGenericArguments(applicationForm);
    }
}
