using ABIS.LogicBuilder.FlowBuilder;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditLiteralList;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditLiteralList.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.Helpers;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using System;
using System.Globalization;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class LiteralListBoxItemFactoryServices
    {
        internal static IServiceCollection AddLiteralListBoxItemFactories(this IServiceCollection services)
        {
            return services
                .AddSingleton<ILiteralListBoxItemFactory, LiteralListBoxItemFactory>();
        }
    }
}
