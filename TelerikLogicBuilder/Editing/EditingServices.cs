using ABIS.LogicBuilder.FlowBuilder.Editing;
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
                .AddSingleton<ICreateLiteralParameterXmlElement, CreateLiteralParameterXmlElement>()
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
                .AddSingleton<IFunctionsFormFieldSetHelper, FunctionsFormFieldSetHelper>()
                .AddSingleton<IGetObjectRichTextBoxVisibleText, GetObjectRichTextBoxVisibleText>()
                .AddSingleton<IGetSourceFilesForDocumentSearch, GetSourceFilesForDocumentSearch>()
                .AddSingleton<ILayoutFieldControlButtons, LayoutFieldControlButtons>()
                .AddSingleton<ISearchFunctions, SearchFunctions>()
                .AddSingleton<ISearchSelectedDocuments, SearchSelectedDocuments>()
                .AddSingleton<ITableLayoutPanelHelper, TableLayoutPanelHelper>()
                .AddSingleton<IUpdateParameterControlValues, UpdateParameterControlValues>()
                .AddSingleton<IUpdateRichInputBoxXml, UpdateRichInputBoxXml>()
                .AddSingleton<IXmlDataHelper, XmlDataHelper>()
                .AddEditingControlFactories()
                .AddEditingControlHelperFactories()
                .AddEditingFormFactories()
                .AddDocumentEditorFactories()
                .AddFieldControlCommandFactories()
                .AddFieldControlFactories()
                .AddFieldControlHelperFactories()
                .AddSearcherFactories()
                .AddSelectConstructorViewControlFactories()
                .AddSelectEditingControlFactories()
                .AddSelectFunctionViewControlFactories()
                .AddSelectVariableViewControlFactories();
        }
    }
}
