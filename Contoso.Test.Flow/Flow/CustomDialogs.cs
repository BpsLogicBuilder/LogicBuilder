using Contoso.Forms.Parameters;
using LogicBuilder.Attributes;
using LogicBuilder.Forms.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Contoso.Test.Flow
{
    public class CustomDialogs : ICustomDialogs
    {
        public void DisplayString(string setting, [ListEditorControl(ListControlType.Connectors)] ICollection<ConnectorParameters> buttons)
        {
            ICollection<CommandButtonParameters> parameters = buttons.Select(b => (CommandButtonParameters)b.ConnectorData).ToList();
            throw new NotImplementedException();
        }
    }
}
