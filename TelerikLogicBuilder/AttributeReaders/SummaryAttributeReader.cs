using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using LogicBuilder.Attributes;

namespace ABIS.LogicBuilder.FlowBuilder.AttributeReaders
{
    internal class SummaryAttributeReader : AttributeReader
    {
        public SummaryAttributeReader(
            IExceptionHelper exceptionHelper,
            object attribute) : base(exceptionHelper)
        {
            this.attribute = attribute;
        }

        private readonly object attribute;

        public string Summary => GetString(attribute, nameof(SummaryAttribute.Summary));
    }
}
