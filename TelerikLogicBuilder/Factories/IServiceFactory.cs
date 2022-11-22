using ABIS.LogicBuilder.FlowBuilder.Components.Helpers;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using ABIS.LogicBuilder.FlowBuilder.UserControls;

namespace ABIS.LogicBuilder.FlowBuilder.Factories
{
    internal interface IServiceFactory
    {
        IApplicationDropDownList GetApplicationDropDownList(IApplicationForm applicationForm);
        ITreeViewXmlDocumentHelper GetTreeViewXmlDocumentHelper(SchemaName schema);
        ITypeAutoCompleteManager GetTypeAutoCompleteManager(IApplicationForm applicationForm,
            ITypeAutoCompleteTextControl textControl);
        IUpdateGenericArguments GetUpdateGenericArguments(IApplicationForm applicationForm);
    }
}
