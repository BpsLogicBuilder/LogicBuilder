using ABIS.LogicBuilder.FlowBuilder.Enums;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.Variables.Factories
{
    internal interface IVariableFactory
    {
        ListOfLiteralsVariable GetListOfLiteralsVariable(string name,
                    string memberName,
                    VariableCategory variableCategory,
                    string castVariableAs,
                    string typeName,
                    string referenceName,
                    string referenceDefinition,
                    string castReferenceAs,
                    ReferenceCategories referenceCategory,
                    string comments,
                    LiteralVariableType literalType,
                    ListType listType,
                    ListVariableInputStyle control,
                    LiteralVariableInputStyle elementControl,
                    string propertySource,
                    List<string> defaultValue,
                    List<string> domain);

        ListOfObjectsVariable GetListOfObjectsVariable(string name,
                    string memberName,
                    VariableCategory variableCategory,
                    string castVariableAs,
                    string typeName,
                    string referenceName,
                    string referenceDefinition,
                    string castReferenceAs,
                    ReferenceCategories referenceCategory,
                    string comments,
                    string objectType,
                    ListType listType,
                    ListVariableInputStyle control);

        LiteralVariable GetLiteralVariable(string name,
                    string memberName,
                    VariableCategory variableCategory,
                    string castVariableAs,
                    string typeName,
                    string referenceName,
                    string referenceDefinition,
                    string castReferenceAs,
                    ReferenceCategories referenceCategory,
                    string comments,
                    LiteralVariableType literalType,
                    LiteralVariableInputStyle control,
                    string propertySource,
                    string defaultValue,
                    List<string> domain);

        ObjectVariable GetObjectVariable(string name,
                    string memberName,
                    VariableCategory variableCategory,
                    string castVariableAs,
                    string typeName,
                    string referenceName,
                    string referenceDefinition,
                    string castReferenceAs,
                    ReferenceCategories referenceCategory,
                    string comments,
                    string objectType);
    }
}
