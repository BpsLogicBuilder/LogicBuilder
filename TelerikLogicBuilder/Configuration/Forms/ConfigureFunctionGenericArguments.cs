using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.Forms
{
    internal partial class ConfigureFunctionGenericArguments : Telerik.WinControls.UI.RadForm, IConfigureGenericArguments
    {
        public ConfigureFunctionGenericArguments()
        {
            InitializeComponent();
        }

        public Action OnInvalidXmlRestoreDocumentAndThrow => throw new NotImplementedException();

        public RadTreeView TreeView => throw new NotImplementedException();

        public XmlDocument XmlDocument => throw new NotImplementedException();
    }
}
