using LogicBuilder.Attributes;
using LogicBuilder.Forms.Parameters;
using System;
using System.Collections.Generic;
using System.Text;

namespace Contoso.Test.Flow
{
    public class CustomDialogs : ICustomDialogs
    {
        public void DisplayString(string setting, [ListEditorControl(ListControlType.Connectors)] ICollection<ConnectorParameters> buttons)
        {
            throw new NotImplementedException();
        }
    }
}
