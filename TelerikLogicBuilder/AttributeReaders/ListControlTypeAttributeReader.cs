﻿using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using LogicBuilder.Attributes;

namespace ABIS.LogicBuilder.FlowBuilder.AttributeReaders
{
    internal class ListControlTypeAttributeReader : AttributeReader
    {
        public ListControlTypeAttributeReader(
            IExceptionHelper exceptionHelper,
            object attribute) : base(exceptionHelper)
        {
            this.attribute = attribute;
        }

        private readonly object attribute;

        public ListParameterInputStyle GetListParameterInputStyle() 
            => GetShort(attribute, nameof(ListEditorControlAttribute.ControlType)) switch
            {
                (short)ListControlType.HashSetForm => ListParameterInputStyle.HashSetForm,
                (short)ListControlType.ListForm => ListParameterInputStyle.ListForm,
                (short)ListControlType.Connectors => ListParameterInputStyle.Connectors,
                _ => ListParameterInputStyle.HashSetForm,
            };

        public ListVariableInputStyle GetListVariableInputStyle() 
            => GetShort(attribute, nameof(ListEditorControlAttribute.ControlType)) switch
            {
                (short)ListControlType.HashSetForm => ListVariableInputStyle.HashSetForm,
                (short)ListControlType.ListForm => ListVariableInputStyle.ListForm,
                _ => ListVariableInputStyle.HashSetForm,
            };
    }
}
