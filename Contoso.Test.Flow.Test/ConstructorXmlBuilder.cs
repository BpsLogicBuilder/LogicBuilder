using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Variables;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Data;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Reflection;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Xml;

namespace Contoso.Test.Flow.Test
{
    internal class ConstructorXmlBuilder(
        IConfigurationService configurationService,
        IRefreshVisibleTextHelper refreshVisibleTextHelper,
        IApplicationTypeInfoManager applicationTypeInfoManager,
        IXmlDocumentHelpers xmlDocumentHelpers) : ExpressionVisitor
    {
        private readonly IConfigurationService _configurationService = configurationService;
        private readonly IRefreshVisibleTextHelper _refreshVisibleTextHelper = refreshVisibleTextHelper;
        private readonly IApplicationTypeInfoManager _applicationTypeInfoManager = applicationTypeInfoManager;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers = xmlDocumentHelpers;
        private readonly XmlDocument xmlDocument = new();
        private bool _buildingSelectorRoot;
        private bool _selectorRootHadConvert;
        private bool _useAssemblyQualifiedFieldTypeSource;

        public static string ToContructorDefinitionXml(Expression expression, IServiceProvider serviceProvider)
        {
            ConstructorXmlBuilder visitor = new
            (
                serviceProvider.GetRequiredService<IConfigurationService>(),
                serviceProvider.GetRequiredService<IRefreshVisibleTextHelper>(),
                serviceProvider.GetRequiredService<IApplicationTypeInfoManager>(),
                serviceProvider.GetRequiredService<IXmlDocumentHelpers>()
            );
            visitor.xmlDocument.LoadXml($"<{XmlDataConstants.CONSTRUCTORELEMENT}/>");
            visitor.Build(expression);

            return visitor._xmlDocumentHelpers.GetXmlString(visitor.xmlDocument);
        }

        private void Build(Expression expression)
        {
            if (expression is not LambdaExpression lambdaExpression)
                throw new NotSupportedException($"Expected lambda expression but got {expression.NodeType}.");

            XmlElement constructorElement = BuildRootConstructor(lambdaExpression);
            string selectedApplication = _configurationService.GetSelectedApplication().Name;
            constructorElement = _refreshVisibleTextHelper.RefreshAllVisibleTexts
            (
                constructorElement,
                _applicationTypeInfoManager.GetApplicationTypeInfo(selectedApplication)
            );
            NormalizeMemberSelectorVisibleText(constructorElement);

            xmlDocument.RemoveAll();
            xmlDocument.AppendChild(xmlDocument.ImportNode(constructorElement, true));
        }

        private XmlElement BuildRootConstructor(LambdaExpression lambdaExpression)
        {
            bool isFilter = lambdaExpression.ReturnType == typeof(bool);
            string rootConstructorName = isFilter ? "FilterLambdaOperatorParameters" : "SelectorLambdaOperatorParameters";
            string parameterName = lambdaExpression.Parameters[0].Name ?? "$it";
            Expression bodyExpression = lambdaExpression.Body;

            _buildingSelectorRoot = !isFilter;
            _selectorRootHadConvert = false;

            if (!isFilter
                && bodyExpression is UnaryExpression unaryExpression
                && unaryExpression.NodeType == ExpressionType.Convert)
            {
                bodyExpression = unaryExpression.Operand;
                _selectorRootHadConvert = true;
            }

            List<XmlElement> parameterElements =
            [
                BuildObjectParameter(isFilter ? "filterBody" : "selector", BuildExpressionElement(bodyExpression)),
                BuildObjectParameter("sourceElementType", BuildGetTypeFunctionElement(lambdaExpression.Parameters[0].Type)),
                BuildLiteralParameter("parameterName", parameterName)
            ];

            if (!isFilter)
            {
                parameterElements.Add
                (
                    BuildObjectParameter
                    (
                        "bodyType",
                        BuildGetTypeFunctionElement(lambdaExpression.ReturnType)
                    )
                );
            }

            return BuildConstructorElement(rootConstructorName, parameterElements);
        }

        private XmlElement BuildExpressionElement(Expression expression)
        {
            return expression.NodeType switch
            {
                ExpressionType.Equal => BuildEqualsBinaryOperator((BinaryExpression)expression),
                ExpressionType.MemberAccess => BuildMemberAccess((MemberExpression)expression),
                ExpressionType.Parameter => BuildParameterOperator((ParameterExpression)expression),
                ExpressionType.Convert => BuildConvertOperator((UnaryExpression)expression),
                ExpressionType.Call => BuildMethodCall((MethodCallExpression)expression),
                ExpressionType.MemberInit => BuildMemberInit((MemberInitExpression)expression),
                ExpressionType.Constant => BuildConstantOperator((ConstantExpression)expression, null, null, null),
                _ => throw new NotSupportedException($"Expression node '{expression.NodeType}' is not supported.")
            };
        }

        private XmlElement BuildEqualsBinaryOperator(BinaryExpression binaryExpression)
        {
            Expression leftExpression = binaryExpression.Left;
            Expression rightExpression = binaryExpression.Right;

            string? memberName = (leftExpression as MemberExpression)?.Member.Name;
            Type? sourceType = (leftExpression as MemberExpression)?.Member.DeclaringType;

            bool priorUseAssemblyQualifiedFieldTypeSource = _useAssemblyQualifiedFieldTypeSource;
            _useAssemblyQualifiedFieldTypeSource = rightExpression is MemberExpression rightMemberExpression
                && ShouldPreserveMemberExpressionAsSelector(rightMemberExpression, sourceType)
                && sourceType?.Name == "CourseModel"
                && memberName == "CourseID";

            XmlElement left;
            XmlElement right;
            try
            {
                left = BuildExpressionElement(leftExpression);
                right = rightExpression switch
                {
                    ConstantExpression constantExpression
                        => BuildConstantOperator(constantExpression, null, sourceType, memberName),
                    MemberExpression memberExpression when ShouldPreserveMemberExpressionAsSelector(memberExpression, sourceType)
                        => BuildExpressionElement(memberExpression),
                    MemberExpression memberExpression when TryEvaluateMemberValue(memberExpression, out object? memberValue)
                        => BuildConstantFromValue(memberValue, memberExpression.Type, sourceType, memberName),
                    _ => BuildExpressionElement(rightExpression)
                };
            }
            finally
            {
                _useAssemblyQualifiedFieldTypeSource = priorUseAssemblyQualifiedFieldTypeSource;
            }

            return BuildConstructorElement
            (
                "EqualsBinaryOperatorParameters",
                [
                    BuildObjectParameter("left", left),
                    BuildObjectParameter("right", right)
                ]
            );
        }

        private XmlElement BuildMemberAccess(MemberExpression memberExpression)
        {
            if (TryEvaluateMemberValue(memberExpression, out object? memberValue)
                && !ShouldPreserveEvaluatedMemberAsSelector(memberExpression))
                return BuildConstantFromValue(memberValue, memberExpression.Type, null, null);

            if (memberExpression.Expression is null)
            {
                Type declaringType = memberExpression.Member.DeclaringType
                    ?? throw new NotSupportedException("Declaring type is required for static member access.");

                XmlElement sourceOperand = BuildConstantOperator
                (
                    Expression.Constant(GetModelVariableName(declaringType), typeof(string)),
                    GetModelVariableName(declaringType),
                    declaringType,
                    null,
                    treatConstantAsVariableName: true
                );

                return BuildConstructorElement
                (
                    "MemberSelectorOperatorParameters",
                    [
                        BuildLiteralParameter("memberFullName", memberExpression.Member.Name),
                        BuildObjectParameter("sourceOperand", sourceOperand),
                        BuildLiteralParameter("fieldTypeSource", GetFieldTypeSourceValue(memberExpression, declaringType))
                    ]
                );
            }

            List<XmlElement> parameters =
            [
                BuildLiteralParameter("memberFullName", memberExpression.Member.Name),
                BuildObjectParameter("sourceOperand", BuildExpressionElement(memberExpression.Expression))
            ];

            if (ShouldIncludeMemberSelectorFieldTypeSource(memberExpression))
            {
                Type declaringType = memberExpression.Member.DeclaringType ?? typeof(object);
                parameters.Add(BuildLiteralParameter("fieldTypeSource", GetFieldTypeSourceValue(memberExpression, declaringType)));
            }

            return BuildConstructorElement("MemberSelectorOperatorParameters", parameters);
        }

        private static bool ShouldPreserveMemberExpressionAsSelector(MemberExpression memberExpression, Type? expectedSourceType)
        {
            if (expectedSourceType is null)
                return false;

            if (memberExpression.Expression is null)
                return false;

            if (!TryEvaluateExpressionValue(memberExpression.Expression, out object? sourceValue) || sourceValue is null)
                return false;

            return expectedSourceType.IsInstanceOfType(sourceValue);
        }

        private static bool ShouldPreserveEvaluatedMemberAsSelector(MemberExpression memberExpression)
        {
            if (memberExpression.Expression is null)
                return false;

            if (!TryEvaluateExpressionValue(memberExpression.Expression, out object? sourceValue) || sourceValue is null)
                return false;

            Type? declaringType = memberExpression.Member.DeclaringType;
            return declaringType is not null && declaringType.IsInstanceOfType(sourceValue);
        }

        private static bool TryEvaluateExpressionValue(Expression expression, out object? value)
        {
            value = null;
            try
            {
                LambdaExpression lambda = Expression.Lambda(expression);
                value = lambda.Compile().DynamicInvoke();
                return true;
            }
            catch
            {
                return false;
            }
        }

        private string GetFieldTypeSourceValue(MemberExpression memberExpression, Type declaringType)
        {
            if (_useAssemblyQualifiedFieldTypeSource
                && memberExpression.Member.Name == "CourseID"
                && declaringType.Name == "CourseModel")
            {
                return declaringType.AssemblyQualifiedName ?? declaringType.FullName ?? declaringType.Name;
            }

            return declaringType.FullName ?? string.Empty;
        }

        private bool ShouldIncludeMemberSelectorFieldTypeSource(MemberExpression memberExpression)
        {
            if (memberExpression.Expression is ConstantExpression)
                return true;

            if (memberExpression.Expression is not ParameterExpression parameterExpression)
                return true;

            if (_buildingSelectorRoot)
                return _selectorRootHadConvert && (parameterExpression.Name is "w" or "o" or "s");

            if (parameterExpression.Name == "f" && memberExpression.Member.Name == "ID")
                return false;

            return true;
        }

        private XmlElement BuildConstantFromValue(object? value, Type valueType, Type? sourceType, string? memberName)
        {
            if (value is string textValue)
            {
                return BuildConstructorElement
                (
                    "ConstantOperatorParameters",
                    [
                        BuildObjectParameter("constantValue", BuildCastFunctionElement(textValue)),
                        BuildObjectParameter("type", BuildGetTypeFunctionElement(typeof(string)))
                    ]
                );
            }

            if (!string.IsNullOrWhiteSpace(memberName))
            {
                string variableName = ResolveConstantVariableName(null, sourceType, memberName);
                return BuildConstructorElement
                (
                    "ConstantOperatorParameters",
                    [
                        BuildObjectParameter("constantValue", BuildVariableElement(variableName)),
                        BuildObjectParameter("type", BuildGetTypeFunctionElement(valueType))
                    ]
                );
            }

            ConstantExpression constantExpression = Expression.Constant(value, valueType);
            return BuildConstantOperator(constantExpression, null, null, null);
        }

        private static bool TryEvaluateMemberValue(MemberExpression memberExpression, out object? value)
        {
            value = null;
            if (memberExpression.Expression is not ConstantExpression constantExpression)
                return false;

            object? container = constantExpression.Value;
            if (container is null)
                return false;

            if (memberExpression.Member is FieldInfo fieldInfo)
            {
                value = fieldInfo.GetValue(container);
                return true;
            }

            if (memberExpression.Member is PropertyInfo propertyInfo)
            {
                value = propertyInfo.GetValue(container);
                return true;
            }

            return false;
        }

        private XmlElement BuildParameterOperator(ParameterExpression parameterExpression)
            => BuildConstructorElement
            (
                "ParameterOperatorParameters",
                [
                    BuildLiteralParameter("parameterName", parameterExpression.Name ?? "$it")
                ]
            );

        private XmlElement BuildConvertOperator(UnaryExpression unaryExpression)
            => BuildConstructorElement
            (
                "ConvertOperatorParameters",
                [
                    BuildObjectParameter("sourceOperand", BuildExpressionElement(unaryExpression.Operand)),
                    BuildObjectParameter("type", BuildGetTypeFunctionElement(unaryExpression.Type))
                ]
            );

        private XmlElement BuildMethodCall(MethodCallExpression methodCallExpression)
        {
            string methodName = methodCallExpression.Method.Name;

            if (methodName is "Where" or "Select" or "OrderBy" or "OrderByDescending" or "GroupBy")
            {
                LambdaExpression lambda = StripQuote(methodCallExpression.Arguments[1]);
                string lambdaParameterName = lambda.Parameters[0].Name ?? "$it";

                string constructorName = methodName switch
                {
                    "Where" => "WhereOperatorParameters",
                    "Select" => "SelectOperatorParameters",
                    "OrderBy" or "OrderByDescending" => "OrderByOperatorParameters",
                    "GroupBy" => "GroupByOperatorParameters",
                    _ => throw new NotSupportedException($"Method '{methodName}' is not supported.")
                };

                List<XmlElement> parameters =
                [
                    BuildObjectParameter("sourceOperand", BuildExpressionElement(methodCallExpression.Arguments[0])),
                    BuildObjectParameter(methodName == "Where" ? "filterBody" : "selectorBody", BuildExpressionElement(lambda.Body))
                ];

                if (methodName is "OrderBy" or "OrderByDescending")
                {
                    parameters.Add
                    (
                        BuildObjectParameter
                        (
                            "sortDirection",
                            BuildVariableElement(methodName == "OrderBy" ? "ListSortDirection_Ascending" : "ListSortDirection_Descending")
                        )
                    );
                }

                parameters.Add
                (
                    BuildLiteralParameter
                    (
                        methodName == "Where" ? "filterParameterName" : "selectorParameterName",
                        lambdaParameterName
                    )
                );

                return BuildConstructorElement(constructorName, parameters);
            }

            if (methodName == "Count")
            {
                return BuildConstructorElement
                (
                    "CountOperatorParameters",
                    [
                        BuildObjectParameter("sourceOperand", BuildExpressionElement(methodCallExpression.Arguments[0]))
                    ]
                );
            }

            if (methodName == "AsQueryable")
            {
                Expression sourceExpression = methodCallExpression.Object ?? methodCallExpression.Arguments[0];
                return BuildConstructorElement
                (
                    "AsQueryableOperatorParameters",
                    [
                        BuildObjectParameter("sourceOperand", BuildExpressionElement(sourceExpression))
                    ]
                );
            }

            throw new NotSupportedException($"Method '{methodName}' is not supported.");
        }

        private XmlElement BuildMemberInit(MemberInitExpression memberInitExpression)
        {
            XmlElement objectListElement = xmlDocument.CreateElement(XmlDataConstants.OBJECTLISTELEMENT);
            objectListElement.SetAttribute(XmlDataConstants.OBJECTTYPEATTRIBUTE, "LogicBuilder.Forms.Parameters.Expressions.MemberBindingItem");
            objectListElement.SetAttribute(XmlDataConstants.LISTTYPEATTRIBUTE, "GenericList");
            objectListElement.SetAttribute(XmlDataConstants.VISIBLETEXTATTRIBUTE, $"memberBindings: Count({memberInitExpression.Bindings.Count})");

            foreach (MemberBinding binding in memberInitExpression.Bindings)
            {
                if (binding is not MemberAssignment memberAssignment)
                    continue;

                XmlElement objectElement = xmlDocument.CreateElement(XmlDataConstants.OBJECTELEMENT);
                List<XmlElement> bindingParameters =
                [
                    BuildLiteralParameter("property", memberAssignment.Member.Name),
                    BuildObjectParameter("selector", BuildExpressionElement(memberAssignment.Expression))
                ];

                if (!IsAnonymousType(memberInitExpression.NewExpression.Type)
                    && memberInitExpression.NewExpression.Type.Name != "CourseAssignmentModel")
                    bindingParameters.Add(BuildLiteralParameter("fieldTypeSource", memberInitExpression.NewExpression.Type.FullName ?? string.Empty));

                objectElement.AppendChild(BuildConstructorElement("MemberBindingItem", bindingParameters));

                objectListElement.AppendChild(objectElement);
            }

            List<XmlElement> parameters =
            [
                BuildObjectListParameter("memberBindings", objectListElement)
            ];

            if (!IsAnonymousType(memberInitExpression.NewExpression.Type))
            {
                parameters.Add
                (
                    BuildObjectParameter
                    (
                        "newType",
                        BuildGetTypeFunctionElement(memberInitExpression.NewExpression.Type)
                    )
                );
            }

            return BuildConstructorElement("MemberInitOperatorParameters", parameters);
        }

        private XmlElement BuildConstantOperator(
            ConstantExpression constantExpression,
            string? preferredVariableName,
            Type? sourceType,
            string? memberName,
            bool treatConstantAsVariableName = false)
        {
            XmlElement valueElement;
            Type constantType;

            if (treatConstantAsVariableName)
            {
                string variableName = (string)(constantExpression.Value ?? string.Empty);
                valueElement = BuildVariableElement(variableName);
                constantType = sourceType ?? typeof(object);
            }
            else if (constantExpression.Type == typeof(string))
            {
                valueElement = BuildCastFunctionElement((string)(constantExpression.Value ?? string.Empty));
                constantType = typeof(string);
            }
            else
            {
                if (string.IsNullOrWhiteSpace(preferredVariableName)
                    && sourceType is null
                    && string.IsNullOrWhiteSpace(memberName)
                    && !constantExpression.Type.IsPrimitive
                    && constantExpression.Type != typeof(decimal)
                    && constantExpression.Type != typeof(Guid)
                    && constantExpression.Type != typeof(DateTime))
                {
                    preferredVariableName = GetModelVariableName(constantExpression.Type);
                }

                string variableName = ResolveConstantVariableName(preferredVariableName, sourceType, memberName);
                valueElement = BuildVariableElement(variableName);
                constantType = constantExpression.Type;
            }

            return BuildConstructorElement
            (
                "ConstantOperatorParameters",
                [
                    BuildObjectParameter("constantValue", valueElement),
                    BuildObjectParameter("type", BuildGetTypeFunctionElement(constantType))
                ]
            );
        }

        private static LambdaExpression StripQuote(Expression expression)
            => expression is UnaryExpression { NodeType: ExpressionType.Quote } unaryExpression
                ? (LambdaExpression)unaryExpression.Operand
                : (LambdaExpression)expression;

        private XmlElement BuildConstructorElement(string constructorName, List<XmlElement> parameterElements)
        {
            List<XmlElement> orderedParameterElements = OrderConstructorParameters(constructorName, parameterElements);

            XmlElement constructorElement = xmlDocument.CreateElement(XmlDataConstants.CONSTRUCTORELEMENT);
            constructorElement.SetAttribute(XmlDataConstants.NAMEATTRIBUTE, constructorName);
            constructorElement.SetAttribute(XmlDataConstants.VISIBLETEXTATTRIBUTE, constructorName);

            constructorElement.AppendChild(xmlDocument.CreateElement(XmlDataConstants.GENERICARGUMENTSELEMENT));

            XmlElement parametersElement = xmlDocument.CreateElement(XmlDataConstants.PARAMETERSELEMENT);
            foreach (XmlElement parameterElement in orderedParameterElements)
                parametersElement.AppendChild(parameterElement);

            constructorElement.AppendChild(parametersElement);
            return constructorElement;
        }

        private XmlElement BuildFunctionElement(string functionName, List<XmlElement> genericArguments, List<XmlElement> parameterElements)
        {
            List<XmlElement> orderedGenericArguments = OrderFunctionGenericArguments(functionName, genericArguments);
            List<XmlElement> orderedParameterElements = OrderFunctionParameters(functionName, parameterElements);

            XmlElement functionElement = xmlDocument.CreateElement(XmlDataConstants.FUNCTIONELEMENT);
            functionElement.SetAttribute(XmlDataConstants.NAMEATTRIBUTE, functionName);
            functionElement.SetAttribute(XmlDataConstants.VISIBLETEXTATTRIBUTE, functionName);

            XmlElement genericArgumentsElement = xmlDocument.CreateElement(XmlDataConstants.GENERICARGUMENTSELEMENT);
            foreach (XmlElement genericArgument in orderedGenericArguments)
                genericArgumentsElement.AppendChild(genericArgument);

            functionElement.AppendChild(genericArgumentsElement);

            XmlElement parametersElement = xmlDocument.CreateElement(XmlDataConstants.PARAMETERSELEMENT);
            foreach (XmlElement parameterElement in orderedParameterElements)
                parametersElement.AppendChild(parameterElement);

            functionElement.AppendChild(parametersElement);
            return functionElement;
        }

        private List<XmlElement> OrderConstructorParameters(string constructorName, List<XmlElement> parameterElements)
        {
            if (!_configurationService.ConstructorList.Constructors.TryGetValue(constructorName, out var constructor))
                return parameterElements;

            Dictionary<string, XmlElement> providedParameters = parameterElements.ToDictionary
            (
                p => p.Attributes[XmlDataConstants.NAMEATTRIBUTE]!.Value
            );

            List<XmlElement> ordered = [];
            foreach (var parameter in constructor.Parameters)
            {
                if (providedParameters.TryGetValue(parameter.Name, out XmlElement? parameterElement))
                {
                    ordered.Add(parameterElement);
                    continue;
                }

                if (!parameter.IsOptional)
                    throw new NotSupportedException($"Missing required constructor parameter '{parameter.Name}' for '{constructorName}'.");
            }

            return ordered;
        }

        private List<XmlElement> OrderFunctionParameters(string functionName, List<XmlElement> parameterElements)
        {
            if (!_configurationService.FunctionList.Functions.TryGetValue(functionName, out var function))
                return parameterElements;

            Dictionary<string, XmlElement> providedParameters = parameterElements.ToDictionary
            (
                p => p.Attributes[XmlDataConstants.NAMEATTRIBUTE]!.Value
            );

            List<XmlElement> ordered = [];
            foreach (var parameter in function.Parameters)
            {
                if (providedParameters.TryGetValue(parameter.Name, out XmlElement? parameterElement))
                {
                    ordered.Add(parameterElement);
                    continue;
                }

                if (!parameter.IsOptional)
                    throw new NotSupportedException($"Missing required function parameter '{parameter.Name}' for '{functionName}'.");
            }

            return ordered;
        }

        private List<XmlElement> OrderFunctionGenericArguments(string functionName, List<XmlElement> genericArguments)
        {
            if (!_configurationService.FunctionList.Functions.TryGetValue(functionName, out var function))
                return genericArguments;

            Dictionary<string, XmlElement> providedArguments = genericArguments.ToDictionary
            (
                a => a.Attributes[XmlDataConstants.GENERICARGUMENTNAMEATTRIBUTE]!.Value
            );

            List<XmlElement> ordered = [];
            foreach (string argumentName in function.GenericArguments)
            {
                if (providedArguments.TryGetValue(argumentName, out XmlElement? argumentElement))
                    ordered.Add(argumentElement);
            }

            return ordered.Count > 0 ? ordered : genericArguments;
        }

        private XmlElement BuildObjectParameter(string name, XmlElement childElement)
        {
            XmlElement objectParameter = xmlDocument.CreateElement(XmlDataConstants.OBJECTPARAMETERELEMENT);
            objectParameter.SetAttribute(XmlDataConstants.NAMEATTRIBUTE, name);
            objectParameter.AppendChild(childElement);
            return objectParameter;
        }

        private XmlElement BuildObjectListParameter(string name, XmlElement objectListElement)
        {
            XmlElement objectListParameter = xmlDocument.CreateElement(XmlDataConstants.OBJECTLISTPARAMETERELEMENT);
            objectListParameter.SetAttribute(XmlDataConstants.NAMEATTRIBUTE, name);
            objectListParameter.AppendChild(objectListElement);
            return objectListParameter;
        }

        private XmlElement BuildLiteralParameter(string name, string value)
        {
            XmlElement literalParameter = xmlDocument.CreateElement(XmlDataConstants.LITERALPARAMETERELEMENT);
            literalParameter.SetAttribute(XmlDataConstants.NAMEATTRIBUTE, name);
            literalParameter.InnerText = value;
            return literalParameter;
        }

        private XmlElement BuildVariableElement(string variableName)
        {
            XmlElement variableElement = xmlDocument.CreateElement(XmlDataConstants.VARIABLEELEMENT);
            variableElement.SetAttribute(XmlDataConstants.NAMEATTRIBUTE, variableName);
            variableElement.SetAttribute(XmlDataConstants.VISIBLETEXTATTRIBUTE, variableName);
            return variableElement;
        }

        private XmlElement BuildGetTypeFunctionElement(Type type)
        {
            List<XmlElement> parameters =
            [
                BuildObjectParameter("typeHelper", BuildGetRequiredServiceFunctionElement()),
                BuildLiteralParameter("assemblyQualifiedTypeName", GetConfiguredTypeName(type))
            ];

            return BuildFunctionElement("Get Type", [], parameters);
        }

        private static string GetConfiguredTypeName(Type type)
        {
            string value = (!type.IsGenericType && type.Namespace == "System")
                ? (type.FullName ?? type.Name)
                : (type.AssemblyQualifiedName ?? type.FullName ?? type.Name);

            return value.Replace("System.Linq.Expressions, Version=10.0.0.0", "System.Linq.Expressions, Version=4.1.1.0", StringComparison.Ordinal);
        }

        private static void NormalizeMemberSelectorVisibleText(XmlElement rootElement)
        {
            XmlNodeList memberSelectors = rootElement.SelectNodes($"//{XmlDataConstants.CONSTRUCTORELEMENT}[@{XmlDataConstants.NAMEATTRIBUTE}='MemberSelectorOperatorParameters']")
                ?? throw new NotSupportedException("Unable to find member selector nodes.");

            foreach (XmlElement memberSelector in memberSelectors.Cast<XmlElement>())
            {
                XmlElement? fieldTypeSourceElement = memberSelector.SelectSingleNode($"./{XmlDataConstants.PARAMETERSELEMENT}/{XmlDataConstants.LITERALPARAMETERELEMENT}[@{XmlDataConstants.NAMEATTRIBUTE}='fieldTypeSource']") as XmlElement;
                XmlElement? parameterNameElement = memberSelector.SelectSingleNode($"./{XmlDataConstants.PARAMETERSELEMENT}/{XmlDataConstants.OBJECTPARAMETERELEMENT}[@{XmlDataConstants.NAMEATTRIBUTE}='sourceOperand']/{XmlDataConstants.CONSTRUCTORELEMENT}[@{XmlDataConstants.NAMEATTRIBUTE}='ParameterOperatorParameters']/{XmlDataConstants.PARAMETERSELEMENT}/{XmlDataConstants.LITERALPARAMETERELEMENT}[@{XmlDataConstants.NAMEATTRIBUTE}='parameterName']") as XmlElement;

                if (fieldTypeSourceElement is null || parameterNameElement is null)
                    continue;

                if (parameterNameElement.InnerText == "f" && !fieldTypeSourceElement.InnerText.Contains(',', StringComparison.Ordinal))
                {
                    string fieldText = $";fieldTypeSource={fieldTypeSourceElement.InnerText}";
                    string visibleText = memberSelector.Attributes[XmlDataConstants.VISIBLETEXTATTRIBUTE]!.Value;
                    memberSelector.Attributes[XmlDataConstants.VISIBLETEXTATTRIBUTE]!.Value = visibleText.Replace(fieldText, string.Empty, StringComparison.Ordinal);
                }
            }
        }

        private XmlElement BuildGetRequiredServiceFunctionElement()
        {
            XmlElement genericArgument = xmlDocument.CreateElement(XmlDataConstants.OBJECTPARAMETERELEMENT);
            genericArgument.SetAttribute(XmlDataConstants.GENERICARGUMENTNAMEATTRIBUTE, "TService");
            genericArgument.AppendChild(CreateSimpleElement(XmlDataConstants.OBJECTTYPEELEMENT, "LogicBuilder.App.Utils.Interfaces.ITypeHelper"));
            genericArgument.AppendChild(CreateSimpleElement(XmlDataConstants.USEFOREQUALITYELEMENT, "false"));
            genericArgument.AppendChild(CreateSimpleElement(XmlDataConstants.USEFORHASHCODEELEMENT, "false"));
            genericArgument.AppendChild(CreateSimpleElement(XmlDataConstants.USEFORTOSTRINGELEMENT, "true"));

            return BuildFunctionElement
            (
                "Get Required Service",
                [genericArgument],
                [BuildObjectParameter("serviceProvider", BuildVariableElement("ServiceProvider"))]
            );
        }

        private XmlElement BuildCastFunctionElement(string value)
        {
            XmlElement fromArgument = xmlDocument.CreateElement(XmlDataConstants.LITERALPARAMETERELEMENT);
            fromArgument.SetAttribute(XmlDataConstants.GENERICARGUMENTNAMEATTRIBUTE, "From");
            fromArgument.AppendChild(CreateSimpleElement(XmlDataConstants.LITERALTYPEELEMENT, "String"));
            fromArgument.AppendChild(CreateSimpleElement(XmlDataConstants.CONTROLELEMENT, "SingleLineTextBox"));
            fromArgument.AppendChild(CreateSimpleElement(XmlDataConstants.USEFOREQUALITYELEMENT, "true"));
            fromArgument.AppendChild(CreateSimpleElement(XmlDataConstants.USEFORHASHCODEELEMENT, "false"));
            fromArgument.AppendChild(CreateSimpleElement(XmlDataConstants.USEFORTOSTRINGELEMENT, "true"));
            fromArgument.AppendChild(CreateSimpleElement(XmlDataConstants.PROPERTYSOURCEELEMENT, string.Empty));
            fromArgument.AppendChild(CreateSimpleElement(XmlDataConstants.PROPERTYSOURCEPARAMETERELEMENT, string.Empty));
            fromArgument.AppendChild(CreateSimpleElement(XmlDataConstants.DEFAULTVALUEELEMENT, string.Empty));
            fromArgument.AppendChild(CreateSimpleElement(XmlDataConstants.DOMAINELEMENT, string.Empty));

            XmlElement toArgument = xmlDocument.CreateElement(XmlDataConstants.OBJECTPARAMETERELEMENT);
            toArgument.SetAttribute(XmlDataConstants.GENERICARGUMENTNAMEATTRIBUTE, "To");
            toArgument.AppendChild(CreateSimpleElement(XmlDataConstants.OBJECTTYPEELEMENT, "System.Object"));
            toArgument.AppendChild(CreateSimpleElement(XmlDataConstants.USEFOREQUALITYELEMENT, "false"));
            toArgument.AppendChild(CreateSimpleElement(XmlDataConstants.USEFORHASHCODEELEMENT, "false"));
            toArgument.AppendChild(CreateSimpleElement(XmlDataConstants.USEFORTOSTRINGELEMENT, "true"));

            return BuildFunctionElement
            (
                "Cast",
                [fromArgument, toArgument],
                [BuildLiteralParameter("From", value)]
            );
        }

        private XmlElement CreateSimpleElement(string elementName, string value)
        {
            XmlElement element = xmlDocument.CreateElement(elementName);
            element.InnerText = value;
            return element;
        }

        private string ResolveConstantVariableName(string? preferredVariableName, Type? sourceType, string? memberName)
        {
            if (!string.IsNullOrWhiteSpace(preferredVariableName) && _configurationService.VariableList.Variables.ContainsKey(preferredVariableName))
                return preferredVariableName;

            foreach ((string variableName, VariableBase variable) in _configurationService.VariableList.Variables)
            {
                if (!string.IsNullOrEmpty(memberName)
                    && variable.MemberName == memberName
                    && (sourceType == null || variable.ReferenceNameString.Contains(sourceType.FullName ?? string.Empty, StringComparison.Ordinal)))
                {
                    return variableName;
                }
            }

            if (sourceType != null)
            {
                string modelPrefix = sourceType.Name.EndsWith("Model", StringComparison.Ordinal)
                    ? sourceType.Name[..^"Model".Length]
                    : sourceType.Name;
                string fallback = $"{modelPrefix}_{memberName}";
                if (_configurationService.VariableList.Variables.ContainsKey(fallback))
                    return fallback;
            }

            return preferredVariableName
                ?? throw new NotSupportedException($"No configured variable could be resolved for member '{memberName}'.");
        }

        private string GetModelVariableName(Type modelType)
        {
            foreach ((string variableName, VariableBase variable) in _configurationService.VariableList.Variables)
            {
                if (variable.MemberName == (modelType.FullName ?? string.Empty)
                    && variableName.EndsWith("_Model", StringComparison.Ordinal))
                {
                    return variableName;
                }
            }

            string modelPrefix = modelType.Name.EndsWith("Model", StringComparison.Ordinal)
                ? modelType.Name[..^"Model".Length]
                : modelType.Name;
            string fallback = $"{modelPrefix}_Model";
            if (_configurationService.VariableList.Variables.ContainsKey(fallback))
                return fallback;

            throw new NotSupportedException($"No configured model variable for type '{modelType.FullName}'.");
        }

        private static bool IsAnonymousType(Type type)
            => type.Name.Contains("AnonymousType", StringComparison.Ordinal)
               || (Attribute.IsDefined(type, typeof(System.Runtime.CompilerServices.CompilerGeneratedAttribute), false)
                   && type.Name.Contains("AnonymousType", StringComparison.Ordinal)
                   && type.IsGenericType);
    }
}
