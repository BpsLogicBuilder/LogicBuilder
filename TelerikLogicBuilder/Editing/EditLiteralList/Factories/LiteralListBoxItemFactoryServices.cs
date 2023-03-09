﻿using ABIS.LogicBuilder.FlowBuilder;
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
                .AddTransient<Func<string, string, Type, IApplicationForm, ListParameterInputStyle, ILiteralListBoxItem>>
                (
                    provider =>
                    (visibleText, hiddenText, assignedTo, applicationForm, listControl) =>
                    {
                        return listControl switch
                        {
                            ListParameterInputStyle.HashSetForm => new LiteralHashSetFormListBoxItem
                            (
                                provider.GetRequiredService<ILiteralElementValidator>(),
                                provider.GetRequiredService<IXmlDataHelper>(),
                                provider.GetRequiredService<IXmlDocumentHelpers>(),
                                visibleText,
                                hiddenText,
                                assignedTo,
                                applicationForm
                            ),
                            ListParameterInputStyle.ListForm => new LiteralListFormListBoxItem
                            (
                                provider.GetRequiredService<ILiteralElementValidator>(),
                                provider.GetRequiredService<IXmlDataHelper>(),
                                provider.GetRequiredService<IXmlDocumentHelpers>(),
                                visibleText,
                                hiddenText,
                                assignedTo,
                                applicationForm
                            ),
                            _ => throw new CriticalLogicBuilderException(string.Format(CultureInfo.InvariantCulture, Strings.invalidArgumentTextFormat, "{25DDAA19-4F80-40DA-943A-3CEA251F186D}")),
                        };
                    }
                )
                .AddTransient<ILiteralListBoxItemFactory, LiteralListBoxItemFactory>();
        }
    }
}