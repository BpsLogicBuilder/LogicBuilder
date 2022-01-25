using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using LogicBuilder.Attributes;

namespace ABIS.LogicBuilder.FlowBuilder.AttributeReaders
{
    internal class VariableControlTypeAttributeReader : AttributeReader
    {
        public VariableControlTypeAttributeReader(object attribute, IExceptionHelper exceptionHelper) : base(exceptionHelper)
        {
            this.attribute = attribute;
        }

        private readonly object attribute;

        public LiteralVariableInputStyle GetLiteralInputStyle() 
            => GetShort(attribute, nameof(VariableEditorControlAttribute.ControlType)) switch
            {
                (short)VariableControlType.SingleLineTextBox => LiteralVariableInputStyle.SingleLineTextBox,
                (short)VariableControlType.MultipleLineTextBox => LiteralVariableInputStyle.MultipleLineTextBox,
                (short)VariableControlType.DropDown => LiteralVariableInputStyle.DropDown,
                (short)VariableControlType.TypeAutoComplete => LiteralVariableInputStyle.TypeAutoComplete,
                (short)VariableControlType.DomainAutoComplete => LiteralVariableInputStyle.DomainAutoComplete,
                (short)VariableControlType.PropertyInput => LiteralVariableInputStyle.PropertyInput,
                _ => LiteralVariableInputStyle.SingleLineTextBox,
            };

        public ObjectVariableInputStyle GetObjectInputStyle() 
            => GetShort(attribute, nameof(VariableEditorControlAttribute.ControlType)) switch
            {
                (short)VariableControlType.Form => ObjectVariableInputStyle.Form,
                _ => ObjectVariableInputStyle.Form,
            };
    }
}
