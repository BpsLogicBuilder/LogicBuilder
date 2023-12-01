using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditBinaryFunction;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditConditionFunction;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditConditionFunction.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditConstructor;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditLiteralList;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditLiteralList.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditObjectList;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditObjectList.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditSetValueFunction;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditSetValueFunction.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditSetValueToNullFunction;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditStandardFunction;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditVariable;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditVariable.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Helpers;
using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.ParameterControls.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.ParameterControls.LiteralListItemControls.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.ParameterControls.ObjectListItemControls.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.VariableControls.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.VariableControls.LiteralListItemControls.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.VariableControls.ObjectListItemControls.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.Helpers;
using ABIS.LogicBuilder.FlowBuilder.Factories;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Data;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.Factories
{
    internal class EditingControlFactory : IEditingControlFactory
    {
        public IEditBinaryFunctionControl GetEditBinaryFunctionControl(IDataGraphEditingHost dataGraphEditingHost, Function function, Type assignedTo, XmlDocument formDocument, string treeNodeXPath, string? selectedParameter = null)
            => new EditBinaryFunctionControl
            (
                Program.ServiceProvider.GetRequiredService<IBinaryFunctionTableLayoutPanelHelper>(),
                Program.ServiceProvider.GetRequiredService<IConfigurationService>(),
                Program.ServiceProvider.GetRequiredService<IExceptionHelper>(),
                Program.ServiceProvider.GetRequiredService<IFunctionDataParser>(),
                Program.ServiceProvider.GetRequiredService<IFunctionElementValidator>(),
                Program.ServiceProvider.GetRequiredService<IFunctionHelper>(),
                Program.ServiceProvider.GetRequiredService<IFunctionParameterControlSetValidator>(),
                Program.ServiceProvider.GetRequiredService<IEditingControlHelperFactory>(),
                Program.ServiceProvider.GetRequiredService<IGenericFunctionHelper>(),
                Program.ServiceProvider.GetRequiredService<IParameterFieldControlFactory>(),
                Program.ServiceProvider.GetRequiredService<IRadCheckBoxHelper>(),
                Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                dataGraphEditingHost,
                function,
                assignedTo,
                formDocument,
                treeNodeXPath,
                selectedParameter
            );

        public IEditConditionFunctionControl GetEditConditionFunctionControl(IApplicationForm parentForm)
            => new EditConditionFunctionControl
            (
                Program.ServiceProvider.GetRequiredService<IConfigurationService>(),
                Program.ServiceProvider.GetRequiredService<IEditConditionFunctionCommandFactory>(),
                Program.ServiceProvider.GetRequiredService<IEditingFormHelperFactory>(),
                Program.ServiceProvider.GetRequiredService<IFunctionDataParser>(),
                Program.ServiceProvider.GetRequiredService<IRadDropDownListHelper>(),
                Program.ServiceProvider.GetRequiredService<IRefreshVisibleTextHelper>(),
                Program.ServiceProvider.GetRequiredService<IServiceFactory>(),
                Program.ServiceProvider.GetRequiredService<IXmlDataHelper>(),
                Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                parentForm
            );

        public IEditConstructorControl GetEditConstructorControl(IDataGraphEditingHost dataGraphEditingHost, Constructor constructor, Type assignedTo, XmlDocument formDocument, string treeNodeXPath, string? selectedParameter = null)
            => new EditConstructorControl
            (
                Program.ServiceProvider.GetRequiredService<IConfigurationService>(),
                Program.ServiceProvider.GetRequiredService<IConstructorDataParser>(),
                Program.ServiceProvider.GetRequiredService<IConstructorElementValidator>(),
                Program.ServiceProvider.GetRequiredService<IConstructorGenericsConfigrationValidator>(),
                Program.ServiceProvider.GetRequiredService<IEditingControlHelperFactory>(),
                Program.ServiceProvider.GetRequiredService<IExceptionHelper>(),
                Program.ServiceProvider.GetRequiredService<IGenericConstructorHelper>(),
                Program.ServiceProvider.GetRequiredService<IParameterElementValidator>(),
                Program.ServiceProvider.GetRequiredService<IParameterFieldControlFactory>(),
                Program.ServiceProvider.GetRequiredService<IRefreshVisibleTextHelper>(),
                Program.ServiceProvider.GetRequiredService<ITableLayoutPanelHelper>(),
                Program.ServiceProvider.GetRequiredService<ITypeLoadHelper>(),
                Program.ServiceProvider.GetRequiredService<IUpdateParameterControlValues>(),
                Program.ServiceProvider.GetRequiredService<IXmlDataHelper>(),
                Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                dataGraphEditingHost,
                constructor,
                assignedTo,
                formDocument,
                treeNodeXPath,
                selectedParameter
            );

        public IEditParameterLiteralListControl GetEditParameterLiteralListControl(IDataGraphEditingHost dataGraphEditingHost, LiteralListParameterElementInfo literalListElementInfo, Type assignedTo, XmlDocument formDocument, string treeNodeXPath, int? selectedIndex)
            => new EditParameterLiteralListControl
            (
                Program.ServiceProvider.GetRequiredService<IEditLiteralListCommandFactory>(),
                Program.ServiceProvider.GetRequiredService<IEnumHelper>(),
                Program.ServiceProvider.GetRequiredService<IExceptionHelper>(),
                Program.ServiceProvider.GetRequiredService<ILiteralListBoxItemFactory>(),
                Program.ServiceProvider.GetRequiredService<ILiteralListDataParser>(),
                Program.ServiceProvider.GetRequiredService<IParameterLiteralListItemEditorControlFactory>(),
                Program.ServiceProvider.GetRequiredService<IRadDropDownListHelper>(),
                Program.ServiceProvider.GetRequiredService<IServiceFactory>(),
                Program.ServiceProvider.GetRequiredService<ITypeHelper>(),
                Program.ServiceProvider.GetRequiredService<IXmlDataHelper>(),
                Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                dataGraphEditingHost,
                literalListElementInfo,
                assignedTo,
                formDocument,
                treeNodeXPath,
                selectedIndex
            );

        public IEditParameterObjectListControl GetEditParameterObjectListControl(IDataGraphEditingHost dataGraphEditingHost, ObjectListParameterElementInfo objectListElementInfo, Type assignedTo, XmlDocument formDocument, string treeNodeXPath, int? selectedIndex)
            => new EditParameterObjectListControl
            (
                Program.ServiceProvider.GetRequiredService<IEditObjectListCommandFactory>(),
                Program.ServiceProvider.GetRequiredService<IEnumHelper>(),
                Program.ServiceProvider.GetRequiredService<IExceptionHelper>(),
                Program.ServiceProvider.GetRequiredService<IGetObjectRichTextBoxVisibleText>(),
                Program.ServiceProvider.GetRequiredService<IObjectListBoxItemFactory>(),
                Program.ServiceProvider.GetRequiredService<IObjectListDataParser>(),
                Program.ServiceProvider.GetRequiredService<IParameterObjectListItemEditorControlFactory>(),
                Program.ServiceProvider.GetRequiredService<IRadDropDownListHelper>(),
                Program.ServiceProvider.GetRequiredService<IRefreshVisibleTextHelper>(),
                Program.ServiceProvider.GetRequiredService<IServiceFactory>(),
                Program.ServiceProvider.GetRequiredService<ITypeHelper>(),
                Program.ServiceProvider.GetRequiredService<ITypeLoadHelper>(),
                Program.ServiceProvider.GetRequiredService<IXmlDataHelper>(),
                Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                dataGraphEditingHost,
                objectListElementInfo,
                assignedTo,
                formDocument,
                treeNodeXPath,
                selectedIndex
            );

        public IEditSetValueFunctionControl GetEditSetValueFunctionControl(IDataGraphEditingHost dataGraphEditingHost, Function function, XmlDocument formDocument, string treeNodeXPath)
            => new EditSetValueFunctionControl
            (
                Program.ServiceProvider.GetRequiredService<IAssertFunctionDataParser>(),
                Program.ServiceProvider.GetRequiredService<IAssertFunctionElementValidator>(),
                Program.ServiceProvider.GetRequiredService<IConfigurationService>(),
                Program.ServiceProvider.GetRequiredService<IEditSetValueFunctionCommandFactory>(),
                Program.ServiceProvider.GetRequiredService<IExceptionHelper>(),
                Program.ServiceProvider.GetRequiredService<IRadDropDownListHelper>(),
                Program.ServiceProvider.GetRequiredService<IRefreshVisibleTextHelper>(),
                Program.ServiceProvider.GetRequiredService<ISetValueFunctionTableLayoutPanelHelper>(),
                Program.ServiceProvider.GetRequiredService<IServiceFactory>(),
                Program.ServiceProvider.GetRequiredService<IVariableValueControlFactory>(),
                Program.ServiceProvider.GetRequiredService<IVariableValueDataParser>(),
                Program.ServiceProvider.GetRequiredService<IXmlDataHelper>(),
                Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                dataGraphEditingHost,
                function,
                formDocument,
                treeNodeXPath
            );

        public IEditSetValueToNullFunctionControl GetEditSetValueToNullFunctionControl(IDataGraphEditingHost dataGraphEditingHost, Function function, XmlDocument formDocument, string treeNodeXPath)
            => new EditSetValueToNullFunctionControl
            (
                Program.ServiceProvider.GetRequiredService<IConfigurationService>(),
                Program.ServiceProvider.GetRequiredService<IEditSetValueFunctionCommandFactory>(),
                Program.ServiceProvider.GetRequiredService<IExceptionHelper>(),
                Program.ServiceProvider.GetRequiredService<IRadDropDownListHelper>(),
                Program.ServiceProvider.GetRequiredService<IRefreshVisibleTextHelper>(),
                Program.ServiceProvider.GetRequiredService<IRetractFunctionDataParser>(),
                Program.ServiceProvider.GetRequiredService<IRetractFunctionElementValidator>(),
                Program.ServiceProvider.GetRequiredService<ISetValueFunctionTableLayoutPanelHelper>(),
                Program.ServiceProvider.GetRequiredService<ITypeLoadHelper>(),
                Program.ServiceProvider.GetRequiredService<IXmlDataHelper>(),
                Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                dataGraphEditingHost,
                function,
                formDocument,
                treeNodeXPath
            );

        public IEditStandardFunctionControl GetEditStandardFunctionControl(IDataGraphEditingHost dataGraphEditingHost, Function function, Type assignedTo, XmlDocument formDocument, string treeNodeXPath, string? selectedParameter = null)
            => new EditStandardFunctionControl
            (
                Program.ServiceProvider.GetRequiredService<IConfigurationService>(),
                Program.ServiceProvider.GetRequiredService<IExceptionHelper>(),
                Program.ServiceProvider.GetRequiredService<IFunctionDataParser>(),
                Program.ServiceProvider.GetRequiredService<IFunctionElementValidator>(),
                Program.ServiceProvider.GetRequiredService<IFunctionHelper>(),
                Program.ServiceProvider.GetRequiredService<IFunctionParameterControlSetValidator>(),
                Program.ServiceProvider.GetRequiredService<IEditingControlHelperFactory>(),
                Program.ServiceProvider.GetRequiredService<IGenericFunctionHelper>(),
                Program.ServiceProvider.GetRequiredService<IParameterFieldControlFactory>(),
                Program.ServiceProvider.GetRequiredService<IRadCheckBoxHelper>(),
                Program.ServiceProvider.GetRequiredService<ITableLayoutPanelHelper>(),
                Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                dataGraphEditingHost,
                function,
                assignedTo,
                formDocument,
                treeNodeXPath,
                selectedParameter
            );

        public IEditVariableControl GetEditVariableControl(IEditVariableHost editVariableHost, Type assignedTo)
            => new EditVariableControl
            (
                Program.ServiceProvider.GetRequiredService<IConfigurationService>(),
                Program.ServiceProvider.GetRequiredService<IExceptionHelper>(),
                Program.ServiceProvider.GetRequiredService<IImageListService>(),
                Program.ServiceProvider.GetRequiredService<IEditVariableViewControlFactory>(),
                Program.ServiceProvider.GetRequiredService<ITypeHelper>(),
                Program.ServiceProvider.GetRequiredService<ITypeLoadHelper>(),
                Program.ServiceProvider.GetRequiredService<IXmlDataHelper>(),
                Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                editVariableHost,
                assignedTo
            );

        public IEditVariableLiteralListControl GetEditVariableLiteralListControl(IDataGraphEditingHost dataGraphEditingHost, LiteralListVariableElementInfo literalListElementInfo, Type assignedTo, XmlDocument formDocument, string treeNodeXPath, int? selectedIndex)
            => new EditVariableLiteralListControl
            (
                Program.ServiceProvider.GetRequiredService<IEditLiteralListCommandFactory>(),
                Program.ServiceProvider.GetRequiredService<IEnumHelper>(),
                Program.ServiceProvider.GetRequiredService<IExceptionHelper>(),
                Program.ServiceProvider.GetRequiredService<ILiteralListBoxItemFactory>(),
                Program.ServiceProvider.GetRequiredService<ILiteralListDataParser>(),
                Program.ServiceProvider.GetRequiredService<IVariableLiteralListItemEditorControlFactory>(),
                Program.ServiceProvider.GetRequiredService<IRadDropDownListHelper>(),
                Program.ServiceProvider.GetRequiredService<IServiceFactory>(),
                Program.ServiceProvider.GetRequiredService<ITypeHelper>(),
                Program.ServiceProvider.GetRequiredService<IXmlDataHelper>(),
                Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                dataGraphEditingHost,
                literalListElementInfo,
                assignedTo,
                formDocument,
                treeNodeXPath,
                selectedIndex
            );

        public IEditVariableObjectListControl GetEditVariableObjectListControl(IDataGraphEditingHost dataGraphEditingHost, ObjectListVariableElementInfo objectListElementInfo, Type assignedTo, XmlDocument formDocument, string treeNodeXPath, int? selectedIndex)
            => new EditVariableObjectListControl
            (
                Program.ServiceProvider.GetRequiredService<IEditObjectListCommandFactory>(),
                Program.ServiceProvider.GetRequiredService<IEnumHelper>(),
                Program.ServiceProvider.GetRequiredService<IExceptionHelper>(),
                Program.ServiceProvider.GetRequiredService<IGetObjectRichTextBoxVisibleText>(),
                Program.ServiceProvider.GetRequiredService<IObjectListBoxItemFactory>(),
                Program.ServiceProvider.GetRequiredService<IObjectListDataParser>(),
                Program.ServiceProvider.GetRequiredService<IVariableObjectListItemEditorControlFactory>(),
                Program.ServiceProvider.GetRequiredService<IRadDropDownListHelper>(),
                Program.ServiceProvider.GetRequiredService<IRefreshVisibleTextHelper>(),
                Program.ServiceProvider.GetRequiredService<IServiceFactory>(),
                Program.ServiceProvider.GetRequiredService<ITypeHelper>(),
                Program.ServiceProvider.GetRequiredService<ITypeLoadHelper>(),
                Program.ServiceProvider.GetRequiredService<IXmlDataHelper>(),
                Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                dataGraphEditingHost,
                objectListElementInfo,
                assignedTo,
                formDocument,
                treeNodeXPath,
                selectedIndex
            );
    }
}
