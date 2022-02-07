using System;
using System.Runtime.Serialization;

namespace ABIS.LogicBuilder.FlowBuilder.Exceptions
{
    /// <summary>
    /// Exceptions thrown from this application
    /// </summary>
    [Serializable()]
    public class XmlValidationException : Exception
    {
        public XmlValidationException()
            : base(Strings.defaultErrorMessage)
        {
            // Add any type-specific logic, and supply the default message.
        }


        public XmlValidationException(string logicBuilderMessage)
            : base(logicBuilderMessage)
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public XmlValidationException(string logicBuilderMessage, Exception ex)
            : base(logicBuilderMessage, ex)
        {
            //
            // TODO: Add constructor logic here
            //
        }

        protected XmlValidationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            // Implement type-specific serialization constructor logic.
        }
    }
}
