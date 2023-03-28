using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditConstructor;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditVariable;
using ABIS.LogicBuilder.FlowBuilder.Editing.SelectConstructor;
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
        private readonly Func<Type, XmlDocument, HashSet<string>, string, IEditConstructorForm> _getEditConstructorForm;
        private readonly Func<Type, ISelectConstructorForm> _getSelectConstructorForm;
        private readonly Func<IList<string>, string, ISelectFromDomainForm> _getSelectFromDomainForm;
        private readonly Func<Type, IDictionary<string, Function>, IList<TreeFolder>, ISelectFunctionForm> _getSelectFunctionForm;
        private readonly Func<Type, IEditVariableForm> _getEditVariableForm;

        public EditingFormFactory(
            Func<Type, XmlDocument, HashSet<string>, string, IEditConstructorForm> getEditConstructorForm,
            Func<Type, ISelectConstructorForm> getSelectConstructorForm,
            Func<IList<string>, string, ISelectFromDomainForm> getSelectFromDomainForm,
            Func<Type, IDictionary<string, Function>, IList<TreeFolder>, ISelectFunctionForm> getSelectFunctionForm,
            Func<Type, IEditVariableForm> getEditVariableForm)
        {
            _getEditConstructorForm = getEditConstructorForm;
            _getSelectConstructorForm = getSelectConstructorForm;
            _getSelectFromDomainForm = getSelectFromDomainForm;
            _getSelectFunctionForm = getSelectFunctionForm;
            _getEditVariableForm = getEditVariableForm;
        }

        public IEditConstructorForm GetEditConstructorForm(Type assignedTo, XmlDocument constructorXmlDocument, HashSet<string> constructorNames, string selectedConstructor)
        {
            _scopedService = _getEditConstructorForm(assignedTo, constructorXmlDocument, constructorNames, selectedConstructor);
            return (IEditConstructorForm)_scopedService;
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
