using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using LogicBuilder.Attributes;

namespace ABIS.LogicBuilder.FlowBuilder.AttributeReaders
{
    internal class FunctionGroupAttributeReader : AttributeReader
    {
        public FunctionGroupAttributeReader(object attribute, IExceptionHelper exceptionHelper) : base(exceptionHelper)
        {
            this.attribute = attribute;
        }

        private readonly object attribute;

        public FunctionCategories GetFunctionCategory() 
            => GetShort(attribute, nameof(FunctionGroupAttribute.FunctionGroup)) switch
            {
                (short)FunctionGroup.Standard => FunctionCategories.Standard,
                (short)FunctionGroup.DialogForm => FunctionCategories.DialogForm,
                _ => FunctionCategories.Standard,
            };
    }
}
