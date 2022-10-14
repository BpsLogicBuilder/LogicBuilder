using ABIS.LogicBuilder.FlowBuilder.Enums;
using System;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.Variables.Factories
{
    internal class VariableFactory : IVariableFactory
    {
        private readonly Func<string, string, VariableCategory, string, string, string, string, string, ReferenceCategories, string, LiteralVariableType, ListType, ListVariableInputStyle, LiteralVariableInputStyle, string, List<string>, List<string>, ListOfLiteralsVariable> _getListOfLiteralsVariable;
        private readonly Func<string, string, VariableCategory, string, string, string, string, string, ReferenceCategories, string, string, ListType, ListVariableInputStyle, ListOfObjectsVariable> _getListOfObjectsVariable;
        private readonly Func<string, string, VariableCategory, string, string, string, string, string, ReferenceCategories, string, LiteralVariableType, LiteralVariableInputStyle, string, string, List<string>, LiteralVariable> _getLiteralVariable;
        private readonly Func<string, string, VariableCategory, string, string, string, string, string, ReferenceCategories, string, string, ObjectVariable> _getObjectVariable;

        public VariableFactory(
            Func<string, string, VariableCategory, string, string, string, string, string, ReferenceCategories, string, LiteralVariableType, ListType, ListVariableInputStyle, LiteralVariableInputStyle, string, List<string>, List<string>, ListOfLiteralsVariable> getListOfLiteralsVariable,
            Func<string, string, VariableCategory, string, string, string, string, string, ReferenceCategories, string, string, ListType, ListVariableInputStyle, ListOfObjectsVariable> getListOfObjectsVariable,
            Func<string, string, VariableCategory, string, string, string, string, string, ReferenceCategories, string, LiteralVariableType, LiteralVariableInputStyle, string, string, List<string>, LiteralVariable> getLiteralVariable,
            Func<string, string, VariableCategory, string, string, string, string, string, ReferenceCategories, string, string, ObjectVariable> getObjectVariable)
        {
            _getListOfLiteralsVariable = getListOfLiteralsVariable;
            _getListOfObjectsVariable = getListOfObjectsVariable;
            _getLiteralVariable = getLiteralVariable;
            _getObjectVariable = getObjectVariable;
        }

        public ListOfLiteralsVariable GetListOfLiteralsVariable(string name, string memberName, VariableCategory variableCategory, string castVariableAs, string typeName, string referenceName, string referenceDefinition, string castReferenceAs, ReferenceCategories referenceCategory, string comments, LiteralVariableType literalType, ListType listType, ListVariableInputStyle control, LiteralVariableInputStyle elementControl, string propertySource, List<string> defaultValue, List<string> domain)
            => _getListOfLiteralsVariable(name,
                        memberName,
                        variableCategory,
                        castVariableAs,
                        typeName,
                        referenceName,
                        referenceDefinition,
                        castReferenceAs,
                        referenceCategory,
                        comments,
                        literalType,
                        listType,
                        control,
                        elementControl,
                        propertySource,
                        defaultValue,
                        domain);

        public ListOfObjectsVariable GetListOfObjectsVariable(string name, string memberName, VariableCategory variableCategory, string castVariableAs, string typeName, string referenceName, string referenceDefinition, string castReferenceAs, ReferenceCategories referenceCategory, string comments, string objectType, ListType listType, ListVariableInputStyle control)
             => _getListOfObjectsVariable(name,
                        memberName,
                        variableCategory,
                        castVariableAs,
                        typeName,
                        referenceName,
                        referenceDefinition,
                        castReferenceAs,
                        referenceCategory,
                        comments,
                        objectType,
                        listType,
                        control);

        public LiteralVariable GetLiteralVariable(string name, string memberName, VariableCategory variableCategory, string castVariableAs, string typeName, string referenceName, string referenceDefinition, string castReferenceAs, ReferenceCategories referenceCategory, string comments, LiteralVariableType literalType, LiteralVariableInputStyle control, string propertySource, string defaultValue, List<string> domain)
            => _getLiteralVariable(name,
                        memberName,
                        variableCategory,
                        castVariableAs,
                        typeName,
                        referenceName,
                        referenceDefinition,
                        castReferenceAs,
                        referenceCategory,
                        comments,
                        literalType,
                        control,
                        propertySource,
                        defaultValue,
                        domain);

        public ObjectVariable GetObjectVariable(string name, string memberName, VariableCategory variableCategory, string castVariableAs, string typeName, string referenceName, string referenceDefinition, string castReferenceAs, ReferenceCategories referenceCategory, string comments, string objectType)
             => _getObjectVariable(name,
                        memberName,
                        variableCategory,
                        castVariableAs,
                        typeName,
                        referenceName,
                        referenceDefinition,
                        castReferenceAs,
                        referenceCategory,
                        comments,
                        objectType);
    }
}
