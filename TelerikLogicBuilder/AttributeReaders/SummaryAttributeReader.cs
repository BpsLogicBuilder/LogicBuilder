using ABIS.LogicBuilder.FlowBuilder.Services;
using LogicBuilder.Attributes;

namespace ABIS.LogicBuilder.FlowBuilder.AttributeReaders
{
    internal class SummaryAttributeReader: AttributeReader
    {
        public SummaryAttributeReader(object attribute, IExceptionHelper exceptionHelper) : base(exceptionHelper)
        {
            this.attribute = attribute;
        }

        private readonly object attribute;

        public string Summary => GetString(attribute, nameof(SummaryAttribute.Summary));
    }
}
