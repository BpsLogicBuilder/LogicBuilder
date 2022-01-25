using ABIS.LogicBuilder.FlowBuilder.Services;
using LogicBuilder.Attributes;

namespace ABIS.LogicBuilder.FlowBuilder.AttributeReaders
{
    internal class AlsoKnownAsAttributeReader : AttributeReader
    {
        public AlsoKnownAsAttributeReader(object attribute, IExceptionHelper exceptionHelper) : base(exceptionHelper)
        {
            this.attribute = attribute;
        }

        private readonly object attribute;

        public string AlsoKnownAs 
            => GetString(attribute, nameof(AlsoKnownAsAttribute.AlsoKnownAs));
    }
}
