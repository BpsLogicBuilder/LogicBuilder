using LogicBuilder.Attributes;
using LogicBuilder.Forms.Parameters;
using System.Collections.Generic;

namespace Contoso.Test.Flow
{
    public interface ICustomDialogs
    {
        [AlsoKnownAs("DisplayString")]
        [FunctionGroup(FunctionGroup.DialogForm)]
        void DisplayString
        (
            string setting,

            [ListEditorControl(ListControlType.Connectors)]
            ICollection<ConnectorParameters> buttons
        );
    }
}
