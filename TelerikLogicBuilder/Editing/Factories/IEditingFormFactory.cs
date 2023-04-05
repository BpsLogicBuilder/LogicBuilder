using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditBooleanFunction;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditConstructor;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditDialogFunction;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditLiteralList;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditObjectList;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditValueFunction;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditVariable;
using ABIS.LogicBuilder.FlowBuilder.Editing.SelectConstructor;
using ABIS.LogicBuilder.FlowBuilder.Editing.SelectFragment;
using ABIS.LogicBuilder.FlowBuilder.Editing.SelectFromDomain;
using ABIS.LogicBuilder.FlowBuilder.Editing.SelectFunction;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;
using System;
using System.Collections.Generic;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.Factories
{
    internal interface IEditingFormFactory : IDisposable
    {
        IEditBooleanFunctionForm GetEditBooleanFunctionForm(XmlDocument? functionXmlDocument);
        IEditConstructorForm GetEditConstructorForm(Type assignedTo, XmlDocument constructorXmlDocument, HashSet<string> constructorNames, string selectedConstructor);
        IEditDialogFunctionForm GetEditDialogFunctionForm(XmlDocument? functionsXmlDocument);
        IEditLiteralListForm GetEditLiteralListForm(Type assignedTo, LiteralListParameterElementInfo literalListInfo, XmlDocument literalListXmlDocument);
        IEditObjectListForm GetEditObjectListForm(Type assignedTo, ObjectListParameterElementInfo objectListInfo, XmlDocument literalListXmlDocument);
        IEditValueFunctionForm GetEditValueFunctionForm(Type assignedTo, XmlDocument? functionXmlDocument);
        IEditVariableForm GetEditVariableForm(Type assignedTo);
        ISelectConstructorForm GetSelectConstructorForm(Type assignedTo);
        ISelectFragmentForm GetSelectFragmentForm();
        ISelectFromDomainForm GetSelectFromDomainForm(IList<string> domain, string comments);
        ISelectFunctionForm GetSelectFunctionForm(Type assignedTo, IDictionary<string, Function> functionDisctionary, IList<TreeFolder> treeFolders);
    }
}
