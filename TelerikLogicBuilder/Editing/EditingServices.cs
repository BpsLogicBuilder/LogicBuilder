using ABIS.LogicBuilder.FlowBuilder.Editing;
using ABIS.LogicBuilder.FlowBuilder.Editing.FindAndReplace;
using ABIS.LogicBuilder.FlowBuilder.Editing.Forms;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class EditingServices
    {
        internal static IServiceCollection AddEditingControls(this IServiceCollection services)
        {
            return services
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
                .AddSingleton<IGetSourceFilesForDocumentSearch, GetSourceFilesForDocumentSearch>()
                .AddSingleton<ISearchFunctions, SearchFunctions>()
                .AddSingleton<ISearchSelectedDocuments, SearchSelectedDocuments>()
                .AddEditingFormFactories()
                .AddDocumentEditorFactories()
                .AddSearcherFactories()
                .AddSelectConstructorViewControlFactories()
                .AddSelectEditingControlFactories()
                .AddSelectVariableViewControlFactories();
        }
    }
}
