using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Editing.SelectConstructor;
using ABIS.LogicBuilder.FlowBuilder.Editing.SelectFromDomain;
using ABIS.LogicBuilder.FlowBuilder.Editing.SelectFunction;
using ABIS.LogicBuilder.FlowBuilder.Editing.SelectVariable;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;
using System;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.Factories
{
    internal class EditingFormFactory : IEditingFormFactory
    {
        private IDisposable? _scopedService;
        private readonly Func<Type, IEditConstructorForm> _getEditConstructorForm;
        private readonly Func<Type, ISelectConstructorForm> _getSelectConstructorForm;
        private readonly Func<IList<string>, string, ISelectFromDomainForm> _getSelectFromDomainForm;
        private readonly Func<Type, IDictionary<string, Function>, IList<TreeFolder>, ISelectFunctionForm> _getSelectFunctionForm;
        private readonly Func<Type, IEditVariableForm> _getEditVariableForm;

        public EditingFormFactory(
            Func<Type, IEditConstructorForm> getEditConstructorForm,
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

        public IEditConstructorForm GetEditConstructorForm(Type assignedTo)
        {
            _scopedService = _getEditConstructorForm(assignedTo);
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
