using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.GenericArguments;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Variables;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Data;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using Microsoft.OData.Edm;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Services.Data
{
    internal class AnyParametersHelper : IAnyParametersHelper
    {
        private readonly IConfigurationService _configurationService;
        private readonly IConstructorDataParser _constructorDataParser;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IFunctionDataParser _functionDataParser;
        private readonly IGenericConstructorHelper _genericConstructorHelper;
        private readonly IGenericFunctionHelper _genericFunctionHelper;
        private readonly ITypeHelper _typeHelper;
        private readonly ITypeLoadHelper _typeLoadHelper;
        private readonly IVariableDataParser _variableDataParser;

        private static readonly Dictionary<Type, List<Type>> ImplicitNumberConversions = new()
        {//float.TryParse and double.TryParse will succeed and round for higher precision numbers
         //decimal fails to parse 1.7976931348623157E+308 where double succeeds
         //Investigation suggests that TryParse cannot be used to distinguish between float, double and decimal
         //I can't find a case where double.TryParse returns false and decimal.TryParse returns true so we'll use double
         //where the floating point type is undecided.
            [typeof(sbyte)] = new List<Type> { typeof(short), typeof(int), typeof(long), typeof(double) },
            [typeof(byte)] = new List<Type> { typeof(short), typeof(ushort), typeof(int), typeof(uint), typeof(long), typeof(ulong), typeof(double) },
            [typeof(short)] = new List<Type> { typeof(int), typeof(long), typeof(double) },
            [typeof(ushort)] = new List<Type> { typeof(int), typeof(uint), typeof(long), typeof(ulong), typeof(double) },
            [typeof(int)] = new List<Type> { typeof(long), typeof(double) },
            [typeof(uint)] = new List<Type> { typeof(long), typeof(ulong), typeof(double) },
            [typeof(long)] = new List<Type> { typeof(double) },
            [typeof(ulong)] = new List<Type> { typeof(double) },
            [typeof(float)] = new List<Type> { typeof(double)},
            [typeof(decimal)] = new List<Type> { },
            [typeof(double)] = new List<Type> { }
        };

        public AnyParametersHelper(
            IConfigurationService configurationService,
            IConstructorDataParser constructorDataParser,
            IExceptionHelper exceptionHelper,
            IFunctionDataParser functionDataParser,
            IGenericConstructorHelper genericConstructorHelper,
            IGenericFunctionHelper genericFunctionHelper,
            ITypeHelper typeHelper,
            ITypeLoadHelper typeLoadHelper,
            IVariableDataParser variableDataParser)
        {
            _configurationService = configurationService;
            _constructorDataParser = constructorDataParser;
            _exceptionHelper = exceptionHelper;
            _functionDataParser = functionDataParser;
            _genericConstructorHelper = genericConstructorHelper;
            _genericFunctionHelper = genericFunctionHelper;
            _typeHelper = typeHelper;
            _typeLoadHelper = typeLoadHelper;
            _variableDataParser = variableDataParser;
        }

        public AnyParameterPair GetTypes(XmlElement firstXmlParameter, XmlElement secondXmlParameter, CodeBinaryOperatorType codeBinaryOperatorType, ApplicationTypeInfo application)
        {
            return GetTypePair
            (
                GetDataTypeForValue(firstXmlParameter, application),
                GetDataTypeForValue(secondXmlParameter, application),
                GetXmlNodeType(firstXmlParameter),
                GetXmlNodeType(secondXmlParameter)
            );

            AnyParameterPair GetTypePair(Type? p1, Type? p2, XmlNodeType? p1NodeType, XmlNodeType? p2NodeType)
            {
                if (p1 == p2 && !BothNodeTypesAreTextOrNoChildNodes())//both types are not null and equal
                    return new AnyParameterPair(p1, p2);

                if (p1NodeType == XmlNodeType.Element && p2NodeType == XmlNodeType.Text)
                {/*Neither p1 nor p2 is not null given the node types XmlNodeType.Element and XmlNodeType.Text*/
                    if (!_typeHelper.IsLiteralType(p1!))
                        return new AnyParameterPair(p1, p2);//Can't convert further if not literal. Subsequent validation for Any will add the error message

                    return new AnyParameterPair
                    (
                        p1,
                        TryConvertTextToLiteralType
                        (
                            _typeHelper.IsNullable(p1!) ? Nullable.GetUnderlyingType(p1!)! : p1!,
                            p2!,
                            (XmlText)secondXmlParameter.ChildNodes[0]!/*ChildNodes[0] is not null if XmlText*/
                        )
                    );
                }

                if (p2NodeType == XmlNodeType.Element && p1NodeType == XmlNodeType.Text)
                {/*Neither p1 nor p2 is not null given the node types XmlNodeType.Element and XmlNodeType.Text*/
                    if (!_typeHelper.IsLiteralType(p2!))/*p2 is not null if p1NodeType == XmlNodeType.Element*/
                        return new AnyParameterPair(p1, p2);//Can't convert further if not literal.

                    return new AnyParameterPair
                    (
                        TryConvertTextToLiteralType
                        (
                            _typeHelper.IsNullable(p2!) ? Nullable.GetUnderlyingType(p2!)! : p2!,
                            p1!,
                            (XmlText)firstXmlParameter.ChildNodes[0]!
                        ),
                        p2
                    );
                }

                if (p1NodeType == XmlNodeType.Element && !p2NodeType.HasValue)
                {/*p1 not null given the node type XmlNodeType.Element*/
                    if (!_typeHelper.IsLiteralType(p1!))
                        return new AnyParameterPair(p1, p2);//Can't convert further if not literal. Subsequent validation for Any will add the error message

                    if (_typeHelper.IsNullable(p1!) || p1 == typeof(string))
                    {
                        return new AnyParameterPair(p1, p1);//p1 is a string or nullable literal and p2 is empty
                    }
                    else
                    {
                        Type nullableType = typeof(Nullable<>).MakeGenericType(p1!);
                        return new AnyParameterPair(p1!, nullableType);
                    }
                }

                if (p2NodeType == XmlNodeType.Element && !p1NodeType.HasValue)
                {/*p2 is not null given the node type XmlNodeType.Element*/
                    if (!_typeHelper.IsLiteralType(p2!))
                        return new AnyParameterPair(p1, p2);//Can't convert further if not literal.

                    if (_typeHelper.IsNullable(p2!) || p2 == typeof(string))
                    {
                        return new AnyParameterPair(p2, p2);//p2 is a string or nullable literal and p1 is empty
                    }
                    else
                    {
                        Type nullableType = typeof(Nullable<>).MakeGenericType(p2!);
                        return new AnyParameterPair(nullableType, p2!);
                    }
                }

                if (BothNodeTypesAreElements())
                    return new AnyParameterPair(p1, p2);

                if (BothNodeTypesAreText())
                {
                    XmlText textNode1 = (XmlText)firstXmlParameter.ChildNodes[0]!;
                    XmlText textNode2 = (XmlText)secondXmlParameter.ChildNodes[0]!;
                    AnyParameterPair? pair;

                    foreach (Type type in GetLiteralTypes(codeBinaryOperatorType))
                    {
                        if ((pair = MatchNodes(type, textNode1, textNode2)) != null)
                            return pair;
                    }
                }
                else if (BothNodeTypesAreTextOrNoChildNodes())
                {//if BothNodeTypesAreText() then nullables don't help hence the "else if"
                    if (firstXmlParameter.ChildNodes.Count > 1 || secondXmlParameter.ChildNodes.Count > 1)
                        return new AnyParameterPair(typeof(string), typeof(string));

                    XmlText? textNode1 = firstXmlParameter.HasChildNodes ? (XmlText)firstXmlParameter.ChildNodes[0]! : null;
                    XmlText? textNode2 = secondXmlParameter.HasChildNodes ? (XmlText)secondXmlParameter.ChildNodes[0]! : null;
                    AnyParameterPair? pair;

                    foreach (Type type in GetLiteralTypes(codeBinaryOperatorType))
                    {
                        if (type == typeof(string))
                            return new AnyParameterPair(typeof(string), typeof(string));

                        if ((pair = MatchNullableNodes(typeof(Nullable<>).MakeGenericType(type), textNode1, textNode2, p1NodeType, p2NodeType)) != null)
                            return pair;
                    }
                }

                return new AnyParameterPair(p1, p2);

                bool BothNodeTypesAreElements()
                    => p1NodeType == XmlNodeType.Element && p2NodeType == XmlNodeType.Element;
                bool BothNodeTypesAreText()
                    => p1NodeType == XmlNodeType.Text && p2NodeType == XmlNodeType.Text;
                bool BothNodeTypesAreTextOrNoChildNodes()
                    => (p1NodeType == XmlNodeType.Text || !p1NodeType.HasValue)
                    && (p2NodeType == XmlNodeType.Text || !p2NodeType.HasValue);
            }

            //return AnyParameterPair using the literal type if both nodes can be parsed to the literal type
            AnyParameterPair? MatchNodes(Type literalType, XmlText textNode1, XmlText textNode2)
                => _typeHelper.TryParse(textNode1.Value!/*Value is not null for XmlText*/, literalType, out object? _) 
                    && _typeHelper.TryParse(textNode2.Value!/*Value is not null for XmlText*/, literalType, out object? _)
                    ? new AnyParameterPair(literalType, literalType)
                    : null;

            AnyParameterPair? MatchNullableNodes(Type nullableType, XmlText? textNode1, XmlText? textNode2, XmlNodeType? p1NodeType, XmlNodeType? p2NodeType)
            {
                if (!_typeHelper.IsNullable(nullableType))
                    throw _exceptionHelper.CriticalException("{7371A0F1-D881-4CC7-94FA-C78BB64B5CAA}");

                //return AnyParameterPair using the nullable type if both nodes are either empty or can be parsed to the nullable type
                return (!p1NodeType.HasValue || _typeHelper.TryParse(textNode1!.Value!/*Value is not null for XmlText and p1NodeType.HasValue*/, nullableType, out object? _))
                       && (!p2NodeType.HasValue || _typeHelper.TryParse(textNode2!.Value!/*Value is not null for XmlText and p2NodeType.HasValue*/, nullableType, out object? _))
                           ? new AnyParameterPair(nullableType, nullableType)
                           : null;
            }

            Type TryConvertTextToLiteralType(Type literalType, Type textType, XmlText textNode)
            {
                if (_typeHelper.TryParse(textNode.Value!/*Value is not null for XmlText*/, literalType, out object _))
                    return literalType;

                if (!ImplicitNumberConversions.TryGetValue(literalType, out List<Type>? possibleConversions))
                    return textType;

                if (possibleConversions.Count == 0)
                    return textType;

                foreach (Type implicitConversion in possibleConversions)
                {
                    if (_typeHelper.TryParse(textNode.Value!/*Value is not null for XmlText*/, implicitConversion, out object _))
                        return implicitConversion;
                }

                return textType;
            }
        }

        private static XmlNodeType? GetXmlNodeType(XmlElement xmlElement)
        {
            if (xmlElement.ChildNodes.Count > 1)
                return null;

            if (xmlElement.ChildNodes.Count == 0)
                return null;

            return xmlElement.ChildNodes[0]!.NodeType;//ChildNodes.Count == 1 for a literal parameter
        }

        private Type? GetDataTypeForValue(XmlElement xmlElement, ApplicationTypeInfo application)
        {
            if (xmlElement.ChildNodes.Count > 1)
                return typeof(string);

            if (xmlElement.ChildNodes.Count == 0)
                return null;

            return GetType(xmlElement.ChildNodes[0]!);//ChildNodes.Count == 1 for a literal parameter
            Type GetType(XmlNode childNode)
            {
                switch (childNode.NodeType)
                {
                    case XmlNodeType.Element:
                        switch (childNode.Name)
                        {
                            case XmlDataConstants.CONSTRUCTORELEMENT:
                                Type GetConstructorType(ConstructorData cData)
                                {
                                    Constructor constructor = _genericConstructorHelper.ConvertGenericTypes
                                    (
                                        _configurationService.ConstructorList.Constructors[cData.Name], 
                                        cData.GenericArguments, 
                                        application
                                    );

                                    if (!_typeLoadHelper.TryGetSystemType(constructor.TypeName, application, out Type? constructorType))
                                        throw _exceptionHelper.CriticalException("{62E39819-12B9-45B9-B27A-09258DA54527}");
                                    //elements have been validated

                                    return constructorType;
                                }

                                return GetConstructorType(_constructorDataParser.Parse((XmlElement)childNode));
                            case XmlDataConstants.FUNCTIONELEMENT:
                                Type GetFunctionReturnType(FunctionData fData)
                                {
                                    Function function = _genericFunctionHelper.ConvertGenericTypes
                                    (
                                        _configurationService.FunctionList.Functions[fData.Name],
                                        fData.GenericArguments,
                                        application
                                    );

                                    if (!_typeLoadHelper.TryGetSystemType(function.ReturnType, Array.Empty<GenericConfigBase>(), application, out Type? functionType))
                                        throw _exceptionHelper.CriticalException("{6A69BDED-53C5-4025-93BB-3BAEB1C52732}");
                                    //elements have been validated

                                    return functionType;
                                }

                                return GetFunctionReturnType(_functionDataParser.Parse((XmlElement)childNode));
                            case XmlDataConstants.VARIABLEELEMENT:
                                VariableBase variable = _configurationService.VariableList.Variables[_variableDataParser.Parse((XmlElement)childNode).Name];

                                if (!_typeLoadHelper.TryGetSystemType(variable, application, out Type? variableType))
                                    throw _exceptionHelper.CriticalException("{2D36AEFC-7812-4307-913C-C52450843D07}");
                                //elements have been validated

                                return variableType;
                            default:
                                throw _exceptionHelper.CriticalException("{D8F72D74-1AA1-40CE-BE96-FEA6082D9842}");
                        }
                    case XmlNodeType.Text:
                    case XmlNodeType.Whitespace:
                        return typeof(string);
                    default:
                        throw _exceptionHelper.CriticalException("{F0EF9FF7-F998-4C2D-9803-D957731C8654}");
                }
            }
        }

        private Type[] GetLiteralTypes(CodeBinaryOperatorType operatorType)
        {
#pragma warning disable IDE0066 // Convert switch statement to expression
            switch (operatorType)
            {
                case CodeBinaryOperatorType.Add:
                    return new Type[]
                    {
                        typeof(sbyte),
                        typeof(byte),
                        typeof(char),
                        typeof(short),
                        typeof(ushort),
                        typeof(int),
                        typeof(uint),
                        typeof(long),
                        typeof(ulong),
                        typeof(double),
                        typeof(float),
                        typeof(decimal),
                        typeof(TimeSpan),
                        typeof(string)
                    };
                case CodeBinaryOperatorType.Subtract:
                    return new Type[]
                    {
                        typeof(sbyte),
                        typeof(byte),
                        typeof(char),
                        typeof(short),
                        typeof(ushort),
                        typeof(int),
                        typeof(uint),
                        typeof(long),
                        typeof(ulong),
                        typeof(double),
                        typeof(float),
                        typeof(decimal),
                        typeof(TimeSpan),
                        typeof(TimeOnly),
                        typeof(DateTime),
                        typeof(DateTimeOffset)
                    };
                case CodeBinaryOperatorType.Multiply:
                case CodeBinaryOperatorType.Modulus:
                    return new Type[]
                    {
                        typeof(sbyte),
                        typeof(byte),
                        typeof(char),
                        typeof(short),
                        typeof(ushort),
                        typeof(int),
                        typeof(uint),
                        typeof(long),
                        typeof(ulong),
                        typeof(double),
                        typeof(float),
                        typeof(decimal)
                    };
                case CodeBinaryOperatorType.Divide:
                    return new Type[]
                    {
                        typeof(sbyte),
                        typeof(byte),
                        typeof(char),
                        typeof(short),
                        typeof(ushort),
                        typeof(int),
                        typeof(uint),
                        typeof(long),
                        typeof(ulong),
                        typeof(double),
                        typeof(float),
                        typeof(decimal),
                        typeof(TimeSpan)
                    };
                case CodeBinaryOperatorType.IdentityInequality:
                case CodeBinaryOperatorType.IdentityEquality:
                case CodeBinaryOperatorType.ValueEquality:
                    return new Type[]
                    {
                        typeof(bool),
                        typeof(sbyte),
                        typeof(byte),
                        typeof(char),
                        typeof(short),
                        typeof(ushort),
                        typeof(int),
                        typeof(uint),
                        typeof(long),
                        typeof(ulong),
                        typeof(double),
                        typeof(float),
                        typeof(decimal),
                        typeof(TimeSpan),
                        typeof(TimeOnly),
                        typeof(TimeOfDay),
                        typeof(DateTime),
                        typeof(DateTimeOffset),
                        typeof(DateOnly),
                        typeof(Date),
                        typeof(Guid),
                        typeof(string)
                    };
                case CodeBinaryOperatorType.BitwiseOr:
                case CodeBinaryOperatorType.BitwiseAnd:
                    return new Type[]
                    {
                        typeof(bool),
                        typeof(sbyte),
                        typeof(byte),
                        typeof(char),
                        typeof(short),
                        typeof(ushort),
                        typeof(int),
                        typeof(uint),
                        typeof(long),
                        typeof(ulong)
                    };
                case CodeBinaryOperatorType.BooleanOr:
                case CodeBinaryOperatorType.BooleanAnd:
                    return new Type[]
                    {
                        typeof(bool)
                    };
                case CodeBinaryOperatorType.LessThan:
                case CodeBinaryOperatorType.LessThanOrEqual:
                case CodeBinaryOperatorType.GreaterThan:
                case CodeBinaryOperatorType.GreaterThanOrEqual:
                    return new Type[]
                    {
                        typeof(sbyte),
                        typeof(byte),
                        typeof(char),
                        typeof(short),
                        typeof(ushort),
                        typeof(int),
                        typeof(uint),
                        typeof(long),
                        typeof(ulong),
                        typeof(double),
                        typeof(float),
                        typeof(decimal),
                        typeof(TimeSpan),
                        typeof(TimeOnly),
                        typeof(TimeOfDay),
                        typeof(DateTime),
                        typeof(DateTimeOffset),
                        typeof(DateOnly),
                        typeof(Date)
                    };
                default:
                    throw _exceptionHelper.CriticalException("{CCF59054-4A26-4ED0-A6E9-FFFD45247EC4}");
            }
#pragma warning restore IDE0066 // Convert switch statement to expression
        }
    }
}
