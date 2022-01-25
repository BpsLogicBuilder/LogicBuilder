using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using LogicBuilder.Attributes;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.AttributeReaders
{
    internal class NameValueAttributeReader: AttributeReader
    {
        public NameValueAttributeReader(object attribute, IExceptionHelper exceptionHelper) : base(exceptionHelper)
        {
            this.attribute = attribute;
        }

        private readonly object attribute;

        public KeyValuePair<string, string> GetNameValuePair() 
            => new            (
                GetString(attribute, nameof(NameValueAttribute.Name)),
                GetString(attribute, nameof(NameValueAttribute.Value))
            );
    }
}
