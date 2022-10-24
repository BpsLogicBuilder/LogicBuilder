using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using LogicBuilder.Attributes;

namespace ABIS.LogicBuilder.FlowBuilder.AttributeReaders
{
    internal class ParameterControlTypeAttributeReader: AttributeReader
    {
        public ParameterControlTypeAttributeReader(
            IExceptionHelper exceptionHelper,
            object attribute) : base(exceptionHelper)
        {
            this.attribute = attribute;
        }

        private readonly object attribute;

        public LiteralParameterInputStyle GetLiteralInputStyle() 
            => GetShort(attribute, nameof(ParameterEditorControlAttribute.ControlType)) switch
            {
                (short)ParameterControlType.SingleLineTextBox => LiteralParameterInputStyle.SingleLineTextBox,
                (short)ParameterControlType.MultipleLineTextBox => LiteralParameterInputStyle.MultipleLineTextBox,
                (short)ParameterControlType.DropDown => LiteralParameterInputStyle.DropDown,
                (short)ParameterControlType.DomainAutoComplete => LiteralParameterInputStyle.DomainAutoComplete,
                (short)ParameterControlType.TypeAutoComplete => LiteralParameterInputStyle.TypeAutoComplete,
                (short)ParameterControlType.PropertyInput => LiteralParameterInputStyle.PropertyInput,
                (short)ParameterControlType.ParameterSourcedPropertyInput => LiteralParameterInputStyle.ParameterSourcedPropertyInput,
                (short)ParameterControlType.ParameterSourceOnly => LiteralParameterInputStyle.ParameterSourceOnly,
                _ => LiteralParameterInputStyle.SingleLineTextBox,
            };

        public ObjectParameterInputStyle GetObjectInputStyle() 
            => GetShort(attribute, nameof(ParameterEditorControlAttribute.ControlType)) switch
            {
                (short)ParameterControlType.Form => ObjectParameterInputStyle.Form,
                _ => ObjectParameterInputStyle.Form,
            };
    }
}
