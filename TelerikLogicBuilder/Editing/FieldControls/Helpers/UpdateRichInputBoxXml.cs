using ABIS.LogicBuilder.FlowBuilder.Components;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Helpers
{
    internal class UpdateRichInputBoxXml : IUpdateRichInputBoxXml
    {
        public void Update(XmlElement inputBoxXmlElement, RichInputBox richInputBox)
        {
            richInputBox.Clear();
            foreach (XmlNode childNode in inputBoxXmlElement.ChildNodes)
            {
                switch (childNode.NodeType)
                {
                    case XmlNodeType.Element:
                        XmlElement xmlElement = (XmlElement)childNode;
                        switch (xmlElement.Name)
                        {
                            case XmlDataConstants.VARIABLEELEMENT:
                                richInputBox.InsertLink(xmlElement.GetAttribute(XmlDataConstants.NAMEATTRIBUTE), xmlElement.OuterXml, LinkType.Variable);
                                break;
                            case XmlDataConstants.FUNCTIONELEMENT:
                                richInputBox.InsertLink(xmlElement.GetAttribute(XmlDataConstants.VISIBLETEXTATTRIBUTE), xmlElement.OuterXml, LinkType.Function);
                                break;
                            case XmlDataConstants.CONSTRUCTORELEMENT:
                                richInputBox.InsertLink(xmlElement.GetAttribute(XmlDataConstants.VISIBLETEXTATTRIBUTE), xmlElement.OuterXml, LinkType.Constructor);
                                break;
                            default:
                                break;
                        }
                        break;
                    case XmlNodeType.Text:
                        XmlText xmlText = (XmlText)childNode;
                        richInputBox.InsertText(xmlText.Value!);//not null for XmlNodeType.Text.
                        break;
                    case XmlNodeType.Whitespace:
                        XmlWhitespace xmlWhitespace = (XmlWhitespace)childNode;
                        richInputBox.InsertText(xmlWhitespace.Value!);//not null for XmlNodeType.Whitespace.
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
