using ABIS.LogicBuilder.FlowBuilder;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditObjectList;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditObjectList.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditObjectList.Helpers;
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
    internal static class ObjectListBoxItemFactoryServices
    {
        internal static IServiceCollection AddObjectListBoxItemFactories(this IServiceCollection services)
        {
            return services
                .AddTransient<Func<string, string, Type, IApplicationControl, ListParameterInputStyle, IObjectListBoxItem>>
                (
                    provider =>
                    (visibleText, hiddenText, assignedTo, applicationControl, listControl) =>
                    {
                        return listControl switch
                        {
                            ListParameterInputStyle.HashSetForm => new ObjectHashSetFormListBoxItem
                            (
                                provider.GetRequiredService<IObjectElementValidator>(),
                                provider.GetRequiredService<IObjectHashSetListBoxItemComparer>(),
                                provider.GetRequiredService<IXmlDataHelper>(),
                                provider.GetRequiredService<IXmlDocumentHelpers>(),
                                visibleText,
                                hiddenText,
                                assignedTo,
                                applicationControl
                            ),
                            ListParameterInputStyle.ListForm => new ObjectListFormListBoxItem
                            (
                                provider.GetRequiredService<IObjectElementValidator>(),
                                provider.GetRequiredService<IXmlDataHelper>(),
                                provider.GetRequiredService<IXmlDocumentHelpers>(),
                                visibleText,
                                hiddenText,
                                assignedTo,
                                applicationControl
                            ),
                            _ => throw new CriticalLogicBuilderException(string.Format(CultureInfo.InvariantCulture, Strings.invalidArgumentTextFormat, "{1FBB18EC-AD29-4D87-A672-F8D52E2742B0}")),
                        };
                    }
                )
                .AddTransient<Func<string, string, Type, IApplicationControl, ListVariableInputStyle, IObjectListBoxItem>>
                (
                    provider =>
                    (visibleText, hiddenText, assignedTo, applicationControl, listControl) =>
                    {
                        return listControl switch
                        {
                            ListVariableInputStyle.HashSetForm => new ObjectHashSetFormListBoxItem
                            (
                                provider.GetRequiredService<IObjectElementValidator>(),
                                provider.GetRequiredService<IObjectHashSetListBoxItemComparer>(),
                                provider.GetRequiredService<IXmlDataHelper>(),
                                provider.GetRequiredService<IXmlDocumentHelpers>(),
                                visibleText,
                                hiddenText,
                                assignedTo,
                                applicationControl
                            ),
                            ListVariableInputStyle.ListForm => new ObjectListFormListBoxItem
                            (
                                provider.GetRequiredService<IObjectElementValidator>(),
                                provider.GetRequiredService<IXmlDataHelper>(),
                                provider.GetRequiredService<IXmlDocumentHelpers>(),
                                visibleText,
                                hiddenText,
                                assignedTo,
                                applicationControl
                            ),
                            _ => throw new CriticalLogicBuilderException(string.Format(CultureInfo.InvariantCulture, Strings.invalidArgumentTextFormat, "{700874A9-4298-4606-83F1-CC1662059F1C}")),
                        };
                    }
                )
                .AddTransient<IObjectListBoxItemFactory, ObjectListBoxItemFactory>();
        }
    }
}
