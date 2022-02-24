using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace ABIS.LogicBuilder.FlowBuilder.XmlValidation.Configuration
{
    internal class ConstructorsXmlValidatorUtility : XmlValidatorUtility
    {
        internal ConstructorsXmlValidatorUtility(string xmlString) : base(Schemas.ConstructorSchema, xmlString)
        {
        }
    }
}
