using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.AttributeReaders.Factories
{
    internal class AttributeReaderFactory : IAttributeReaderFactory
    {
        public AlsoKnownAsAttributeReader GetAlsoKnownAsAttributeReader(object attribute)
            => new            (
                Program.ServiceProvider.GetRequiredService<IExceptionHelper>(),
                attribute
            );

        public CommentsAttributeReader GetCommentsAttributeReader(object attribute)
            => new            (
                Program.ServiceProvider.GetRequiredService<IExceptionHelper>(),
                attribute
            );

        public DomainAttributeReader GetDomainAttributeReader(object attribute)
            => new            (
                Program.ServiceProvider.GetRequiredService<IExceptionHelper>(),
                Program.ServiceProvider.GetRequiredService<IStringHelper>(),
                attribute
            );

        public FunctionGroupAttributeReader GetFunctionGroupAttributeReader(object attribute)
            => new            (
                Program.ServiceProvider.GetRequiredService<IExceptionHelper>(),
                attribute
            );

        public ListControlTypeAttributeReader GetListControlTypeAttributeReader(object attribute)
            => new            (
                Program.ServiceProvider.GetRequiredService<IExceptionHelper>(),
                attribute
            );

        public NameValueAttributeReader GetNameValueAttributeReader(object attribute)
            => new            (
                Program.ServiceProvider.GetRequiredService<IExceptionHelper>(),
                attribute
            );

        public ParameterControlTypeAttributeReader GetParameterControlTypeAttributeReader(object attribute)
            => new            (
                Program.ServiceProvider.GetRequiredService<IExceptionHelper>(),
                attribute
            );

        public SummaryAttributeReader GetSummaryAttributeReader(object attribute)
            => new            (
                Program.ServiceProvider.GetRequiredService<IExceptionHelper>(),
                attribute
            );

        public VariableControlTypeAttributeReader GetVariableControlTypeAttributeReader(object attribute)
            => new            (
                Program.ServiceProvider.GetRequiredService<IExceptionHelper>(),
                attribute
            );
    }
}
