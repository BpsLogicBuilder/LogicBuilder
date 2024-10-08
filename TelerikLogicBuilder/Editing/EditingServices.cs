﻿using ABIS.LogicBuilder.FlowBuilder.Editing;
using ABIS.LogicBuilder.FlowBuilder.Editing.DataGraph;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditCell;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditDecision.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditObjectList.Helpers;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditShape;
using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Helpers;
using ABIS.LogicBuilder.FlowBuilder.Editing.FindAndReplace;
using ABIS.LogicBuilder.FlowBuilder.Editing.Forms;
using ABIS.LogicBuilder.FlowBuilder.Editing.Helpers;
using ABIS.LogicBuilder.FlowBuilder.Editing.IndexInformation;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class EditingServices
    {
        internal static IServiceCollection AddEditingControls(this IServiceCollection services)
        {
            return services
                .AddSingleton<IBinaryFunctionTableLayoutPanelHelper, BinaryFunctionTableLayoutPanelHelper>()
                .AddSingleton<ICreateLiteralParameterXmlElement, CreateLiteralParameterXmlElement>()
                .AddTransient<IDiagramEditor, DiagramEditor>()
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
                .AddTransient<IIndexInformationForm, IndexInformationForm>()
                .AddSingleton<ILayoutFieldControlButtons, LayoutFieldControlButtons>()
                .AddSingleton<IObjectHashSetListBoxItemComparer , ObjectHashSetListBoxItemComparer>()
                .AddSingleton<ISearchFunctions, SearchFunctions>()
                .AddSingleton<ISearchSelectedDocuments, SearchSelectedDocuments>()
                .AddSingleton<ISetValueFunctionTableLayoutPanelHelper,  SetValueFunctionTableLayoutPanelHelper>()
                .AddTransient<ITableEditor, TableEditor>()
                .AddSingleton<ITableLayoutPanelHelper, TableLayoutPanelHelper>()
                .AddSingleton<IUpdateParameterControlValues, UpdateParameterControlValues>()
                .AddSingleton<IUpdateRichInputBoxXml, UpdateRichInputBoxXml>()
                .AddSingleton<IXmlDataHelper, XmlDataHelper>()
                .AddCellEditorFactories()
                .AddConditionFunctionListBoxItemFactories()
                .AddDecisionFunctionListBoxItemFactories()
                .AddDecisionListBoxItemFactories()
                .AddEditDecisionFormCommandFactories()
                .AddEditDecisionsFormCommandFactories()
                .AddEditBooleanFunctionCommandFactories()
                .AddEditConditionFunctionCommandFactories()
                .AddEditConditionFunctionsFormCommandFactories()
                .AddEditConstructorCommandFactories()
                .AddEditDialogConnectorControlFactories()
                .AddEditDialogConnectorFieldControlFactories()
                .AddEditDialogFunctionCommandFactories()
                .AddEditFunctionsCommandFactories()
                .AddEditFunctionsControlFactories()
                .AddEditingControlFactories()
                .AddEditingControlHelperFactories()
                .AddEditingFormCommandFactories()
                .AddEditingFormFactories()
                .AddEditingFormHelperFactories()
                .AddEditLiteralListCommandFactories()
                .AddEditObjectListCommandFactories()
                .AddEditSetValueFunctionCommandFactories()
                .AddEditValueFunctionCommandFactories()
                .AddEditVoidFunctionCommandFactories()
                .AddEditXmlFormFactories()
                .AddEditXmlHelperFactories()
                .AddDocumentEditorFactories()
                .AddFieldControlCommandFactories()
                .AddFieldControlHelperFactories()
                .AddFunctionListBoxItemFactories()
                .AddLiteralListBoxItemFactories()
                .AddObjectListBoxItemFactories()
                .AddParameterFieldControlFactories()
                .AddParameterLiteralListItemEditorControlFactories()
                .AddParameterObjectListItemEditorControlFactories()
                .AddSearcherFactories()
                .AddSelectConstructorViewControlFactories()
                .AddSelectEditingControlFactories()
                .AddSelectFragmentViewControlFactories()
                .AddSelectFunctionViewControlFactories()
                .AddSelectVariableViewControlFactories()
                .AddShapeEditorFactories()
                .AddVariableLiteralListItemEditorControlFactories()
                .AddVariableObjectListItemEditorControlFactories()
                .AddVariableValueControlFactories();
        }
    }
}
