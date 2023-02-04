using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration
{
    internal class FunctionList
    {
        public FunctionList(IDictionary<string, Function> functions,
                            IDictionary<string, Function> voidFunctions,
                            IDictionary<string, Function> booleanFunctions,
                            IDictionary<string, Function> valueFunctions,
                            IDictionary<string, Function> dialogFunctions,
                            IDictionary<string, Function> tableFunctions,
                            TreeFolder voidFunctionsTreeFolder,
                            TreeFolder booleanFunctionsTreeFolder,
                            TreeFolder valueFunctionsTreeFolder,
                            TreeFolder dialogFunctionsTreeFolder,
                            TreeFolder tableFunctionsTreeFolder,
                            TreeFolder builtInVoidFunctionsTreeFolder,
                            TreeFolder builtInBooleanFunctionsTreeFolder,
                            TreeFolder builtInValueFunctionsTreeFolder,
                            TreeFolder builtInDialogFunctionsTreeFolder,
                            TreeFolder builtInTableFunctionsTreeFolder)
        {
            Functions = functions;
            VoidFunctions = voidFunctions;
            BooleanFunctions = booleanFunctions;
            DialogFunctions = dialogFunctions;
            ValueFunctions = valueFunctions;
            TableFunctions = tableFunctions;
            VoidFunctionsTreeFolder = voidFunctionsTreeFolder;
            BooleanFunctionsTreeFolder = booleanFunctionsTreeFolder;
            ValueFunctionsTreeFolder = valueFunctionsTreeFolder;
            DialogFunctionsTreeFolder = dialogFunctionsTreeFolder;
            TableFunctionsTreeFolder = tableFunctionsTreeFolder;
            BuiltInVoidFunctionsTreeFolder = builtInVoidFunctionsTreeFolder;
            BuiltInBooleanFunctionsTreeFolder = builtInBooleanFunctionsTreeFolder;
            BuiltInValueFunctionsTreeFolder = builtInValueFunctionsTreeFolder;
            BuiltInDialogFunctionsTreeFolder = builtInDialogFunctionsTreeFolder;
            BuiltInTableFunctionsTreeFolder = builtInTableFunctionsTreeFolder;
        }

        internal IDictionary<string, Function> Functions { get; }
        internal IDictionary<string, Function> VoidFunctions { get; }
        internal IDictionary<string, Function> BooleanFunctions { get; }
        internal IDictionary<string, Function> ValueFunctions { get; }
        internal IDictionary<string, Function> DialogFunctions { get; }
        internal IDictionary<string, Function> TableFunctions { get; }

        internal TreeFolder VoidFunctionsTreeFolder { get; }
        internal TreeFolder BooleanFunctionsTreeFolder { get; }
        internal TreeFolder ValueFunctionsTreeFolder { get; }
        internal TreeFolder DialogFunctionsTreeFolder { get; }
        internal TreeFolder TableFunctionsTreeFolder { get; }

        internal TreeFolder BuiltInVoidFunctionsTreeFolder { get; }
        internal TreeFolder BuiltInBooleanFunctionsTreeFolder { get; }
        internal TreeFolder BuiltInValueFunctionsTreeFolder { get; }
        internal TreeFolder BuiltInDialogFunctionsTreeFolder { get; }
        internal TreeFolder BuiltInTableFunctionsTreeFolder { get; }
    }
}
