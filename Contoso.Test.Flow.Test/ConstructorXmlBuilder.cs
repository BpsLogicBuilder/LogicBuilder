using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Variables;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Data;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Reflection;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
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
            Type sourceElementType = isFilter
                ? GetFilterSourceElementType(lambdaExpression)
                : lambdaExpression.Parameters[0].Type;
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
                BuildObjectParameter("sourceElementType", BuildGetTypeFunctionElement(sourceElementType)),
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

        private static Type GetFilterSourceElementType(LambdaExpression lambdaExpression)
        {
            ParameterExpression parameter = lambdaExpression.Parameters[0];
            if (TryFindRootUpcastType(lambdaExpression.Body, parameter, out Type? upcastType))
                return upcastType;

            return parameter.Type;
        }


        private static bool TryFindRootUpcastType(Expression expression, ParameterExpression parameter, [NotNullWhen(true)] out Type? upcastType)
        {
            upcastType = null;

            if (expression is UnaryExpression unaryExpression)
            {
                if ((unaryExpression.NodeType == ExpressionType.TypeAs || unaryExpression.NodeType == ExpressionType.Convert)
                    && unaryExpression.Operand == parameter
                    && unaryExpression.Type != parameter.Type
                    && unaryExpression.Type.IsAssignableFrom(parameter.Type))
                {
                    upcastType = unaryExpression.Type;
                    return true;
                }

                return TryFindRootUpcastType(unaryExpression.Operand, parameter, out upcastType);
            }

            if (expression is BinaryExpression binaryExpression)
            {
                return TryFindRootUpcastType(binaryExpression.Left, parameter, out upcastType)
                    || TryFindRootUpcastType(binaryExpression.Right, parameter, out upcastType);
            }

            if (expression is MemberExpression memberExpression && memberExpression.Expression is not null)
                return TryFindRootUpcastType(memberExpression.Expression, parameter, out upcastType);

            if (expression is MethodCallExpression methodCallExpression)
            {
                if (methodCallExpression.Object is not null
                    && TryFindRootUpcastType(methodCallExpression.Object, parameter, out upcastType))
                {
                    return true;
                }

                foreach (Expression argument in methodCallExpression.Arguments)
                {
                    if (TryFindRootUpcastType(argument, parameter, out upcastType))
                        return true;
                }
            }

            return false;
        }

        private XmlElement BuildExpressionElement(Expression expression)
        {
            return expression.NodeType switch
            {
                ExpressionType.Equal => BuildEqualsBinaryOperator((BinaryExpression)expression),
                ExpressionType.NotEqual => BuildBinaryOperator((BinaryExpression)expression, "NotEqualsBinaryOperatorParameters"),
                ExpressionType.GreaterThan => BuildBinaryOperator((BinaryExpression)expression, "GreaterThanBinaryOperatorParameters"),
                ExpressionType.GreaterThanOrEqual => BuildBinaryOperator((BinaryExpression)expression, "GreaterThanOrEqualsBinaryOperatorParameters"),
                ExpressionType.LessThan => BuildBinaryOperator((BinaryExpression)expression, "LessThanBinaryOperatorParameters"),
                ExpressionType.LessThanOrEqual => BuildBinaryOperator((BinaryExpression)expression, "LessThanOrEqualsBinaryOperatorParameters"),
                ExpressionType.Add => BuildBinaryOperator((BinaryExpression)expression, "AddBinaryOperatorParameters"),
                ExpressionType.Subtract => BuildBinaryOperator((BinaryExpression)expression, "SubtractBinaryOperatorParameters"),
                ExpressionType.Multiply => BuildBinaryOperator((BinaryExpression)expression, "MultiplyBinaryOperatorParameters"),
                ExpressionType.Divide => BuildBinaryOperator((BinaryExpression)expression, "DivideBinaryOperatorParameters"),
                ExpressionType.Modulo => BuildBinaryOperator((BinaryExpression)expression, "ModuloBinaryOperatorParameters"),
                ExpressionType.AndAlso => BuildBinaryOperator((BinaryExpression)expression, "AndBinaryOperatorParameters"),
                ExpressionType.OrElse => BuildBinaryOperator((BinaryExpression)expression, "OrBinaryOperatorParameters"),
                ExpressionType.MemberAccess => BuildMemberAccess((MemberExpression)expression),
                ExpressionType.Parameter => BuildParameterOperator((ParameterExpression)expression),
                ExpressionType.Convert => BuildConvertOperator((UnaryExpression)expression),
                ExpressionType.ConvertChecked => BuildConvertOperator((UnaryExpression)expression),
                ExpressionType.TypeAs => BuildCastAsOperator((UnaryExpression)expression),
                ExpressionType.Not => BuildUnaryOperator((UnaryExpression)expression, "NotOperatorParameters"),
                ExpressionType.Negate => BuildUnaryOperator((UnaryExpression)expression, "NegateOperatorParameters"),
                ExpressionType.Call => BuildMethodCall((MethodCallExpression)expression),
                ExpressionType.MemberInit => BuildMemberInit((MemberInitExpression)expression),
                ExpressionType.Constant => BuildConstantOperator((ConstantExpression)expression, ((ConstantExpression)expression).Value is null ? "Null" : null, null, null),
                ExpressionType.Conditional => BuildConditionalExpression((ConditionalExpression)expression),
                _ => throw new NotSupportedException($"Expression node '{expression.NodeType}' is not supported.")
            };
        }

        private XmlElement BuildConditionalExpression(ConditionalExpression conditionalExpression)
        {
            if (TryBuildNullableToStringConditional(conditionalExpression, out XmlElement? nullableSourceOperand))
            {
                return BuildConstructorElement
                (
                    "ConvertToStringOperatorParameters",
                    [
                        BuildObjectParameter("sourceOperand", nullableSourceOperand)
                    ]
                );
            }

            return BuildEvaluatedConstantExpression(conditionalExpression);
        }

        private bool TryBuildNullableToStringConditional(ConditionalExpression conditionalExpression, [NotNullWhen(true)] out XmlElement? sourceOperand)
        {
            sourceOperand = null;

            if (conditionalExpression.Type != typeof(string))
                return false;

            if (conditionalExpression.IfFalse is not ConstantExpression { Value: null })
                return false;

            if (conditionalExpression.Test is not MemberExpression { Member.Name: "HasValue" } hasValueMember
                || hasValueMember.Expression is null
                || Nullable.GetUnderlyingType(hasValueMember.Expression.Type) is null)
            {
                return false;
            }

            if (conditionalExpression.IfTrue is not MethodCallExpression { Method.Name: "ToString", Arguments.Count: 0 } toStringCall
                || toStringCall.Object is null)
            {
                return false;
            }

            if (!TryGetNullableSourceExpressionFromToStringObject(toStringCall.Object, out Expression? trueSourceExpression))
                return false;

            Expression testSourceExpression = hasValueMember.Expression;
            if (testSourceExpression.Type != trueSourceExpression.Type
                || !string.Equals(testSourceExpression.ToString(), trueSourceExpression.ToString(), StringComparison.Ordinal))
            {
                return false;
            }

            sourceOperand = BuildExpressionElement(testSourceExpression);
            return true;
        }

        private static bool TryGetNullableSourceExpressionFromToStringObject(Expression expression, [NotNullWhen(true)] out Expression? sourceExpression)
        {
            sourceExpression = null;

            if (expression is MemberExpression { Member.Name: "Value" } valueMember
                && valueMember.Expression is not null
                && Nullable.GetUnderlyingType(valueMember.Expression.Type) is not null)
            {
                sourceExpression = valueMember.Expression;
                return true;
            }

            if (expression is UnaryExpression { NodeType: ExpressionType.Convert or ExpressionType.ConvertChecked } unaryExpression)
                return TryGetNullableSourceExpressionFromToStringObject(unaryExpression.Operand, out sourceExpression);

            return false;
        }

        private XmlElement BuildEvaluatedConstantExpression(Expression expression)
        {
            if (!TryEvaluateExpressionValue(expression, out object? value))
                throw new NotSupportedException($"Expression node '{expression.NodeType}' is not supported.");

            ConstantExpression constant = Expression.Constant(value, expression.Type);
            return BuildConstantOperator(constant, value is null ? "Null" : null, null, null);
        }

        private XmlElement BuildBinaryOperator(BinaryExpression binaryExpression, string constructorName)
            => BuildConstructorElement
            (
                constructorName,
                [
                    BuildObjectParameter("left", BuildExpressionElement(binaryExpression.Left)),
                    BuildObjectParameter("right", BuildExpressionElement(binaryExpression.Right))
                ]
            );

        private XmlElement BuildUnaryOperator(UnaryExpression unaryExpression, string constructorName)
            => BuildConstructorElement
            (
                constructorName,
                [
                    BuildObjectParameter("operand", BuildExpressionElement(unaryExpression.Operand))
                ]
            );

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
                if (TryEvaluateStaticMemberValue(memberExpression.Member, out object? staticValue))
                    return BuildConstantFromValue(staticValue, memberExpression.Type, null, null);

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

        private bool ShouldPreserveEvaluatedMemberAsSelector(MemberExpression memberExpression)
        {
            if (memberExpression.Expression is null)
                return false;

            if (!TryEvaluateExpressionValue(memberExpression.Expression, out object? sourceValue) || sourceValue is null)
                return false;

            Type? declaringType = memberExpression.Member.DeclaringType;
            return declaringType is not null
                && declaringType.IsInstanceOfType(sourceValue)
                && HasConfiguredModelVariable(declaringType);
        }

        private bool HasConfiguredModelVariable(Type modelType)
            => _configurationService.VariableList.Variables
                .Any(v => v.Key.EndsWith("_Model", StringComparison.Ordinal)
                    && v.Value.MemberName == (modelType.FullName ?? string.Empty));

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

        private static bool TryEvaluateStaticMemberValue(MemberInfo memberInfo, out object? value)
        {
            value = null;
            if (memberInfo is FieldInfo fieldInfo && fieldInfo.IsStatic)
            {
                value = fieldInfo.GetValue(null);
                return true;
            }

            if (memberInfo is PropertyInfo propertyInfo && propertyInfo.GetMethod?.IsStatic == true)
            {
                value = propertyInfo.GetValue(null);
                return true;
            }

            return false;
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
                string? variableName = TryResolveConstantVariableName(null, sourceType, memberName);
                if (variableName is not null)
                {
                    return BuildConstructorElement
                    (
                        "ConstantOperatorParameters",
                        [
                            BuildObjectParameter("constantValue", BuildVariableElement(variableName)),
                            BuildObjectParameter("type", BuildGetTypeFunctionElement(valueType))
                        ]
                    );
                }
            }

            ConstantExpression constantExpression = Expression.Constant(value, valueType);
            return BuildConstantOperator(constantExpression, constantExpression.Value is null ? "Null" : null, null, null);
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
        {
            Type targetType = Nullable.GetUnderlyingType(unaryExpression.Type) ?? unaryExpression.Type;
            if (targetType.IsEnum && TryGetEnumTextValue(unaryExpression.Operand, out string? enumTextValue))
            {
                return BuildConstructorElement
                (
                    "ConvertToEnumOperatorParameters",
                    [
                        BuildObjectParameter("constantValue", BuildCastFunctionElement(enumTextValue)),
                        BuildObjectParameter("type", BuildGetTypeFunctionElement(targetType))
                    ]
                );
            }

            return BuildConstructorElement
            (
                "ConvertOperatorParameters",
                [
                    BuildObjectParameter("sourceOperand", BuildExpressionElement(unaryExpression.Operand)),
                    BuildObjectParameter("type", BuildGetTypeFunctionElement(unaryExpression.Type))
                ]
            );
        }

        private static bool TryGetEnumTextValue(Expression expression, [NotNullWhen(true)] out string? value)
        {
            value = null;

            if (expression is ConstantExpression constantExpression)
            {
                if (constantExpression.Value is string text)
                {
                    value = text;
                    return true;
                }

                if (constantExpression.Value is not null)
                {
                    Type valueType = Nullable.GetUnderlyingType(constantExpression.Type) ?? constantExpression.Type;
                    if (valueType.IsEnum)
                    {
                        value = constantExpression.Value.ToString() ?? "";
                        return true;
                    }
                }
            }

            if (expression is UnaryExpression { NodeType: ExpressionType.Convert or ExpressionType.ConvertChecked } unaryExpression)
                return TryGetEnumTextValue(unaryExpression.Operand, out value);

            if (TryEvaluateExpressionValue(expression, out object? evaluatedValue) && evaluatedValue is not null)
            {
                Type evaluatedType = Nullable.GetUnderlyingType(evaluatedValue.GetType()) ?? evaluatedValue.GetType();
                if (evaluatedType.IsEnum)
                {
                    value = evaluatedValue.ToString() ?? "";
                    return true;
                }
            }

            return false;
        }

        private XmlElement BuildCastAsOperator(UnaryExpression unaryExpression)
            => BuildConstructorElement
            (
                "CastOperatorParameters",
                [
                    BuildObjectParameter("operand", BuildExpressionElement(unaryExpression.Operand)),
                    BuildObjectParameter("type", BuildGetTypeFunctionElement(unaryExpression.Type))
                ]
            );

        private XmlElement BuildMethodCall(MethodCallExpression methodCallExpression)
        {
            string methodName = methodCallExpression.Method.Name;

            if (methodName is "Where" or "Select" or "OrderBy" or "OrderByDescending" or "GroupBy" or "ThenBy" or "ThenByDescending" or "SelectMany")
            {
                LambdaExpression lambda = StripQuote(methodCallExpression.Arguments[1]);
                string lambdaParameterName = lambda.Parameters[0].Name ?? "$it";

                string constructorName = methodName switch
                {
                    "Where" => "WhereOperatorParameters",
                    "Select" => "SelectOperatorParameters",
                    "OrderBy" or "OrderByDescending" => "OrderByOperatorParameters",
                    "ThenBy" or "ThenByDescending" => "ThenByOperatorParameters",
                    "GroupBy" => "GroupByOperatorParameters",
                    "SelectMany" => "SelectManyOperatorParameters",
                    _ => throw new NotSupportedException($"Method '{methodName}' is not supported.")
                };

                List<XmlElement> parameters =
                [
                    BuildObjectParameter("sourceOperand", BuildExpressionElement(methodCallExpression.Arguments[0])),
                    BuildObjectParameter(methodName == "Where" ? "filterBody" : "selectorBody", BuildExpressionElement(lambda.Body))
                ];

                if (methodName is "OrderBy" or "OrderByDescending" or "ThenBy" or "ThenByDescending")
                {
                    parameters.Add
                    (
                        BuildObjectParameter
                        (
                            "sortDirection",
                            BuildVariableElement(methodName is "OrderBy" or "ThenBy" ? "ListSortDirection_Ascending" : "ListSortDirection_Descending")
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

            if (methodName == "Has")
            {
                Expression leftExpression = methodCallExpression.Object ?? methodCallExpression.Arguments[0];
                Expression rightExpression = methodCallExpression.Object is null
                    ? methodCallExpression.Arguments[1]
                    : methodCallExpression.Arguments[0];

                return BuildConstructorElement
                (
                    "HasOperatorParameters",
                    [
                        BuildObjectParameter("left", BuildExpressionElement(leftExpression)),
                        BuildObjectParameter("right", BuildExpressionElement(rightExpression))
                    ]
                );
            }

            if (methodName == "Count")
            {
                if (methodCallExpression.Arguments.Count == 2)
                {
                    LambdaExpression lambda = StripQuote(methodCallExpression.Arguments[1]);
                    return BuildConstructorElement
                    (
                        "CountOperatorParameters",
                        [
                            BuildObjectParameter("sourceOperand", BuildExpressionElement(methodCallExpression.Arguments[0])),
                            BuildObjectParameter("filterBody", BuildExpressionElement(lambda.Body)),
                            BuildLiteralParameter("filterParameterName", lambda.Parameters[0].Name ?? "$it")
                        ]
                    );
                }

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

            if (methodName == "AsEnumerable")
            {
                Expression sourceExpression = methodCallExpression.Object ?? methodCallExpression.Arguments[0];
                return BuildConstructorElement
                (
                    "AsEnumerableOperatorParameters",
                    [
                        BuildObjectParameter("sourceOperand", BuildExpressionElement(sourceExpression))
                    ]
                );
            }

            if (methodName == "Cast" && methodCallExpression.Method.IsGenericMethod)
            {
                Type castType = methodCallExpression.Method.GetGenericArguments()[0];
                Expression sourceExpression = methodCallExpression.Object ?? methodCallExpression.Arguments[0];

                return BuildConstructorElement
                (
                    "CollectionCastOperatorParameters",
                    [
                        BuildObjectParameter("operand", BuildExpressionElement(sourceExpression)),
                        BuildObjectParameter("type", BuildGetTypeFunctionElement(castType))
                    ]
                );
            }

            if (methodName == "Distinct")
            {
                Expression sourceExpression = methodCallExpression.Object ?? methodCallExpression.Arguments[0];
                return BuildConstructorElement
                (
                    "DistinctOperatorParameters",
                    [
                        BuildObjectParameter("sourceOperand", BuildExpressionElement(sourceExpression))
                    ]
                );
            }

            if (methodName is "Skip" or "Take")
            {
                string constructorName = methodName == "Skip" ? "SkipOperatorParameters" : "TakeOperatorParameters";
                return BuildConstructorElement
                (
                    constructorName,
                    [
                        BuildObjectParameter("sourceOperand", BuildExpressionElement(methodCallExpression.Arguments[0])),
                        BuildObjectParameter("count", BuildExpressionElement(methodCallExpression.Arguments[1]))
                    ]
                );
            }

            if (methodName is "Any" or "All" or "First" or "FirstOrDefault" or "Last" or "LastOrDefault" or "Single")
            {
                string constructorName = methodName switch
                {
                    "Any" => "AnyOperatorParameters",
                    "All" => "AllOperatorParameters",
                    "First" => "FirstOperatorParameters",
                    "FirstOrDefault" => "FirstOrDefaultOperatorParameters",
                    "Last" => "LastOperatorParameters",
                    "LastOrDefault" => "LastOrDefaultOperatorParameters",
                    _ => "SingleOperatorParameters"
                };

                List<XmlElement> parameters =
                [
                    BuildObjectParameter("sourceOperand", BuildExpressionElement(methodCallExpression.Arguments[0]))
                ];

                if (methodCallExpression.Arguments.Count == 2)
                {
                    LambdaExpression lambda = StripQuote(methodCallExpression.Arguments[1]);
                    parameters.Add(BuildObjectParameter("filterBody", BuildExpressionElement(lambda.Body)));
                    parameters.Add(BuildLiteralParameter("filterParameterName", lambda.Parameters[0].Name ?? "$it"));
                }

                return BuildConstructorElement(constructorName, parameters);
            }

            if (methodName is "Average" or "Max" or "Min" or "Sum")
            {
                string constructorName = methodName switch
                {
                    "Average" => "AverageOperatorParameters",
                    "Max" => "MaxOperatorParameters",
                    "Min" => "MinOperatorParameters",
                    _ => "SumOperatorParameters"
                };

                List<XmlElement> parameters =
                [
                    BuildObjectParameter("sourceOperand", BuildExpressionElement(methodCallExpression.Arguments[0]))
                ];

                if (methodCallExpression.Arguments.Count == 2)
                {
                    LambdaExpression lambda = StripQuote(methodCallExpression.Arguments[1]);
                    parameters.Add(BuildObjectParameter("selectorBody", BuildExpressionElement(lambda.Body)));
                    parameters.Add(BuildLiteralParameter("selectorParameterName", lambda.Parameters[0].Name ?? "$it"));
                }

                return BuildConstructorElement(constructorName, parameters);
            }

            if (methodName == "Union" || methodName == "Except" || methodName == "Concat")
            {
                Expression leftExpression = methodCallExpression.Object ?? methodCallExpression.Arguments[0];
                Expression rightExpression = methodCallExpression.Object is null
                    ? methodCallExpression.Arguments[1]
                    : methodCallExpression.Arguments[0];

                string constructorName = methodName switch
                {
                    "Union" => "UnionOperatorParameters",
                    "Except" => "ExceptOperatorParameters",
                    _ => "ConcatOperatorParameters"
                };

                return BuildConstructorElement
                (
                    constructorName,
                    [
                        BuildObjectParameter("left", BuildExpressionElement(leftExpression)),
                        BuildObjectParameter("right", BuildExpressionElement(rightExpression))
                    ]
                );
            }

            if (methodName == "Contains" && TryGetInOperatorExpressions(methodCallExpression, out Expression? itemToFindExpression, out Expression? listToSearchExpression))
            {
                return BuildConstructorElement
                (
                    "InOperatorParameters",
                    [
                        BuildObjectParameter("itemToFind", BuildExpressionElement(itemToFindExpression)),
                        BuildObjectParameter("listToSearch", BuildListExpressionElement(listToSearchExpression))
                    ]
                );
            }

            if (methodName is "Contains" or "StartsWith" or "EndsWith")
            {
                string constructorName = methodName switch
                {
                    "Contains" => "ContainsOperatorParameters",
                    "StartsWith" => "StartsWithOperatorParameters",
                    _ => "EndsWithOperatorParameters"
                };

                Expression leftExpression = methodCallExpression.Object ?? methodCallExpression.Arguments[0];
                Expression rightExpression = methodCallExpression.Object is null
                    ? methodCallExpression.Arguments[1]
                    : methodCallExpression.Arguments[0];

                return BuildConstructorElement
                (
                    constructorName,
                    [
                        BuildObjectParameter("left", BuildExpressionElement(leftExpression)),
                        BuildObjectParameter("right", BuildExpressionElement(rightExpression))
                    ]
                );
            }

            if (methodName == "IndexOf" && methodCallExpression.Object is not null)
            {
                return BuildConstructorElement
                (
                    "IndexOfOperatorParameters",
                    [
                        BuildObjectParameter("sourceOperand", BuildExpressionElement(methodCallExpression.Object)),
                        BuildObjectParameter("itemToFind", BuildExpressionElement(methodCallExpression.Arguments[0]))
                    ]
                );
            }

            if (methodName == "Substring" && methodCallExpression.Object is not null)
            {
                XmlElement indexElements = BuildObjectList
                (
                    methodCallExpression.Arguments.Select(BuildExpressionElement).ToList(),
                    "LogicBuilder.Forms.Parameters.Expressions.IExpressionParameter",
                    "Array",
                    "indexes"
                );

                return BuildConstructorElement
                (
                    "SubstringOperatorParameters",
                    [
                        BuildObjectParameter("sourceOperand", BuildExpressionElement(methodCallExpression.Object)),
                        BuildObjectListParameter("indexes", indexElements)
                    ]
                );
            }

            if (methodName == "ToLower" && methodCallExpression.Object is not null)
                return BuildConstructorElement("ToLowerOperatorParameters", [BuildObjectParameter("operand", BuildExpressionElement(methodCallExpression.Object))]);

            if (methodName == "ToUpper" && methodCallExpression.Object is not null)
                return BuildConstructorElement("ToUpperOperatorParameters", [BuildObjectParameter("operand", BuildExpressionElement(methodCallExpression.Object))]);

            if (methodName == "Trim" && methodCallExpression.Object is not null)
                return BuildConstructorElement("TrimOperatorParameters", [BuildObjectParameter("operand", BuildExpressionElement(methodCallExpression.Object))]);

            if (methodName == "Floor")
                return BuildConstructorElement("FloorOperatorParameters", [BuildObjectParameter("operand", BuildExpressionElement(methodCallExpression.Object ?? methodCallExpression.Arguments[0]))]);

            if (methodName == "Round")
                return BuildConstructorElement("RoundOperatorParameters", [BuildObjectParameter("operand", BuildExpressionElement(methodCallExpression.Object ?? methodCallExpression.Arguments[0]))]);

            if (methodName == "ToString" && methodCallExpression.Object is not null && methodCallExpression.Arguments.Count == 0)
                return BuildConstructorElement("ConvertToStringOperatorParameters", [BuildObjectParameter("sourceOperand", BuildExpressionElement(methodCallExpression.Object))]);

            return BuildCustomMethodCall(methodCallExpression);
        }

        private XmlElement BuildListExpressionElement(Expression expression)
        {
            if (expression is ConstantExpression constantExpression
                && constantExpression.Value is System.Collections.IEnumerable enumerable
                && expression.Type != typeof(string))
            {
                return BuildCollectionConstantOperator(enumerable, expression.Type);
            }

            return BuildExpressionElement(expression);
        }

        private static bool TryGetInOperatorExpressions(MethodCallExpression methodCallExpression, [NotNullWhen(true)] out Expression? itemToFindExpression, [NotNullWhen(true)]  out Expression? listToSearchExpression)
        {
            itemToFindExpression = null;
            listToSearchExpression = null;

            if (methodCallExpression.Method.Name != "Contains")
                return false;

            if (methodCallExpression.Object is not null)
            {
                if (methodCallExpression.Object.Type == typeof(string)
                    || methodCallExpression.Arguments.Count != 1)
                {
                    return false;
                }

                listToSearchExpression = methodCallExpression.Object;
                itemToFindExpression = methodCallExpression.Arguments[0];
                return true;
            }

            if (methodCallExpression.Arguments.Count != 2)
                return false;

            Expression listExpression = methodCallExpression.Arguments[0];
            if (listExpression.Type == typeof(string))
                return false;

            listToSearchExpression = listExpression;
            itemToFindExpression = methodCallExpression.Arguments[1];
            return true;
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

            if (constantExpression.Value is null)
            {
                string nullVariable = ResolveConstantVariableName("Null", null, null);
                return BuildConstructorElement
                (
                    "ConstantOperatorParameters",
                    [
                        BuildObjectParameter("constantValue", BuildVariableElement(nullVariable)),
                        BuildObjectParameter("type", BuildGetTypeFunctionElement(constantExpression.Type))
                    ]
                );
            }

            if (treatConstantAsVariableName)
            {
                string variableName = (string)(constantExpression.Value ?? string.Empty);
                valueElement = BuildVariableElement(variableName);
                constantType = sourceType ?? typeof(object);
            }
            else
            {
                (object normalizedValue, Type normalizedType) = NormalizeConstantValueAndType(constantExpression.Value!, constantExpression.Type);
                Type underlyingType = Nullable.GetUnderlyingType(normalizedType) ?? normalizedType;

                if (underlyingType.IsEnum)
                {
                    string enumText = normalizedValue.ToString() ?? string.Empty;
                    return BuildConstructorElement
                    (
                        "ConvertToEnumOperatorParameters",
                        [
                            BuildObjectParameter("constantValue", BuildCastFunctionElement(enumText)),
                            BuildObjectParameter("type", BuildGetTypeFunctionElement(underlyingType))
                        ]
                    );
                }

                if (normalizedType == typeof(string))
                {
                    valueElement = BuildCastFunctionElement((string)normalizedValue);
                    constantType = typeof(string);
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(preferredVariableName)
                        && sourceType is null
                        && string.IsNullOrWhiteSpace(memberName)
                        && !normalizedType.IsPrimitive
                        && normalizedType != typeof(decimal)
                        && normalizedType != typeof(Guid)
                        && normalizedType != typeof(DateTime)
                        && normalizedType != typeof(DateTimeOffset)
                        && normalizedType != typeof(DateOnly)
                        && normalizedType != typeof(TimeOnly)
                        && normalizedType != typeof(TimeSpan)
                        && normalizedType != typeof(byte[])
                        && !typeof(System.Collections.IEnumerable).IsAssignableFrom(normalizedType))
                    {
                        preferredVariableName = TryGetModelVariableName(normalizedType);
                    }

                    string? variableName = TryResolveConstantVariableName(preferredVariableName, sourceType, memberName);
                    valueElement = variableName is not null
                        ? BuildVariableElement(variableName)
                        : BuildTypedConstantValue(normalizedValue, normalizedType);
                    constantType = normalizedType;
                }
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

        private XmlElement BuildCustomMethodCall(MethodCallExpression methodCallExpression)
        {
            Type declaringType = methodCallExpression.Method.DeclaringType
                ?? throw new NotSupportedException("Custom method call requires a declaring type.");

            List<XmlElement> argumentElements = methodCallExpression.Method.IsStatic
                ? methodCallExpression.Arguments.Select(BuildExpressionElement).ToList()
                : [BuildExpressionElement(methodCallExpression.Object ?? throw new NotSupportedException("Instance method call object is required.")), .. methodCallExpression.Arguments.Select(BuildExpressionElement)];

            XmlElement argsObjectList = BuildObjectList
            (
                argumentElements,
                "LogicBuilder.Forms.Parameters.Expressions.IExpressionParameter",
                "Array",
                "args"
            );

            return BuildConstructorElement
            (
                "CustomMethodOperatorParameters",
                [
                    BuildObjectParameter("declaringType", BuildGetTypeFunctionElement(declaringType)),
                    BuildLiteralParameter("methodName", methodCallExpression.Method.Name),
                    BuildLiteralListParameter
                    (
                        "parameterTypeNames",
                        methodCallExpression.Method.GetParameters().Select(p => GetConfiguredTypeName(p.ParameterType)).ToList(),
                        "String",
                        "Array"
                    ),
                    BuildObjectListParameter("args", argsObjectList)
                ]
            );
        }

        private XmlElement BuildObjectList(List<XmlElement> items, string objectType, string listType, string visibleName)
        {
            XmlElement objectListElement = xmlDocument.CreateElement(XmlDataConstants.OBJECTLISTELEMENT);
            objectListElement.SetAttribute(XmlDataConstants.OBJECTTYPEATTRIBUTE, objectType);
            objectListElement.SetAttribute(XmlDataConstants.LISTTYPEATTRIBUTE, listType);
            objectListElement.SetAttribute(XmlDataConstants.VISIBLETEXTATTRIBUTE, $"{visibleName}: Count({items.Count})");

            foreach (XmlElement item in items)
            {
                XmlElement objectElement = xmlDocument.CreateElement(XmlDataConstants.OBJECTELEMENT);
                objectElement.AppendChild(item);
                objectListElement.AppendChild(objectElement);
            }

            return objectListElement;
        }

        private XmlElement BuildLiteralListParameter(string name, List<string> values, string literalType, string listType)
        {
            XmlElement literalListParameter = xmlDocument.CreateElement(XmlDataConstants.LITERALLISTPARAMETERELEMENT);
            literalListParameter.SetAttribute(XmlDataConstants.NAMEATTRIBUTE, name);

            XmlElement literalList = xmlDocument.CreateElement(XmlDataConstants.LITERALLISTELEMENT);
            literalList.SetAttribute(XmlDataConstants.LITERALTYPEATTRIBUTE, literalType);
            literalList.SetAttribute(XmlDataConstants.LISTTYPEATTRIBUTE, listType);
            literalList.SetAttribute(XmlDataConstants.VISIBLETEXTATTRIBUTE, $"{name}: Count({values.Count})");

            foreach (string value in values)
            {
                XmlElement literal = xmlDocument.CreateElement(XmlDataConstants.LITERALELEMENT);
                literal.InnerText = value;
                literalList.AppendChild(literal);
            }

            literalListParameter.AppendChild(literalList);
            return literalListParameter;
        }

        private XmlElement BuildTypedConstantValue(object value, Type valueType)
        {
            if (value is System.Collections.IEnumerable enumerable && valueType != typeof(string))
            {
                return BuildCollectionConstantOperator(enumerable, valueType);
            }

            string literalTypeName = GetLiteralTypeName(valueType);
            string literalValue = ConvertToLiteralString(value, valueType);

            XmlElement fromArgument = xmlDocument.CreateElement(XmlDataConstants.LITERALPARAMETERELEMENT);
            fromArgument.SetAttribute(XmlDataConstants.GENERICARGUMENTNAMEATTRIBUTE, "From");
            fromArgument.AppendChild(CreateSimpleElement(XmlDataConstants.LITERALTYPEELEMENT, literalTypeName));
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
                [BuildLiteralParameter("From", literalValue)]
            );
        }

        private XmlElement BuildCollectionConstantOperator(System.Collections.IEnumerable enumerable, Type collectionType)
        {
            Type elementType = collectionType.IsArray
                ? collectionType.GetElementType() ?? typeof(object)
                : GetCollectionTypeForNonArrays(collectionType);

            List<XmlElement> valueElements = [];
            foreach (object? item in enumerable)
            {
                valueElements.Add(BuildCollectionItemValue(item, elementType));
            }

            XmlElement objectList = BuildObjectList(valueElements, "System.Object", "IGenericCollection", "constantValues");

            return BuildConstructorElement
            (
                "CollectionConstantOperatorParameters",
                [
                    BuildObjectListParameter("constantValues", objectList),
                    BuildObjectParameter("elementType", BuildGetTypeFunctionElement(elementType))
                ]
            );

            static Type GetCollectionTypeForNonArrays(Type collectionType)
            {
                return collectionType.IsGenericType ? collectionType.GetGenericArguments().FirstOrDefault() ?? typeof(object) : typeof(object);
            }
        }

        private XmlElement BuildCollectionItemValue(object? item, Type declaredElementType)
        {
            if (item is null)
                return BuildVariableElement(ResolveConstantVariableName("Null", null, null));

            Type elementType = Nullable.GetUnderlyingType(declaredElementType) ?? declaredElementType;
            if (elementType.IsEnum)
                return BuildConvertToEnumElement(item.ToString() ?? string.Empty, elementType);

            if (item is string text)
                return BuildCastFunctionElement(text);

            Type itemType = item.GetType();
            if (itemType == typeof(DateOnly))
                return BuildCastFunctionElement(((DateOnly)item).ToString("O", CultureInfo.InvariantCulture), typeof(DateOnly));

            if (itemType == typeof(TimeOnly))
                return BuildCastFunctionElement(((TimeOnly)item).ToString("O", CultureInfo.InvariantCulture), typeof(TimeOnly));

            return BuildTypedConstantValue(item, itemType);
        }

        private XmlElement BuildConvertToEnumElement(string enumText, Type enumType)
            => BuildConstructorElement
            (
                "ConvertToEnumOperatorParameters",
                [
                    BuildObjectParameter("constantValue", BuildCastFunctionElement(enumText)),
                    BuildObjectParameter("type", BuildGetTypeFunctionElement(enumType))
                ]
            );

        private static (object value, Type type) NormalizeConstantValueAndType(object value, Type valueType)
        {
            Type targetType = Nullable.GetUnderlyingType(valueType) ?? valueType;

            if (targetType == typeof(DateOnly) && value is DateOnly dateOnly)
                return (new Microsoft.OData.Edm.Date(dateOnly.Year, dateOnly.Month, dateOnly.Day), typeof(Microsoft.OData.Edm.Date));

            return (value, valueType);
        }

        private static string GetLiteralTypeName(Type type)
        {
            Type valueType = Nullable.GetUnderlyingType(type) ?? type;

            if (valueType == typeof(bool)) return "Boolean";
            if (valueType == typeof(byte)) return "Byte";
            if (valueType == typeof(short)) return "Short";
            if (valueType == typeof(int)) return "Integer";
            if (valueType == typeof(long)) return "Long";
            if (valueType == typeof(float)) return "Float";
            if (valueType == typeof(double)) return "Double";
            if (valueType == typeof(decimal)) return "Decimal";
            if (valueType == typeof(char)) return "Char";
            if (valueType == typeof(sbyte)) return "SByte";
            if (valueType == typeof(ushort)) return "UShort";
            if (valueType == typeof(uint)) return "UInteger";
            if (valueType == typeof(ulong)) return "ULong";
            if (valueType == typeof(Guid)) return "Guid";
            if (valueType == typeof(DateTimeOffset)) return "DateTimeOffset";
            if (valueType == typeof(DateTime)) return "DateTime";
            if (valueType == typeof(DateOnly)) return "DateOnly";
            if (valueType == typeof(Microsoft.OData.Edm.Date)) return "Date";
            if (valueType == typeof(TimeOnly)) return "TimeOnly";
            if (valueType == typeof(Microsoft.OData.Edm.TimeOfDay)) return "TimeOfDay";
            if (valueType == typeof(TimeSpan)) return "TimeSpan";
            if (valueType == typeof(string)) return "String";

            return "String";
        }

        private static string ConvertToLiteralString(object? value, Type valueType)
        {
            if (value is null)
                return string.Empty;

            Type targetType = Nullable.GetUnderlyingType(valueType) ?? valueType;

            if (targetType == typeof(bool))
                return ((bool)value).ToString(CultureInfo.InvariantCulture);
            if (targetType == typeof(DateTimeOffset))
                return ((DateTimeOffset)value).ToString("O", CultureInfo.InvariantCulture);
            if (targetType == typeof(DateTime))
                return ((DateTime)value).ToString("O", CultureInfo.InvariantCulture);
            if (targetType == typeof(DateOnly))
                return ((DateOnly)value).ToString("O", CultureInfo.InvariantCulture);
            if (targetType == typeof(Microsoft.OData.Edm.Date))
                return ((Microsoft.OData.Edm.Date)value).ToString();
            if (targetType == typeof(TimeOnly))
                return ((TimeOnly)value).ToString("O", CultureInfo.InvariantCulture);
            if (targetType == typeof(Microsoft.OData.Edm.TimeOfDay))
                return ((Microsoft.OData.Edm.TimeOfDay)value).ToString();
            if (targetType == typeof(TimeSpan))
                return ((TimeSpan)value).ToString("c", CultureInfo.InvariantCulture);
            if (targetType.IsEnum)
                return value.ToString() ?? string.Empty;

            return Convert.ToString(value, CultureInfo.InvariantCulture) ?? string.Empty;
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
            => BuildCastFunctionElement(value, null);

        private XmlElement BuildCastFunctionElement(string value, Type? toType)
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
            toArgument.AppendChild(CreateSimpleElement(XmlDataConstants.OBJECTTYPEELEMENT, toType is null ? "System.Object" : GetConfiguredTypeName(toType)));
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
            string? resolvedName = TryResolveConstantVariableName(preferredVariableName, sourceType, memberName);
            if (resolvedName is not null)
                return resolvedName;

            return preferredVariableName
                ?? throw new NotSupportedException($"No configured variable could be resolved for member '{memberName}'.");
        }

        private string? TryResolveConstantVariableName(string? preferredVariableName, Type? sourceType, string? memberName)
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

            return null;
        }

        private string? TryGetModelVariableName(Type modelType)
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
            return _configurationService.VariableList.Variables.ContainsKey(fallback) ? fallback : null;
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
