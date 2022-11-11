using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.Forms
{
    internal interface IConfigureGenericArguments
    {
        Action OnInvalidXmlRestoreDocumentAndThrow { get; }
        RadTreeView TreeView { get; }
        XmlDocument XmlDocument { get; }
    }
}
