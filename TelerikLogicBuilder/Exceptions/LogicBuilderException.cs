using System;
using System.Runtime.Serialization;

namespace ABIS.LogicBuilder.FlowBuilder.Exceptions
{
    [Serializable()]
    public class LogicBuilderException : Exception
    {
        public LogicBuilderException()
            : base(Strings.defaultErrorMessage)
        {
            // Add any type-specific logic, and supply the default message.
        }


        public LogicBuilderException(string logicBuilderMessage)
            : base(logicBuilderMessage)
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public LogicBuilderException(string logicBuilderMessage, Exception ex)
            : base(logicBuilderMessage, ex)
        {
            //
            // TODO: Add constructor logic here
            //
        }

        protected LogicBuilderException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            // Implement type-specific serialization constructor logic.
        }
    }
}
