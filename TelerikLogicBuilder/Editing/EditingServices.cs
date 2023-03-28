using ABIS.LogicBuilder.FlowBuilder.Editing;
using ABIS.LogicBuilder.FlowBuilder.Editing.DataGraph;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditObjectList.Helpers;
using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Helpers;
using ABIS.LogicBuilder.FlowBuilder.Editing.FindAndReplace;
using ABIS.LogicBuilder.FlowBuilder.Editing.Forms;
using ABIS.LogicBuilder.FlowBuilder.Editing.Helpers;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class EditingServices
    {
        internal static IServiceCollection AddEditingControls(this IServiceCollection services)
        {
            return services
                .AddSingleton<IBinaryFunctionTableLayoutPanelHelper, BinaryFunctionTableLayoutPanelHelper>()
                .AddSingleton<ICreateLiteralParameterXmlElement, CreateLiteralParameterXmlElement>()
                .AddSingleton<IDataGraphTreeViewHelper, DataGraphTreeViewHelper>()
                .AddSingleton<IEditFormFieldSetHelper, EditFormFieldSetHelper>()
                .AddSingleton<IFindAndReplaceHelper, FindAndReplaceHelper>()
                .AddTransient<IFindCell, FindCell>()
                .AddTransient<IFindConstructorInCell, FindConstructorInCell>()
                .AddTransient<IFindConstructorInFiles, FindConstructorInFiles>()
                .AddTransient<IFindConstructorInShape, FindConstructorInShape>()
                .AddTransient<IFindFunctionInCell, FindFunctionInCell>()
                .AddTransient<IFindFunctionInFiles, FindFunctionInFiles>()
                .AddTransient<IFindFunctionInShape, FindFunctionInShape>()
                .AddTransient<IFindShape, FindShape>()
                .AddTransient<IFindTextInCell, FindTextInCell>()
                .AddTransient<IFindTextInFiles, FindTextInFiles>()
                .AddTransient<IFindTextInShape, FindTextInShape>()
                .AddTransient<IFindReplaceConstructorInCell, FindReplaceConstructorInCell>()
                .AddTransient<IFindReplaceConstructorInShape, FindReplaceConstructorInShape>()
                .AddTransient<IFindReplaceFunctionInCell, FindReplaceFunctionInCell>()
                .AddTransient<IFindReplaceFunctionInShape, FindReplaceFunctionInShape>()
                .AddTransient<IFindReplaceTextInCell, FindReplaceTextInCell>()
                .AddTransient<IFindReplaceTextInShape, FindReplaceTextInShape>()
                .AddTransient<IFindReplaceVariableInCell, FindReplaceVariableInCell>()
                .AddTransient<IFindReplaceVariableInShape, FindReplaceVariableInShape>()
                .AddTransient<IFindVariableInCell, FindVariableInCell>()
                .AddTransient<IFindVariableInFiles, FindVariableInFiles>()
                .AddTransient<IFindVariableInShape, FindVariableInShape>()
                .AddSingleton<IFunctionParameterControlSetValidator, FunctionParameterControlSetValidator>()
                .AddSingleton<IGetObjectRichTextBoxVisibleText, GetObjectRichTextBoxVisibleText>()
                .AddSingleton<IGetSourceFilesForDocumentSearch, GetSourceFilesForDocumentSearch>()
                .AddSingleton<ILayoutFieldControlButtons, LayoutFieldControlButtons>()
                .AddSingleton<IObjectHashSetListBoxItemComparer , ObjectHashSetListBoxItemComparer>()
                .AddSingleton<ISearchFunctions, SearchFunctions>()
                .AddSingleton<ISearchSelectedDocuments, SearchSelectedDocuments>()
                .AddSingleton<ITableLayoutPanelHelper, TableLayoutPanelHelper>()
                .AddSingleton<IUpdateParameterControlValues, UpdateParameterControlValues>()
                .AddSingleton<IUpdateRichInputBoxXml, UpdateRichInputBoxXml>()
                .AddSingleton<IXmlDataHelper, XmlDataHelper>()
                .AddEditingControlFactories()
                .AddEditingControlHelperFactories()
                .AddEditingFormCommandFactories()
                .AddEditingFormFactories()
                .AddEditingFormHelperFactories()
                .AddEditLiteralListCommandFactories()
                .AddEditObjectListCommandFactories()
                .AddDocumentEditorFactories()
                .AddFieldControlCommandFactories()
                .AddFieldControlFactories()
                .AddFieldControlHelperFactories()
                .AddLiteralListBoxItemFactories()
                .AddLiteralListItemControlHelperFactories()
                .AddLiteralListItemEditorControlFactories()
                .AddObjectListBoxItemFactories()
                .AddObjectListItemEditorControlFactories()
                .AddSearcherFactories()
                .AddSelectConstructorViewControlFactories()
                .AddSelectEditingControlFactories()
                .AddSelectFunctionViewControlFactories()
                .AddSelectVariableViewControlFactories();
        }
    }
}
