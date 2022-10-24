using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using LogicBuilder.Attributes;

namespace ABIS.LogicBuilder.FlowBuilder.AttributeReaders
{
    internal class CommentsAttributeReader : AttributeReader
    {
        public CommentsAttributeReader(IExceptionHelper exceptionHelper, object attribute) : base(exceptionHelper)
        {
            this.attribute = attribute;
        }

        private readonly object attribute;

        public string Comments 
            => GetString(attribute, nameof(CommentsAttribute.Comments));
    }
}
