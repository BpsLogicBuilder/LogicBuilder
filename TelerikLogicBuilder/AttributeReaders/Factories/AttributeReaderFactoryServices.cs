using ABIS.LogicBuilder.FlowBuilder.AttributeReaders;
using ABIS.LogicBuilder.FlowBuilder.AttributeReaders.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class AttributeReaderFactoryServices
    {
        internal static IServiceCollection AddAttributeReaderFactories(this IServiceCollection services)
        {
            return services
                .AddTransient<Func<object, AlsoKnownAsAttributeReader>>
                (
                    provider =>
                    attribute => new AlsoKnownAsAttributeReader
                    (
                        provider.GetRequiredService<IExceptionHelper>(),
                        attribute
                    )
                )
                .AddTransient<Func<object, CommentsAttributeReader>>
                (
                    provider =>
                    attribute => new CommentsAttributeReader
                    (
                        provider.GetRequiredService<IExceptionHelper>(),
                        attribute
                    )
                )
                .AddTransient<Func<object, DomainAttributeReader>>
                (
                    provider =>
                    attribute => new DomainAttributeReader
                    (
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<IStringHelper>(),
                        attribute
                    )
                )
                .AddTransient<Func<object, FunctionGroupAttributeReader>>
                (
                    provider =>
                    attribute => new FunctionGroupAttributeReader
                    (
                        provider.GetRequiredService<IExceptionHelper>(),
                        attribute
                    )
                )
                .AddTransient<Func<object, ListControlTypeAttributeReader>>
                (
                    provider =>
                    attribute => new ListControlTypeAttributeReader
                    (
                        provider.GetRequiredService<IExceptionHelper>(),
                        attribute
                    )
                )
                .AddTransient<Func<object, NameValueAttributeReader>>
                (
                    provider =>
                    attribute => new NameValueAttributeReader
                    (
                        provider.GetRequiredService<IExceptionHelper>(),
                        attribute
                    )
                )
                .AddTransient<Func<object, ParameterControlTypeAttributeReader>>
                (
                    provider =>
                    attribute => new ParameterControlTypeAttributeReader
                    (
                        provider.GetRequiredService<IExceptionHelper>(),
                        attribute
                    )
                )
                .AddTransient<Func<object, SummaryAttributeReader>>
                (
                    provider =>
                    attribute => new SummaryAttributeReader
                    (
                        provider.GetRequiredService<IExceptionHelper>(),
                        attribute
                    )
                )
                .AddTransient<Func<object, VariableControlTypeAttributeReader>>
                (
                    provider =>
                    attribute => new VariableControlTypeAttributeReader
                    (
                        provider.GetRequiredService<IExceptionHelper>(),
                        attribute
                    )
                )
                .AddTransient<IAttributeReaderFactory, AttributeReaderFactory>();
        }
    }
}
