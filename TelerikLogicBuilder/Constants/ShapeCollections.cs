using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.Constants
{
    internal struct ShapeCollections
    {
        internal static readonly string[] Connectors = new string[] { UniversalMasterName.CONNECTOBJECT, UniversalMasterName.APP01CONNECTOBJECT, UniversalMasterName.APP02CONNECTOBJECT, UniversalMasterName.APP03CONNECTOBJECT, UniversalMasterName.APP04CONNECTOBJECT, UniversalMasterName.APP05CONNECTOBJECT, UniversalMasterName.APP06CONNECTOBJECT, UniversalMasterName.APP07CONNECTOBJECT, UniversalMasterName.APP08CONNECTOBJECT, UniversalMasterName.APP09CONNECTOBJECT, UniversalMasterName.APP10CONNECTOBJECT, UniversalMasterName.OTHERSCONNECTOBJECT };
        internal static readonly string[] ApplicationConnectors = new string[] { UniversalMasterName.APP01CONNECTOBJECT, UniversalMasterName.APP02CONNECTOBJECT, UniversalMasterName.APP03CONNECTOBJECT, UniversalMasterName.APP04CONNECTOBJECT, UniversalMasterName.APP05CONNECTOBJECT, UniversalMasterName.APP06CONNECTOBJECT, UniversalMasterName.APP07CONNECTOBJECT, UniversalMasterName.APP08CONNECTOBJECT, UniversalMasterName.APP09CONNECTOBJECT, UniversalMasterName.APP10CONNECTOBJECT, UniversalMasterName.OTHERSCONNECTOBJECT };
        internal static readonly string[] ApplicationSpecificConnectors = new string[] { UniversalMasterName.APP01CONNECTOBJECT, UniversalMasterName.APP02CONNECTOBJECT, UniversalMasterName.APP03CONNECTOBJECT, UniversalMasterName.APP04CONNECTOBJECT, UniversalMasterName.APP05CONNECTOBJECT, UniversalMasterName.APP06CONNECTOBJECT, UniversalMasterName.APP07CONNECTOBJECT, UniversalMasterName.APP08CONNECTOBJECT, UniversalMasterName.APP09CONNECTOBJECT, UniversalMasterName.APP10CONNECTOBJECT };
        internal static readonly string[] AllowedApplicationFlowShapes = new string[] { UniversalMasterName.ACTION, UniversalMasterName.MERGEOBJECT, UniversalMasterName.MODULE, UniversalMasterName.WAITCONDITIONOBJECT, UniversalMasterName.WAITDECISIONOBJECT };
        internal static readonly string[] EditConnectorShapes = new string[] { UniversalMasterName.DIALOG, UniversalMasterName.CONDITIONOBJECT, UniversalMasterName.DECISIONOBJECT };
        internal static readonly string[] EditDialogConnectorShapes = new string[] { UniversalMasterName.DIALOG };
        internal static readonly string[] DecisionShapes = new string[] { UniversalMasterName.CONDITIONOBJECT, UniversalMasterName.DECISIONOBJECT };
        internal static readonly string[] EndModuleShapes = new string[] { UniversalMasterName.ENDFLOW, UniversalMasterName.MODULEEND, UniversalMasterName.TERMINATE };

        internal static readonly HashSet<string> TextSearchableShapes = new(new string[] { UniversalMasterName.ACTION, UniversalMasterName.DIALOG, UniversalMasterName.CONDITIONOBJECT, UniversalMasterName.WAITCONDITIONOBJECT, UniversalMasterName.DECISIONOBJECT, UniversalMasterName.WAITDECISIONOBJECT, UniversalMasterName.JUMPOBJECT, UniversalMasterName.CONNECTOBJECT });
    }
}
