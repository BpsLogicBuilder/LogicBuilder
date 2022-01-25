using ABIS.LogicBuilder.FlowBuilder.Services;

namespace ABIS.LogicBuilder.FlowBuilder.AttributeReaders
{
    internal abstract class AttributeReader
    {
        protected readonly IExceptionHelper exceptionHelper;

        protected AttributeReader(IExceptionHelper exceptionHelper)
        {
            this.exceptionHelper = exceptionHelper;
        }

        protected int GetInt(object attribute, string propertyName)
        {
            if (attribute == null)
                throw this.exceptionHelper.CriticalException("{7A73B403-CE0B-47CB-A4DA-00889B7FB355}");

            return (int)attribute.GetType().GetProperty(propertyName).GetValue(attribute);
        }

        protected short GetShort(object attribute, string propertyName)
        {
            if (attribute == null)
                throw this.exceptionHelper.CriticalException("{198CA65E-C19D-4CFA-B398-AC2149AB4565}");

            return (short)attribute.GetType().GetProperty(propertyName).GetValue(attribute);
        }

        protected string GetString(object attribute, string propertyName)
        {
            if (attribute == null)
                throw this.exceptionHelper.CriticalException("{E855381B-8347-4958-A462-7D4D12D55CBE}");

            return (string)attribute.GetType().GetProperty(propertyName).GetValue(attribute);
        }
    }
}
