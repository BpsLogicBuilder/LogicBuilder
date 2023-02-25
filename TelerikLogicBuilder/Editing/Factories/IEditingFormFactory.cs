using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditVariable;
using ABIS.LogicBuilder.FlowBuilder.Editing.SelectConstructor;
using ABIS.LogicBuilder.FlowBuilder.Editing.SelectFromDomain;
using ABIS.LogicBuilder.FlowBuilder.Editing.SelectFunction;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;
using System;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.Factories
{
    internal interface IEditingFormFactory : IDisposable
    {
        IEditConstructorForm GetEditConstructorForm(Type assignedTo);
        IEditVariableForm GetEditVariableForm(Type assignedTo);
        ISelectConstructorForm GetSelectConstructorForm(Type assignedTo);
        ISelectFromDomainForm GetSelectFromDomainForm(IList<string> domain, string comments);
        ISelectFunctionForm GetSelectFunctionForm(Type assignedTo, IDictionary<string, Function> functionDisctionary, IList<TreeFolder> treeFolders);
    }
}
