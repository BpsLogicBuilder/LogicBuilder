using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using LogicBuilder.Attributes;

namespace ABIS.LogicBuilder.FlowBuilder.AttributeReaders
{
    internal class AlsoKnownAsAttributeReader : AttributeReader
    {
        public AlsoKnownAsAttributeReader(IExceptionHelper exceptionHelper, object attribute) : base(exceptionHelper)
        {
            this.attribute = attribute;
        }

        private readonly object attribute;

        public string AlsoKnownAs 
            => GetString(attribute, nameof(AlsoKnownAsAttribute.AlsoKnownAs));
    }
}
