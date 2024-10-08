﻿using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFragments.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class FragmentItemFactoryServices
    {
        internal static IServiceCollection AddFragmentItemFactories(this IServiceCollection services)
        {
            return services
                .AddTransient<Func<string, string, string, Fragment>>
                (
                    provider =>
                    (name, xml, description) => new Fragment
                    (
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        name,
                        xml,
                        description
                    )
                )
                .AddTransient<IFragmentItemFactory, FragmentItemFactory>();
        }
    }
}
