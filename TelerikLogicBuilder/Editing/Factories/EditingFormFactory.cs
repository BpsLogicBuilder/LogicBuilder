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
    internal class EditingFormFactory : IEditingFormFactory
    {
        private IDisposable? _scopedService;
        private readonly Func<XmlDocument?, IEditBooleanFunctionForm> _getEditBooleanFunctionForm;
        private readonly Func<Type, XmlDocument, HashSet<string>, string, IEditConstructorForm> _getEditConstructorForm;
        private readonly Func<XmlDocument?, IEditDialogFunctionForm> _getEditDialogFunctionForm;
        private readonly Func<Type, LiteralListParameterElementInfo, XmlDocument, IEditLiteralListForm> _getEditLiteralListForm;
        private readonly Func<Type, ObjectListParameterElementInfo, XmlDocument, IEditObjectListForm> _getEditObjectListForm;
        private readonly Func<Type, XmlDocument?, IEditValueFunctionForm> _getEditValueFunctionForm;
        private readonly Func<Type, IEditVariableForm> _getEditVariableForm;
        private readonly Func<Type, ISelectConstructorForm> _getSelectConstructorForm;
        private readonly Func<ISelectFragmentForm> _getSelectFragmentForm;
        private readonly Func<IList<string>, string, ISelectFromDomainForm> _getSelectFromDomainForm;
        private readonly Func<Type, IDictionary<string, Function>, IList<TreeFolder>, ISelectFunctionForm> _getSelectFunctionForm;

        public EditingFormFactory(
            Func<XmlDocument?, IEditBooleanFunctionForm> getEditBooleanFunctionForm,
            Func<Type, XmlDocument, HashSet<string>, string, IEditConstructorForm> getEditConstructorForm,
            Func<XmlDocument?, IEditDialogFunctionForm> getEditDialogFunctionForm,
            Func<Type, LiteralListParameterElementInfo, XmlDocument, IEditLiteralListForm> getEditLiteralListForm,
            Func<Type, ObjectListParameterElementInfo, XmlDocument, IEditObjectListForm> getEditObjectListForm,
            Func<Type, XmlDocument?, IEditValueFunctionForm> getEditValueFunctionForm,
            Func<Type, IEditVariableForm> getEditVariableForm,
            Func<Type, ISelectConstructorForm> getSelectConstructorForm,
            Func<ISelectFragmentForm> getSelectFragmentForm,
            Func<IList<string>, string, ISelectFromDomainForm> getSelectFromDomainForm,
            Func<Type, IDictionary<string, Function>, IList<TreeFolder>, ISelectFunctionForm> getSelectFunctionForm)
        {
            _getEditBooleanFunctionForm = getEditBooleanFunctionForm;
            _getEditConstructorForm = getEditConstructorForm;
            _getEditDialogFunctionForm = getEditDialogFunctionForm;
            _getEditLiteralListForm = getEditLiteralListForm;
            _getEditObjectListForm = getEditObjectListForm;
            _getEditValueFunctionForm = getEditValueFunctionForm;
            _getEditVariableForm = getEditVariableForm;
            _getSelectConstructorForm = getSelectConstructorForm;
            _getSelectFragmentForm = getSelectFragmentForm;
            _getSelectFromDomainForm = getSelectFromDomainForm;
            _getSelectFunctionForm = getSelectFunctionForm;
        }

        public IEditBooleanFunctionForm GetEditBooleanFunctionForm(XmlDocument? functionXmlDocument)
        {
            _scopedService = _getEditBooleanFunctionForm(functionXmlDocument);
            return (IEditBooleanFunctionForm)_scopedService;
        }

        public IEditConstructorForm GetEditConstructorForm(Type assignedTo, XmlDocument constructorXmlDocument, HashSet<string> constructorNames, string selectedConstructor)
        {
            _scopedService = _getEditConstructorForm(assignedTo, constructorXmlDocument, constructorNames, selectedConstructor);
            return (IEditConstructorForm)_scopedService;
        }

        public IEditDialogFunctionForm GetEditDialogFunctionForm(XmlDocument? functionsXmlDocument)
        {
            _scopedService = _getEditDialogFunctionForm(functionsXmlDocument);
            return (IEditDialogFunctionForm)_scopedService;
        }

        public IEditLiteralListForm GetEditLiteralListForm(Type assignedTo, LiteralListParameterElementInfo literalListInfo, XmlDocument literalListXmlDocument)
        {
            _scopedService = _getEditLiteralListForm(assignedTo, literalListInfo, literalListXmlDocument);
            return (IEditLiteralListForm)_scopedService;
        }

        public IEditObjectListForm GetEditObjectListForm(Type assignedTo, ObjectListParameterElementInfo objectListInfo, XmlDocument literalListXmlDocument)
        {
            _scopedService = _getEditObjectListForm(assignedTo, objectListInfo, literalListXmlDocument);
            return (IEditObjectListForm)_scopedService;
        }

        public IEditValueFunctionForm GetEditValueFunctionForm(Type assignedTo, XmlDocument? functionXmlDocument)
        {
            _scopedService = _getEditValueFunctionForm(assignedTo, functionXmlDocument);
            return (IEditValueFunctionForm)_scopedService;
        }

        public IEditVariableForm GetEditVariableForm(Type assignedTo)
        {
            _scopedService = _getEditVariableForm(assignedTo);
            return (IEditVariableForm)_scopedService;
        }

        public ISelectConstructorForm GetSelectConstructorForm(Type assignedTo)
        {
            _scopedService = _getSelectConstructorForm(assignedTo);
            return (ISelectConstructorForm)_scopedService;
        }

        public ISelectFragmentForm GetSelectFragmentForm()
        {
            _scopedService = _getSelectFragmentForm();
            return (ISelectFragmentForm)_scopedService;
        }

        public ISelectFromDomainForm GetSelectFromDomainForm(IList<string> domain, string comments)
        {
            _scopedService = _getSelectFromDomainForm(domain, comments);
            return (ISelectFromDomainForm)_scopedService;
        }

        public ISelectFunctionForm GetSelectFunctionForm(Type assignedTo, IDictionary<string, Function> functionDisctionary, IList<TreeFolder> treeFolders)
        {
            _scopedService = _getSelectFunctionForm(assignedTo, functionDisctionary, treeFolders);
            return (ISelectFunctionForm)_scopedService;
        }

        public void Dispose()
        {
            _scopedService?.Dispose();
        }
    }
}
