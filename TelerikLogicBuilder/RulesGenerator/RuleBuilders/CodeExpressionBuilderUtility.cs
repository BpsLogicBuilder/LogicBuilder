using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Variables;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Data;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Variables;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator.RuleBuilders;
using Microsoft.OData.Edm;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.RulesGenerator.RuleBuilders
{
    internal class CodeExpressionBuilderUtility
    {
        private readonly IAnyParametersHelper _anyParametersHelper;
        private readonly IConfigurationService _configurationService;
        private readonly IConstructorDataParser _constructorDataParser;
        private readonly IEnumHelper _enumHelper;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IFunctionDataParser _functionDataParser;
        private readonly IFunctionHelper _functionHelper; 
        private readonly IGetValidConfigurationFromData _getValidConfigurationFromData;
        private readonly ILiteralListDataParser _literalListDataParser;
        private readonly ILiteralListParameterDataParser _literalListParameterDataParser;
        private readonly ILiteralListVariableDataParser _literalListVariableDataParser;
        private readonly IMetaObjectDataParser _metaObjectDataParser;
        private readonly IObjectDataParser _objectDataParser;
        private readonly IObjectListDataParser _objectListDataParser;
        private readonly IObjectListParameterDataParser _objectListParameterDataParser;
        private readonly IObjectListVariableDataParser _objectListVariableDataParser;
        private readonly IObjectParameterDataParser _objectParameterDataParser;
        private readonly IObjectVariableDataParser _objectVariableDataParser;
        private readonly IParameterHelper _parameterHelper;
        private readonly IResourcesManager _resourcesManager;
        private readonly ITypeLoadHelper _typeLoadHelper;
        private readonly IVariableDataParser _variableDataParser;
        private readonly IVariableHelper _variableHelper;
        
        private readonly ApplicationTypeInfo application;
        private readonly IDictionary<string, string> resourceStrings;
        private readonly string moduleName;

        public CodeExpressionBuilderUtility(
            string moduleName,
            ApplicationTypeInfo application,
            IDictionary<string, string> resourceStrings,
            IConfigurationService configurationService,
            IContextProvider contextProvider,
            IAnyParametersHelper anyParametersHelper,
            IConstructorDataParser constructorDataParser,
            IFunctionDataParser functionDataParser,
            IGetValidConfigurationFromData getValidConfigurationFromData,
            ILiteralListDataParser literalListDataParser,
            ILiteralListParameterDataParser literalListParameterDataParser,
            ILiteralListVariableDataParser literalListVariableDataParser,
            IMetaObjectDataParser metaObjectDataParser,
            IObjectDataParser objectDataParser,
            IObjectListDataParser objectListDataParser,
            IObjectListParameterDataParser objectListParameterDataParser,
            IObjectListVariableDataParser objectListVariableDataParser,
            IObjectParameterDataParser objectParameterDataParser,
            IObjectVariableDataParser objectVariableDataParser,
            IParameterHelper parameterHelper,
            IResourcesManager resourcesManager,
            ITypeLoadHelper typeLoadHelper,
            IVariableDataParser variableDataParser)
        {
            this.application = application;
            this.resourceStrings = resourceStrings;
            this.moduleName = moduleName;
            _anyParametersHelper = anyParametersHelper;
            _configurationService = configurationService;
            _enumHelper = contextProvider.EnumHelper;
            _exceptionHelper = contextProvider.ExceptionHelper;
            _functionHelper = contextProvider.FunctionHelper;
            _variableHelper = contextProvider.VariableHelper;
            _constructorDataParser = constructorDataParser;
            _functionDataParser = functionDataParser;
            _getValidConfigurationFromData = getValidConfigurationFromData;
            _literalListDataParser = literalListDataParser;
            _literalListParameterDataParser = literalListParameterDataParser;
            _literalListVariableDataParser = literalListVariableDataParser;
            _metaObjectDataParser = metaObjectDataParser;
            _objectDataParser = objectDataParser;
            _objectListDataParser = objectListDataParser;
            _objectListParameterDataParser = objectListParameterDataParser;
            _objectListVariableDataParser = objectListVariableDataParser;
            _objectParameterDataParser = objectParameterDataParser;
            _objectVariableDataParser = objectVariableDataParser;
            _parameterHelper = parameterHelper;
            _resourcesManager = resourcesManager;
            _typeLoadHelper = typeLoadHelper;  
            _variableDataParser = variableDataParser;
        }

        #region Constants
        internal const string PARSEMETHOD = "Parse";
        internal const string INVARIANTCULTUREPROPERTY = "InvariantCulture";
        internal const string NONEFIELD = "None";
        #endregion Constants

        #region Properties
        private static CodePropertyReferenceExpression DirectorReference
            => new (new CodeThisReferenceExpression(), RuleFunctionConstants.DIRECTORPROPERTYNAME);
        #endregion Properties

        /// <summary>
        /// Result of multiple binary operations where operatorType == CodeBinaryOperatorType.BooleanAnd or operatorType == CodeBinaryOperatorType.BooleanOr
        /// </summary>
        /// <param name="conditions"></param>
        /// <param name="operatorType"></param>
        /// <returns></returns>
        public static CodeExpression AggregateConditions(IEnumerable<IfCondition> conditions, CodeBinaryOperatorType operatorType)
            => conditions
                .Select(c => c.ResultantCondition)
                .Aggregate
                (
                    (c1, c2) =>
                    new CodeBinaryOperatorExpression
                    {
                        Left = c1,
                        Operator = operatorType,
                        Right = c2
                    }
                );

        /// <summary>
        /// Builds a Code Assign Statement given the value data and the variable
        /// </summary>
        /// <param name="valueData"></param>
        /// <param name="variable"></param>
        /// <returns></returns>
        public CodeStatement BuildAssignmentStatement(VariableValueData valueData, VariableBase variable)
            => new CodeAssignStatement
            (
                BuildImplementedVariableExpression(variable),
                BuildAssignmentValue(valueData, variable)
            );

        /// <summary>
        /// Builds a Code Assign Statement to set a variable to null.
        /// </summary>
        /// <param name="variable"></param>
        /// <returns></returns>
        public CodeStatement BuildAssignToNullStatement(VariableBase variable)
            => new CodeAssignStatement(BuildImplementedVariableExpression(variable), new CodePrimitiveExpression(null));

        /// <summary>
        /// Returns a Code Binary Operator Expression for property == propertyValue test
        /// </summary>
        /// <param name="connectorXmlNode"></param>
        /// <returns></returns>
        public static CodeExpression BuildDirectorPropertyCondition(string property, CodeExpression propertyValue)
            => new CodeBinaryOperatorExpression
            {
                Left = new CodePropertyReferenceExpression(DirectorReference, property),
                Operator = CodeBinaryOperatorType.ValueEquality,
                Right = propertyValue
            };

        /// <summary>
        /// Returns a Code Binary Operator Expression for the Driver condition given shape index and page index
        /// </summary>
        /// <param name="connectorXmlNode"></param>
        /// <returns></returns>
        public static CodeExpression BuildDriverCondition(int shapeIndex, int pageIndex)
            => new CodeBinaryOperatorExpression
            {
                Left = new CodePropertyReferenceExpression(DirectorReference, DirectorProperties.DRIVERPROPERTY),
                Operator = CodeBinaryOperatorType.ValueEquality,
                Right = new CodePrimitiveExpression(string.Format(CultureInfo.InvariantCulture, RuleDefinitionConstants.DRIVERFORMAT, shapeIndex, pageIndex))
            };

        /// <summary>
        /// Builds a Method Invoke Expression given function data
        /// </summary>
        /// <param name="functionData"></param>
        /// <param name="connectorDataList"></param>
        /// <returns></returns>
        /// <exception cref="CriticalLogicBuilderException"></exception>
        public CodeExpression BuildFunction(FunctionData functionData, IList<ConnectorData>? connectorDataList)
        {
            if (!_getValidConfigurationFromData.TryGetFunction(functionData, this.application, out Function? function))
                throw _exceptionHelper.CriticalException("{3AF15750-8A8B-4DD8-94E3-70EF0AE07AE8}");

            IList<CodeExpression> parameters = BuildParametersList
            (
                function.Parameters,
                functionData.ParameterElementsList.ToDictionary(e => e.GetAttribute(XmlDataConstants.NAMEATTRIBUTE))
            );

            if (function.FunctionCategory == FunctionCategories.DialogForm)
            {
                if (connectorDataList?.Count > 0)
                    parameters.Add(BuildChoices(connectorDataList));
                else
                    parameters.Add(BuildConnectorListExpression(Array.Empty<CodeExpression>()));
            }

            return new CodeMethodInvokeExpression
            (
                GetReferenceExpression(function),
                function.MemberName,
                parameters.ToArray()
            );
        }

        /// <summary>
        /// returns If Condition given decision data
        /// </summary>
        /// <param name="decisionData"></param>
        /// <returns></returns>
        public IfCondition BuildIfCondition(DecisionData decisionData)
        {
            if (decisionData == null)
                throw _exceptionHelper.CriticalException("{EB2A4A3E-20E2-4582-8D85-C1F4C69F36AD}");
            
            if (!_getValidConfigurationFromData.TryGetVariable(decisionData, this.application, out VariableBase? _))
                throw _exceptionHelper.CriticalException("{34CAA9D0-BE61-42EA-BCF6-8A790CD09CAA}");

            return new(BuildPredicates(decisionData), decisionData.IsNotDecision);
        }

        /// <summary>
        /// Returns If Condition given function data
        /// </summary>
        /// <param name="functionData"></param>
        /// <returns></returns>
        public IfCondition BuildIfCondition(FunctionData functionData)
        {
            return new(BuildCondition(functionData), functionData.IsNotFunction);
        }

        /// <summary>
        /// Returns a Code Binary Operator Expression for the Selection condition given connector XML
        /// </summary>
        /// <param name="connectorXmlNode"></param>
        /// <returns></returns>
        public CodeExpression BuildSelectCondition(XmlNode connectorXmlNode)
            => new CodeBinaryOperatorExpression
            {
                Left = new CodePropertyReferenceExpression(DirectorReference, DirectorProperties.SELECTIONPROPERTY),
                Operator = CodeBinaryOperatorType.ValueEquality,
                Right = BuildSelectionShortString(connectorXmlNode)
            };

        /// <summary>
        /// Returns a Code Binary Operator Expression for the Selection condition given a string
        /// </summary>
        /// <param name="connectorXmlNode"></param>
        /// <returns></returns>
        public static CodeExpression BuildSelectCondition(string text)
            => new CodeBinaryOperatorExpression
            {
                Left = new CodePropertyReferenceExpression(DirectorReference, DirectorProperties.SELECTIONPROPERTY),
                Operator = CodeBinaryOperatorType.ValueEquality,
                Right = new CodePrimitiveExpression(text)
            };

        private static CodeExpression[] BuildArrayIndicesExpression(string variableName)
        {
            string[] variableNames = variableName.Trim().Split(new char[] { MiscellaneousConstants.COMMA }, StringSplitOptions.RemoveEmptyEntries);
            CodeExpression[] arrayIndices = new CodeExpression[variableNames.Length];
            for (int i = 0; i < variableNames.Length; i++)
                arrayIndices[i] = new CodePrimitiveExpression(int.Parse(variableNames[i], CultureInfo.InvariantCulture));

            return arrayIndices;
        }

        private CodeExpression BuildAssignmentValue(VariableValueData valueData, VariableBase variable)
            => BuildVariable(valueData.ChildElement, variable);

        private CodeExpression BuildBinaryOperatorExpression(Function function, FunctionData functionData, ApplicationTypeInfo applicaton)
        {
            if (!_parameterHelper.IsParameterAny(function.Parameters[0]))
            {
                return new CodeBinaryOperatorExpression
                {
                    Left = BuildParameter(functionData.ParameterElementsList[0], function),
                    Operator = (CodeBinaryOperatorType)Enum.Parse(typeof(CodeBinaryOperatorType), function.MemberName),
                    Right = BuildParameter(functionData.ParameterElementsList[1], function)
                };
            }
            else
            {
                return GetBinaryExpression
                (
                    _anyParametersHelper.GetTypes
                    (
                        functionData.ParameterElementsList[0], 
                        functionData.ParameterElementsList[1],
                        _enumHelper.ParseEnumText<CodeBinaryOperatorType>(function.MemberName), 
                        applicaton
                    )
                );
                CodeExpression GetBinaryExpression(AnyParameterPair types)
                {
                    return new CodeBinaryOperatorExpression
                    {
                        Left = BuildParameterValue(functionData.ParameterElementsList[0], _enumHelper.GetLiteralType(types.ParameterOneType!)),
                        Operator = (CodeBinaryOperatorType)Enum.Parse(typeof(CodeBinaryOperatorType), function.MemberName),
                        Right = BuildParameterValue(functionData.ParameterElementsList[1], _enumHelper.GetLiteralType(types.ParameterTwoType!))
                    };
                }
            }
        }

        private CodeExpression BuildChoices(IList<ConnectorData> connectorDataList)
            => BuildConnectorListExpression
            (
                connectorDataList
                    .Select
                        (
                            connectorData => new CodeObjectCreateExpression
                            (//ConnectorParameters(int Id, string ShortString, string LongString, object ConnectorData)
                                new CodeTypeReference(ConnectorWrapperClasses.CONNECTORCLASS),
                                new CodePrimitiveExpression((int)connectorData.Index),
                                BuildSelectionShortString(connectorData.TextXmlNode),//short string
                                BuildSelectionLongString(connectorData.TextXmlNode),//long string
                                BuildObject(_metaObjectDataParser.Parse(connectorData.MetaObjectDataXmlNode!))
                            )
                        )
                    .ToArray()
            );

        private static CodeObjectCreateExpression BuildCollectionListExpression(CodeExpression[] makeListArgs)
            => new
            (
                new CodeTypeReference(RuleFunctionConstants.COLLECTIONCLASSNAME, new CodeTypeReference[] { new CodeTypeReference(RuleFunctionConstants.COLLECTIONELEMENTSCLASSNAME) }),
                new CodeArrayCreateExpression(RuleFunctionConstants.COLLECTIONELEMENTSCLASSNAME, makeListArgs)
            );

        private static CodeObjectCreateExpression BuildConnectorListExpression(CodeExpression[] makeListArgs)
            => new
            (
                new CodeTypeReference(RuleFunctionConstants.LISTCLASSNAME, new CodeTypeReference[] { new CodeTypeReference(ConnectorWrapperClasses.CONNECTORCLASS) }),
                new CodeArrayCreateExpression(ConnectorWrapperClasses.CONNECTORCLASS, makeListArgs)
            );

        private CodeExpression BuildCondition(FunctionData functionData)
        {
            if (!_getValidConfigurationFromData.TryGetFunction(functionData, this.application, out Function? function))
                throw _exceptionHelper.CriticalException("{70FA0D74-B0BF-4A76-9FA3-1863888FD03D}");

            if (!_functionHelper.IsBoolean(function))
                throw _exceptionHelper.CriticalException("{3D7BB08C-8CDE-461A-82C6-370CE22878CC}");

            if (function.FunctionCategory != FunctionCategories.BinaryOperator)
                return BuildFunction(functionData);

            return BuildBinaryOperatorExpression(function, functionData, this.application);
        }

        private CodeExpression BuildConstructor(ConstructorData constructorData)
        {
            if (!_getValidConfigurationFromData.TryGetConstructor(constructorData, this.application, out Constructor? constructor))
                throw _exceptionHelper.CriticalException("{64C1C685-8721-44DA-8004-29C875BF160B}");

            return new CodeObjectCreateExpression
            (
                new CodeTypeReference(constructor.TypeName),
                BuildParametersList
                (
                    constructor.Parameters,
                    constructorData.ParameterElementsList.ToDictionary(e => e.GetAttribute(XmlDataConstants.NAMEATTRIBUTE))
                ).ToArray()
            );
        }

        private CodeExpression BuildFunction(FunctionData functionData)
        {
            if (!_getValidConfigurationFromData.TryGetFunction(functionData, this.application, out Function? function))
                throw _exceptionHelper.CriticalException("{19FECC32-70D3-4CDB-9703-C9404868026E}");

            if (function.FunctionCategory == FunctionCategories.BinaryOperator)
                return BuildBinaryOperatorExpression(function, functionData, this.application);

            if (function.FunctionCategory == FunctionCategories.Cast)
            {
                if(!_typeLoadHelper.TryGetSystemType(function.ReturnType, functionData.GenericArguments, this.application, out Type? returnType))
                    throw _exceptionHelper.CriticalException("{440F5987-D1A9-4917-B0E0-43A4C8B0FAE3}");

                return new CodeCastExpression(returnType, BuildParameter(functionData.ParameterElementsList[0], function));
            }

            return new CodeMethodInvokeExpression
            (
                GetReferenceExpression(function),
                function.MemberName,
                BuildParametersList
                (
                    function.Parameters,
                    functionData.ParameterElementsList.ToDictionary(e => e.GetAttribute(XmlDataConstants.NAMEATTRIBUTE))
                ).ToArray()
            );
        }

        private static CodeExpression BuildGetResorceFunction(string shortValue, Type literalType)
            => new CodeMethodInvokeExpression//public static T GetResource(string shortValue, DirectorBase director)
            (
                new CodeTypeReferenceExpression
                (
                    new CodeTypeReference
                    (
                        RuleFunctionConstants.RESOURCESHELPERCLASS,
                        new CodeTypeReference[] { new CodeTypeReference(literalType) }
                    )
                ),
                RuleFunctionConstants.GETRESOURCEMETHODNAME,
                new CodeExpression[] { new CodePrimitiveExpression(shortValue), DirectorReference }
            );

        private CodeExpression BuildImplementedVariableExpression(VariableBase variable)
        {
            CodePropertyReferenceExpression invariantCultureReference = new(new CodeTypeReferenceExpression(typeof(CultureInfo)), INVARIANTCULTUREPROPERTY);
            CodeFieldReferenceExpression noneDateTimeStylesReference = new(new CodeTypeReferenceExpression(typeof(DateTimeStyles)), NONEFIELD);
            return variable.VariableCategory switch
            {
                VariableCategory.Property => new CodePropertyReferenceExpression(GetReferenceExpression(variable), variable.MemberName),
                VariableCategory.Field => new CodeFieldReferenceExpression(GetReferenceExpression(variable), variable.MemberName),
                VariableCategory.StringKeyIndexer => new CodeIndexerExpression(GetReferenceExpression(variable), new CodePrimitiveExpression(variable.MemberName)),
                VariableCategory.IntegerKeyIndexer => new CodeIndexerExpression(GetReferenceExpression(variable), new CodePrimitiveExpression(int.Parse(variable.MemberName, CultureInfo.InvariantCulture))),
                VariableCategory.ArrayIndexer => new CodeArrayIndexerExpression(GetReferenceExpression(variable), BuildArrayIndicesExpression(variable.MemberName)),
                VariableCategory.BooleanKeyIndexer => new CodeIndexerExpression(GetReferenceExpression(variable), new CodePrimitiveExpression(bool.Parse(variable.MemberName))),
                VariableCategory.ByteKeyIndexer => new CodeIndexerExpression(GetReferenceExpression(variable), new CodePrimitiveExpression(byte.Parse(variable.MemberName, CultureInfo.InvariantCulture))),
                VariableCategory.CharKeyIndexer => new CodeIndexerExpression(GetReferenceExpression(variable), new CodePrimitiveExpression(char.Parse(variable.MemberName))),
                VariableCategory.DateTimeKeyIndexer => new CodeIndexerExpression(GetReferenceExpression(variable), new CodeMethodInvokeExpression(new CodeTypeReferenceExpression(typeof(DateTime)), PARSEMETHOD, new CodeExpression[] { new CodePrimitiveExpression(variable.MemberName), invariantCultureReference })),
                VariableCategory.DateTimeOffsetKeyIndexer => new CodeIndexerExpression(GetReferenceExpression(variable), new CodeMethodInvokeExpression(new CodeTypeReferenceExpression(typeof(DateTimeOffset)), PARSEMETHOD, new CodeExpression[] { new CodePrimitiveExpression(variable.MemberName), invariantCultureReference })),
                VariableCategory.DateOnlyKeyIndexer => new CodeIndexerExpression(GetReferenceExpression(variable), new CodeMethodInvokeExpression(new CodeTypeReferenceExpression(typeof(DateOnly)), PARSEMETHOD, new CodeExpression[] { new CodePrimitiveExpression(variable.MemberName), invariantCultureReference, noneDateTimeStylesReference })),
                VariableCategory.DateKeyIndexer => new CodeIndexerExpression(GetReferenceExpression(variable), new CodeMethodInvokeExpression(new CodeTypeReferenceExpression(typeof(Date)), PARSEMETHOD, new CodeExpression[] { new CodePrimitiveExpression(variable.MemberName), invariantCultureReference })),
                VariableCategory.TimeSpanKeyIndexer => new CodeIndexerExpression(GetReferenceExpression(variable), new CodeMethodInvokeExpression(new CodeTypeReferenceExpression(typeof(TimeSpan)), PARSEMETHOD, new CodeExpression[] { new CodePrimitiveExpression(variable.MemberName), invariantCultureReference })),
                VariableCategory.TimeOnlyKeyIndexer => new CodeIndexerExpression(GetReferenceExpression(variable), new CodeMethodInvokeExpression(new CodeTypeReferenceExpression(typeof(TimeOnly)), PARSEMETHOD, new CodeExpression[] { new CodePrimitiveExpression(variable.MemberName), invariantCultureReference, noneDateTimeStylesReference })),
                VariableCategory.TimeOfDayKeyIndexer => new CodeIndexerExpression(GetReferenceExpression(variable), new CodeMethodInvokeExpression(new CodeTypeReferenceExpression(typeof(TimeOfDay)), PARSEMETHOD, new CodeExpression[] { new CodePrimitiveExpression(variable.MemberName), invariantCultureReference })),
                VariableCategory.GuidKeyIndexer => new CodeIndexerExpression(GetReferenceExpression(variable), new CodeMethodInvokeExpression(new CodeTypeReferenceExpression(typeof(Guid)), PARSEMETHOD, new CodeExpression[] { new CodePrimitiveExpression(variable.MemberName) })),
                VariableCategory.DecimalKeyIndexer => new CodeIndexerExpression(GetReferenceExpression(variable), new CodePrimitiveExpression(decimal.Parse(variable.MemberName, CultureInfo.InvariantCulture))),
                VariableCategory.DoubleKeyIndexer => new CodeIndexerExpression(GetReferenceExpression(variable), new CodePrimitiveExpression(double.Parse(variable.MemberName, CultureInfo.InvariantCulture))),
                VariableCategory.FloatKeyIndexer => new CodeIndexerExpression(GetReferenceExpression(variable), new CodePrimitiveExpression(float.Parse(variable.MemberName, CultureInfo.InvariantCulture))),
                VariableCategory.LongKeyIndexer => new CodeIndexerExpression(GetReferenceExpression(variable), new CodePrimitiveExpression(long.Parse(variable.MemberName, CultureInfo.InvariantCulture))),
                VariableCategory.SByteKeyIndexer => new CodeIndexerExpression(GetReferenceExpression(variable), new CodePrimitiveExpression(sbyte.Parse(variable.MemberName, CultureInfo.InvariantCulture))),
                VariableCategory.ShortKeyIndexer => new CodeIndexerExpression(GetReferenceExpression(variable), new CodePrimitiveExpression(short.Parse(variable.MemberName, CultureInfo.InvariantCulture))),
                VariableCategory.UIntegerKeyIndexer => new CodeIndexerExpression(GetReferenceExpression(variable), new CodePrimitiveExpression(uint.Parse(variable.MemberName, CultureInfo.InvariantCulture))),
                VariableCategory.ULongKeyIndexer => new CodeIndexerExpression(GetReferenceExpression(variable), new CodePrimitiveExpression(ulong.Parse(variable.MemberName, CultureInfo.InvariantCulture))),
                VariableCategory.UShortKeyIndexer => new CodeIndexerExpression(GetReferenceExpression(variable), new CodePrimitiveExpression(ushort.Parse(variable.MemberName, CultureInfo.InvariantCulture))),
                VariableCategory.VariableKeyIndexer => new CodeIndexerExpression(GetReferenceExpression(variable), BuildVariable(variable.MemberName)),
                VariableCategory.VariableArrayIndexer => new CodeArrayIndexerExpression(GetReferenceExpression(variable), BuildVariableArrayIndicesExpression(variable.MemberName)),
                _ => throw _exceptionHelper.CriticalException("{6E52B376-5BFD-44C9-BFF3-C521572AD4CE}"),
            };
        }

        private CodeExpression BuildLiteralList(LiteralListData literalListData) 
            => GetListExpression
            (
                literalListData.ListType,
                _enumHelper.GetSystemType(literalListData.LiteralType),
                literalListData
                    .ChildElements
                    .Select
                    (
                        i => BuildParameterValue
                        (
                            i, 
                            _enumHelper.GetLiteralType
                            (
                                _enumHelper.GetSystemType(literalListData.LiteralType)
                            )
                        )
                    )
                    .ToArray()
            );

        private CodeExpression BuildObject(ObjectParameterData objectParameterData)
            => BuildObject(objectParameterData.ChildElementCategory, objectParameterData.ChildElement);

        private CodeExpression BuildObject(LiteralListParameterData literalListParameterData)
            => BuildObject(literalListParameterData.ChildElementCategory, literalListParameterData.ChildElement);

        private CodeExpression BuildObject(ObjectListParameterData objectListParameterData)
            => BuildObject(objectListParameterData.ChildElementCategory, objectListParameterData.ChildElement);

        private CodeExpression BuildObject(ObjectVariableData objectVariableData)
            => BuildObject(objectVariableData.ChildElementCategory, objectVariableData.ChildElement);

        private CodeExpression BuildObject(LiteralListVariableData literalListVariableData)
            => BuildObject(literalListVariableData.ChildElementCategory, literalListVariableData.ChildElement);

        private CodeExpression BuildObject(ObjectListVariableData objectListVariableData)
            => BuildObject(objectListVariableData.ChildElementCategory, objectListVariableData.ChildElement);

        private CodeExpression BuildObject(ObjectData objectData)
            => BuildObject(objectData.ChildElementCategory, objectData.ChildElement);

        internal CodeExpression BuildObject(MetaObjectData objectData)
            => BuildObject(objectData.ChildElementCategory, objectData.ChildElement);

        private CodeExpression BuildObject(ObjectCategory objectCategory, XmlElement objectElement) 
            => objectCategory switch
            {
                ObjectCategory.Constructor => BuildConstructor(_constructorDataParser.Parse(objectElement)),
                ObjectCategory.Function => BuildFunction(_functionDataParser.Parse(objectElement)),
                ObjectCategory.Variable => BuildVariable(_variableDataParser.Parse(objectElement).Name),
                ObjectCategory.LiteralList => BuildLiteralList(_literalListDataParser.Parse(objectElement)),
                ObjectCategory.ObjectList => BuildObjectList(_objectListDataParser.Parse(objectElement)),
                _ => throw _exceptionHelper.CriticalException("{13142E2F-4561-470F-A047-106D2813703A}"),
            };

        private CodeExpression BuildObjectList(ObjectListData objectListData)
        {
            if (!_typeLoadHelper.TryGetSystemType(objectListData.ObjectType, this.application, out Type? objectType))
                throw _exceptionHelper.CriticalException("{CB57F398-3429-4A5B-91F4-9E141A50A49A}");

            return GetListExpression
            (
                objectListData.ListType,
                objectType,
                objectListData
                    .ChildElements
                    .Select(i => BuildObject(_objectDataParser.Parse(i)))
                    .ToArray()
            );
        }

        private CodeExpression BuildParameter(XmlElement parameterNode, Function function)
        {
            string parameterName = parameterNode.GetAttribute(XmlDataConstants.NAMEATTRIBUTE);
            if (!function.Parameters.ToDictionary(p => p.Name).TryGetValue(parameterName, out ParameterBase? parameter))
                throw _exceptionHelper.CriticalException("{A873C2A0-6E4B-4BC1-9DE9-FC2109C5D8D3}");

            return BuildParameter(parameterNode, parameter);
        }

        private CodeExpression BuildParameter(XmlElement parameterNode, ParameterBase parameter)
        {
            switch (parameter)
            {
                case LiteralParameter literalParameter:
                    Type literaltype = _enumHelper.GetSystemType(literalParameter.LiteralType);
                    return BuildParameterValue(parameterNode, _enumHelper.GetLiteralType(literaltype));
                case ObjectParameter:
                    return BuildObject(_objectParameterDataParser.Parse(parameterNode));
                case ListOfLiteralsParameter:
                    return BuildObject(_literalListParameterDataParser.Parse(parameterNode));
                case ListOfObjectsParameter:
                    return BuildObject(_objectListParameterDataParser.Parse(parameterNode));
                default:
                    throw _exceptionHelper.CriticalException("{3335F848-32FE-4F21-A27D-610C3FD48FA7}");
            }
        }

        private List<CodeExpression> BuildParametersList(List<ParameterBase> parameters, Dictionary<string, XmlElement> parametersNodeDictionary)
        {
            return parameters.Aggregate(new List<CodeExpression>(), (list, next) =>
            {
                if (!parametersNodeDictionary.TryGetValue(next.Name, out XmlElement? parameterElement))
                {
                    if (next is LiteralParameter literalParameter)
                    {
                        switch (literalParameter.LiteralType)
                        {
                            case LiteralParameterType.DateTime:
                                list.Add(GetParseMethodInvokeExpression(typeof(DateTime)));
                                break;
                            case LiteralParameterType.DateTimeOffset:
                                list.Add(GetParseMethodInvokeExpression(typeof(DateTimeOffset)));
                                break;
                            case LiteralParameterType.DateOnly:
                                list.Add(GetParseDateOnlyAndTimeOnlyMethodInvokeExpression(typeof(DateOnly)));
                                break;
                            case LiteralParameterType.Date:
                                list.Add(GetParseMethodInvokeExpression(typeof(Date)));
                                break;
                            case LiteralParameterType.TimeSpan:
                                list.Add(GetParseMethodInvokeExpression(typeof(TimeSpan)));
                                break;
                            case LiteralParameterType.TimeOnly:
                                list.Add(GetParseDateOnlyAndTimeOnlyMethodInvokeExpression(typeof(TimeOnly)));
                                break;
                            case LiteralParameterType.TimeOfDay:
                                list.Add(GetParseMethodInvokeExpression(typeof(TimeOfDay)));
                                break;
                            case LiteralParameterType.Guid:
                                list.Add(GetParseGuidMethodInvokeExpression(typeof(Guid)));
                                break;
                            default:
                                list.Add(new CodePrimitiveExpression(literalParameter.GetDefaultValue()));
                                break;
                        }

                        CodeExpression GetParseMethodInvokeExpression(Type literalType)
                            => new CodeMethodInvokeExpression
                            (
                                new CodeTypeReferenceExpression(literalType),
                                PARSEMETHOD,
                                new CodeExpression[]
                                {
                                    new CodePrimitiveExpression(literalParameter.GetDefaultValue()!/*not null for literals*/.ToString()),
                                    new CodePropertyReferenceExpression(new CodeTypeReferenceExpression(typeof(CultureInfo)), INVARIANTCULTUREPROPERTY)
                                }
                            );

                        CodeExpression GetParseDateOnlyAndTimeOnlyMethodInvokeExpression(Type literalType)
                            => new CodeMethodInvokeExpression
                            (
                                new CodeTypeReferenceExpression(literalType),
                                PARSEMETHOD,
                                new CodeExpression[]
                                {
                                    new CodePrimitiveExpression(literalParameter.GetDefaultValue()!/*not null for literals*/.ToString()),
                                    new CodePropertyReferenceExpression(new CodeTypeReferenceExpression(typeof(CultureInfo)), INVARIANTCULTUREPROPERTY),
                                    new CodeFieldReferenceExpression(new CodeTypeReferenceExpression(typeof(DateTimeStyles)), NONEFIELD)
                                }
                            );

                        CodeExpression GetParseGuidMethodInvokeExpression(Type literalType)
                            => new CodeMethodInvokeExpression
                            (
                                new CodeTypeReferenceExpression(literalType),
                                PARSEMETHOD,
                                new CodeExpression[]
                                {
                                    new CodePrimitiveExpression(literalParameter.GetDefaultValue()!/*not null for literals*/.ToString())
                                }
                            );
                    }
                    else
                    {
                        list.Add(new CodePrimitiveExpression(null));
                    }

                    return list;
                }

                switch (next)
                {
                    case LiteralParameter literalParameter:
                        Type literaltype = _enumHelper.GetSystemType(literalParameter.LiteralType);
                        list.Add(BuildParameterValue(parameterElement, _enumHelper.GetLiteralType(literaltype)));
                        break;
                    case ObjectParameter:
                        list.Add(BuildObject(_objectParameterDataParser.Parse(parameterElement)));
                        break;
                    case ListOfLiteralsParameter:
                        list.Add(BuildObject(_literalListParameterDataParser.Parse(parameterElement)));
                        break;
                    case ListOfObjectsParameter:
                        list.Add(BuildObject(_objectListParameterDataParser.Parse(parameterElement)));
                        break;
                    default:
                        throw _exceptionHelper.CriticalException("{4EEE0D10-C6E5-4359-88A5-41343E4077BB}");
                }

                return list;
            });
        }

        private CodeExpression BuildParameterValue(XmlNode textXmlNode, LiteralType literalType)
        {
            if (textXmlNode.ChildNodes.Count == 0)
            {
                return literalType switch
                {
                    LiteralType.NullableByte 
                    or LiteralType.NullableSByte 
                    or LiteralType.NullableChar 
                    or LiteralType.NullableUShort 
                    or LiteralType.NullableShort 
                    or LiteralType.NullableUInteger 
                    or LiteralType.NullableInteger 
                    or LiteralType.NullableULong 
                    or LiteralType.NullableLong 
                    or LiteralType.NullableFloat 
                    or LiteralType.NullableDouble 
                    or LiteralType.NullableDecimal 
                    or LiteralType.NullableDateTime 
                    or LiteralType.NullableDateTimeOffset 
                    or LiteralType.NullableDateOnly 
                    or LiteralType.NullableDate 
                    or LiteralType.NullableTimeSpan 
                    or LiteralType.NullableTimeOnly 
                    or LiteralType.NullableTimeOfDay 
                    or LiteralType.NullableGuid 
                    or LiteralType.NullableBoolean 
                        => new CodePrimitiveExpression(null),
                    LiteralType.String => new CodePrimitiveExpression(string.Empty),
                    _ => throw _exceptionHelper.CriticalException("{2C72E704-8BC1-4670-B17E-F3BE0178DF7A}"),
                };
            }

            if (textXmlNode.ChildNodes.Count == 1)
            {
                switch (textXmlNode.ChildNodes[0]!.NodeType)
                {
                    case XmlNodeType.Element:
                        XmlElement xmlElement = (XmlElement)textXmlNode.ChildNodes[0]!;
                        return xmlElement.Name switch
                        {
                            XmlDataConstants.VARIABLEELEMENT 
                            or XmlDataConstants.FUNCTIONELEMENT 
                            or XmlDataConstants.CONSTRUCTORELEMENT 
                                => GetNodeValue(xmlElement),
                            _ => throw _exceptionHelper.CriticalException("{C56AF6A6-E9D7-4D5A-81EF-3F3AB88865FA}"),
                        };
                    case XmlNodeType.Text:
                        XmlText xmlText = (XmlText)textXmlNode.ChildNodes[0]!;
                        return GetLiteralExpression(xmlText.Value!, literalType);
                    case XmlNodeType.Whitespace:
                        XmlWhitespace xmlWhitespace = (XmlWhitespace)textXmlNode.ChildNodes[0]!;
                        return new CodePrimitiveExpression(xmlWhitespace.Value);
                    default:
                        throw _exceptionHelper.CriticalException("{06E3F53C-2E2D-4687-88B1-85E5CD588C06}");
                }
            }

            string shortString = _resourcesManager.GetShortString(textXmlNode, this.resourceStrings, this.moduleName);
            CodeExpression getResourceMethod = BuildGetResorceFunction(shortString, _enumHelper.GetSystemType(literalType));
            return new CodeMethodInvokeExpression(new CodeThisReferenceExpression(), RuleFunctionConstants.VIRTUALFUNCTIONFORMATSTRING, new CodeExpression[] { getResourceMethod, GetFormatArgList(textXmlNode) });
        }

        private CodeExpression BuildPredicates(DecisionData decisionData)
        {
            return GetPredicates
            (
                decisionData.FirstChildElementName == XmlDataConstants.ORELEMENT
                    ? CodeBinaryOperatorType.BooleanOr
                    : CodeBinaryOperatorType.BooleanAnd
            );

            CodeExpression GetPredicates(CodeBinaryOperatorType operatorType)
                => AggregateConditions
                (
                    decisionData
                        .FunctionElements
                        .Select(element => _functionDataParser.Parse(element))
                        .Select(functionData => BuildIfCondition(functionData)),
                    operatorType
                );
        }

        private CodeExpression BuildSelectionLongString(XmlNode textXmlNode)
        {
            if (textXmlNode.ChildNodes.Count == 0)
                throw _exceptionHelper.CriticalException("{F263B7DE-FC3D-4711-BE59-7442AE44F89E}");

            if (textXmlNode.ChildNodes.Count == 1)
            {
                XmlNode singleChildNode = textXmlNode.ChildNodes[0]!;
                switch (singleChildNode.NodeType)
                {
                    case XmlNodeType.Element:
                        XmlElement xmlElement = (XmlElement)singleChildNode;
                        return xmlElement.Name switch
                        {
                            XmlDataConstants.VARIABLEELEMENT 
                            or XmlDataConstants.FUNCTIONELEMENT 
                            or XmlDataConstants.CONSTRUCTORELEMENT 
                                => GetNodeValue(xmlElement),
                            _ => throw _exceptionHelper.CriticalException("{BB0B2794-451A-47C2-A3D0-986C6FEE20B9}"),
                        };
                    case XmlNodeType.Text:
                        XmlText xmlText = (XmlText)singleChildNode;
                        return GetLiteralExpression(xmlText.Value!, LiteralType.String);
                    case XmlNodeType.Whitespace:
                        throw _exceptionHelper.CriticalException("{4B4A18ED-8A7A-4CFD-AA3E-EF43AF57CCA9}");
                    default:
                        throw _exceptionHelper.CriticalException("{34EC0040-E828-4528-8533-7943D29F0258}");
                }
            }

            string shortString = _resourcesManager.GetShortString(textXmlNode, this.resourceStrings, this.moduleName);
            CodeExpression getStringMethod = BuildGetResorceFunction(shortString, typeof(string));
            return new CodeMethodInvokeExpression(new CodeThisReferenceExpression(), RuleFunctionConstants.VIRTUALFUNCTIONFORMATSTRING, new CodeExpression[] { getStringMethod, GetFormatArgList(textXmlNode) });
        }

        CodeExpression BuildSelectionShortString(XmlNode textXmlNode)
        {
            if (textXmlNode.ChildNodes.Count == 0)
                throw _exceptionHelper.CriticalException("{71E84850-E556-4923-B987-996A83C892D1}");

            if (textXmlNode.ChildNodes.Count == 1)
            {
                XmlNode singleChildNode = textXmlNode.ChildNodes[0]!;
                switch (singleChildNode.NodeType)
                {
                    case XmlNodeType.Element:
                        XmlElement xmlElement = (XmlElement)singleChildNode;
                        return xmlElement.Name switch
                        {
                            XmlDataConstants.VARIABLEELEMENT 
                            or XmlDataConstants.FUNCTIONELEMENT
                            or XmlDataConstants.CONSTRUCTORELEMENT 
                                => GetNodeValue(xmlElement),
                            _ => throw _exceptionHelper.CriticalException("{8D790BDE-227C-408C-B2F1-A6F6D605B500}"),
                        };
                    case XmlNodeType.Text:
                        XmlText xmlText = (XmlText)singleChildNode;
                        return new CodePrimitiveExpression(_resourcesManager.GetShortString(xmlText.Value!, this.resourceStrings, this.moduleName));
                    case XmlNodeType.Whitespace:
                        throw _exceptionHelper.CriticalException("{65D50FF6-42B9-4755-9B32-823499316C18}");
                    default:
                        throw _exceptionHelper.CriticalException("{72701975-E7F3-416E-BE1B-AF631ED0E0C5}");
                }
            }

            return new CodePrimitiveExpression(_resourcesManager.GetShortString(textXmlNode, this.resourceStrings, this.moduleName));
        }

        private CodeExpression BuildVariable(string variableName)
        {
            if (!_configurationService.VariableList.Variables.TryGetValue(variableName, out VariableBase? variable))
                throw _exceptionHelper.CriticalException("{A4ED4971-1FA4-4B7E-8EEA-734439D743F8}");

            return IsCast()
                ? new CodeCastExpression(variable.CastVariableAs, GetExpression())
                : GetExpression();

            bool IsCast()
                => variable.CastVariableAs != MiscellaneousConstants.TILDE
                        && !string.IsNullOrEmpty(variable.CastVariableAs);

            CodeExpression GetExpression()
                => BuildImplementedVariableExpression(variable);
        }

        private CodeExpression BuildVariable(XmlElement variableNode, VariableBase variable)
        {
            switch (variable)
            {
                case LiteralVariable literalVariable:
                    Type literaltype = _enumHelper.GetSystemType(literalVariable.LiteralType);
                    return BuildParameterValue(variableNode, _enumHelper.GetLiteralType(literaltype));
                case ObjectVariable:
                    return BuildObject(_objectVariableDataParser.Parse(variableNode));
                case ListOfLiteralsVariable:
                    return BuildObject(_literalListVariableDataParser.Parse(variableNode));
                case ListOfObjectsVariable:
                    return BuildObject(_objectListVariableDataParser.Parse(variableNode));
                default:
                    throw _exceptionHelper.CriticalException("{17918A50-DBF6-4137-AF9C-7448EBBFC7F0}");
            }
        }

        private CodeExpression[] BuildVariableArrayIndicesExpression(string variableName)
        {
            string[] variableNames = variableName.Trim().Split(new char[] { MiscellaneousConstants.COMMA }, StringSplitOptions.RemoveEmptyEntries);
            CodeExpression[] arrayIndices = new CodeExpression[variableNames.Length];
            for (int i = 0; i < variableNames.Length; i++)
            {
                //IVariableValidationHelper.ValidateVariableIndirectReferenceName ensures that the variable is an integer or a type that can be implicitly cast to an integer.
                if (_configurationService.VariableList.Variables.TryGetValue(variableNames[i], out VariableBase? variable) && _variableHelper.CanBeInteger(variable))
                    arrayIndices[i] = BuildVariable(variableNames[i]);
                else//If not the int.Parse will throw an exception
                    arrayIndices[i] = new CodePrimitiveExpression(int.Parse(variableNames[i], CultureInfo.InvariantCulture));
            }

            return arrayIndices;
        }

        private CodeExpression GetFormatArgList(XmlNode parameterNode) 
            => BuildCollectionListExpression
            (
                parameterNode.ChildNodes.OfType<XmlNode>().Aggregate
                (
                    new List<CodeExpression>(),
                    (list, childNode) =>
                    {
                        switch (childNode.NodeType)
                        {
                            case XmlNodeType.Element:
                                XmlElement xmlElement = (XmlElement)childNode;
                                switch (xmlElement.Name)
                                {
                                    case XmlDataConstants.VARIABLEELEMENT:
                                    case XmlDataConstants.FUNCTIONELEMENT:
                                    case XmlDataConstants.CONSTRUCTORELEMENT:
                                        list.Add(GetNodeValue(xmlElement));
                                        break;
                                    default:
                                        throw _exceptionHelper.CriticalException("{47ACA83C-47B0-4854-AB89-965FB49AFF69}");
                                }
                                break;
                            case XmlNodeType.Text:
                            case XmlNodeType.Whitespace:
                                break;
                            default:
                                throw _exceptionHelper.CriticalException("{597FC4F6-4F19-4441-A305-9B3AF386DC4E}");
                        }

                        return list;
                    }
                ).ToArray()
            );



        private CodeExpression GetListExpression(ListType listType, Type literalType, CodeExpression[] arr)
        {
            return listType switch
            {
                ListType.GenericList or ListType.IGenericList => new CodeObjectCreateExpression
                (
                    new CodeTypeReference
                    (
                        RuleFunctionConstants.LISTCLASSNAME,
                        new CodeTypeReference[] { new CodeTypeReference(literalType) }
                    ),
                    new CodeArrayCreateExpression(literalType, arr)
                ),
                ListType.GenericCollection or ListType.IGenericCollection or ListType.IGenericEnumerable => new CodeObjectCreateExpression
                (
                    new CodeTypeReference
                    (
                        RuleFunctionConstants.COLLECTIONCLASSNAME,
                        new CodeTypeReference[] { new CodeTypeReference(literalType) }
                    ),
                    new CodeArrayCreateExpression(literalType, arr)
                ),
                ListType.Array => new CodeArrayCreateExpression(literalType, arr),
                _ => throw _exceptionHelper.CriticalException("{B9882B7A-0713-4ACF-BDA2-5D7854E76970}"),
            };
        }

        private CodeExpression GetLiteralExpression(string literalText, LiteralType literalType)
        {
            return literalType switch
            {
                LiteralType.Byte or LiteralType.NullableByte => new CodePrimitiveExpression(byte.Parse(literalText, CultureInfo.InvariantCulture)),
                LiteralType.SByte or LiteralType.NullableSByte => new CodePrimitiveExpression(sbyte.Parse(literalText, CultureInfo.InvariantCulture)),
                LiteralType.Char or LiteralType.NullableChar => new CodePrimitiveExpression(char.Parse(literalText)),
                LiteralType.UShort or LiteralType.NullableUShort => new CodePrimitiveExpression(ushort.Parse(literalText, CultureInfo.InvariantCulture)),
                LiteralType.Short or LiteralType.NullableShort => new CodePrimitiveExpression(short.Parse(literalText, CultureInfo.InvariantCulture)),
                LiteralType.UInteger or LiteralType.NullableUInteger => new CodePrimitiveExpression(uint.Parse(literalText, CultureInfo.InvariantCulture)),
                LiteralType.Integer or LiteralType.NullableInteger => new CodePrimitiveExpression(int.Parse(literalText, CultureInfo.InvariantCulture)),
                LiteralType.ULong or LiteralType.NullableULong => new CodePrimitiveExpression(ulong.Parse(literalText, CultureInfo.InvariantCulture)),
                LiteralType.Long or LiteralType.NullableLong => new CodePrimitiveExpression(long.Parse(literalText, CultureInfo.InvariantCulture)),
                LiteralType.Float or LiteralType.NullableFloat => new CodePrimitiveExpression(float.Parse(literalText, CultureInfo.InvariantCulture)),
                LiteralType.Double or LiteralType.NullableDouble => new CodePrimitiveExpression(double.Parse(literalText, CultureInfo.InvariantCulture)),
                LiteralType.Decimal or LiteralType.NullableDecimal => new CodePrimitiveExpression(decimal.Parse(literalText, CultureInfo.InvariantCulture)),
                LiteralType.DateTime or LiteralType.NullableDateTime => GetParseMethodInvokeExpression(typeof(DateTime)),
                LiteralType.DateTimeOffset or LiteralType.NullableDateTimeOffset => GetParseMethodInvokeExpression(typeof(DateTimeOffset)),
                LiteralType.DateOnly or LiteralType.NullableDateOnly => GetParseDateOnlyAndTimeOnlyMethodInvokeExpression(typeof(DateOnly)),
                LiteralType.Date or LiteralType.NullableDate => GetParseMethodInvokeExpression(typeof(Date)),
                LiteralType.TimeSpan or LiteralType.NullableTimeSpan => GetParseMethodInvokeExpression(typeof(TimeSpan)),
                LiteralType.TimeOnly or LiteralType.NullableTimeOnly => GetParseDateOnlyAndTimeOnlyMethodInvokeExpression(typeof(TimeOnly)),
                LiteralType.TimeOfDay or LiteralType.NullableTimeOfDay => GetParseMethodInvokeExpression(typeof(TimeOfDay)),
                LiteralType.Guid or LiteralType.NullableGuid => GetParseGuidMethodInvokeExpression(typeof(Guid)),
                LiteralType.Boolean or LiteralType.NullableBoolean => new CodePrimitiveExpression(bool.Parse(literalText)),
                LiteralType.String => BuildGetResorceFunction(_resourcesManager.GetShortString(literalText, this.resourceStrings, this.moduleName), typeof(string)),
                LiteralType.Void => throw _exceptionHelper.CriticalException("{4AE26DD7-C355-4874-A37E-18A1F5B2DCA5}"),
                _ => throw _exceptionHelper.CriticalException("{7EC2F1AF-8E24-4203-8C18-37214E7FD5ED}"),
            };

            CodeExpression GetParseMethodInvokeExpression(Type literalType)
                => new CodeMethodInvokeExpression
                (
                    new CodeTypeReferenceExpression(literalType),
                    PARSEMETHOD,
                    new CodeExpression[]
                    {
                        new CodePrimitiveExpression(literalText),
                        new CodePropertyReferenceExpression(new CodeTypeReferenceExpression(typeof(CultureInfo)), INVARIANTCULTUREPROPERTY)
                    }
                );

            CodeExpression GetParseDateOnlyAndTimeOnlyMethodInvokeExpression(Type literalType)
                => new CodeMethodInvokeExpression
                (
                    new CodeTypeReferenceExpression(literalType),
                    PARSEMETHOD,
                    new CodeExpression[]
                    {
                        new CodePrimitiveExpression(literalText),
                        new CodePropertyReferenceExpression(new CodeTypeReferenceExpression(typeof(CultureInfo)), INVARIANTCULTUREPROPERTY),
                        new CodeFieldReferenceExpression(new CodeTypeReferenceExpression(typeof(DateTimeStyles)), NONEFIELD)
                    }
                );

            CodeExpression GetParseGuidMethodInvokeExpression(Type literalType)
                => new CodeMethodInvokeExpression
                (
                    new CodeTypeReferenceExpression(literalType),
                    PARSEMETHOD,
                    new CodeExpression[]
                    {
                        new CodePrimitiveExpression(literalText)
                    }
                );
        }

        private CodeExpression GetNodeValue(XmlNode childNode)
        {
            switch (childNode.NodeType)
            {
                case XmlNodeType.Element:
                    XmlElement xmlElement = (XmlElement)childNode;
                    switch (xmlElement.Name)
                    {
                        case XmlDataConstants.VARIABLEELEMENT:
                            return BuildVariable(xmlElement.GetAttribute(XmlDataConstants.NAMEATTRIBUTE));
                        case XmlDataConstants.FUNCTIONELEMENT:
                            return GetFunction(_functionDataParser.Parse(xmlElement));
                            CodeExpression GetFunction(FunctionData functionData)
                            {
                                if (!_getValidConfigurationFromData.TryGetFunction(functionData, this.application, out Function? _))
                                    throw _exceptionHelper.CriticalException("{81383D9A-AF58-47DE-A642-565A445E648F}");

                                return BuildFunction(functionData);
                            }
                        case XmlDataConstants.CONSTRUCTORELEMENT:
                            return GetConstructor(_constructorDataParser.Parse(xmlElement));
                            CodeExpression GetConstructor(ConstructorData constructorData)
                            {
                                if (!_getValidConfigurationFromData.TryGetConstructor(constructorData, this.application, out Constructor? _))
                                    throw _exceptionHelper.CriticalException("{235FBF72-7B8B-41D6-A64D-85D2BE55160D}");

                                return BuildConstructor(constructorData);
                            }
                        default:
                            throw _exceptionHelper.CriticalException("{EB7B70AC-3867-492D-8F9D-FE28379C6421}");
                    }
                case XmlNodeType.Text:
                    XmlText xmlText = (XmlText)childNode;
                    return new CodePrimitiveExpression(xmlText.Value);
                case XmlNodeType.Whitespace:
                    XmlWhitespace xmlWhitespace = (XmlWhitespace)childNode;
                    return new CodePrimitiveExpression(xmlWhitespace.Value);
                default:
                    throw _exceptionHelper.CriticalException("{540F7A40-98A2-4678-B78D-40D47F991617}");
            }
        }

        private CodeExpression GetReferenceExpression(CodeExpression referencesReference, string referenceName, ValidIndirectReference referenceCategory)
        {
            return referenceCategory switch
            {
                ValidIndirectReference.Field => new CodeFieldReferenceExpression(referencesReference, referenceName),
                ValidIndirectReference.Property => new CodePropertyReferenceExpression(referencesReference, referenceName),
                ValidIndirectReference.StringKeyIndexer => new CodeIndexerExpression(referencesReference, new CodePrimitiveExpression(referenceName)),
                ValidIndirectReference.IntegerKeyIndexer => new CodeIndexerExpression(referencesReference, new CodePrimitiveExpression(int.Parse(referenceName, CultureInfo.InvariantCulture))),
                ValidIndirectReference.ArrayIndexer => new CodeArrayIndexerExpression(referencesReference, BuildArrayIndicesExpression(referenceName)),
                ValidIndirectReference.BooleanKeyIndexer => new CodeIndexerExpression(referencesReference, new CodePrimitiveExpression(bool.Parse(referenceName))),
                ValidIndirectReference.ByteKeyIndexer => new CodeIndexerExpression(referencesReference, new CodePrimitiveExpression(byte.Parse(referenceName, CultureInfo.InvariantCulture))),
                ValidIndirectReference.CharKeyIndexer => new CodeIndexerExpression(referencesReference, new CodePrimitiveExpression(char.Parse(referenceName))),
                ValidIndirectReference.DateTimeKeyIndexer => new CodeIndexerExpression(referencesReference, GetParseMethodInvokeExpression(typeof(DateTime))),
                ValidIndirectReference.DateTimeOffsetKeyIndexer => new CodeIndexerExpression(referencesReference, GetParseMethodInvokeExpression(typeof(DateTimeOffset))),
                ValidIndirectReference.DateOnlyKeyIndexer => new CodeIndexerExpression(referencesReference, GetParseDateOnlyAndTimeOnlyMethodInvokeExpression(typeof(DateOnly))),
                ValidIndirectReference.DateKeyIndexer => new CodeIndexerExpression(referencesReference, GetParseMethodInvokeExpression(typeof(Date))),
                ValidIndirectReference.TimeSpanKeyIndexer => new CodeIndexerExpression(referencesReference, GetParseMethodInvokeExpression(typeof(TimeSpan))),
                ValidIndirectReference.TimeOnlyKeyIndexer => new CodeIndexerExpression(referencesReference, GetParseDateOnlyAndTimeOnlyMethodInvokeExpression(typeof(TimeOnly))),
                ValidIndirectReference.TimeOfDayKeyIndexer => new CodeIndexerExpression(referencesReference, GetParseMethodInvokeExpression(typeof(TimeOfDay))),
                ValidIndirectReference.GuidKeyIndexer => new CodeIndexerExpression(referencesReference, GetParseGuidMethodInvokeExpression(typeof(Guid))),
                ValidIndirectReference.DecimalKeyIndexer => new CodeIndexerExpression(referencesReference, new CodePrimitiveExpression(decimal.Parse(referenceName, CultureInfo.InvariantCulture))),
                ValidIndirectReference.DoubleKeyIndexer => new CodeIndexerExpression(referencesReference, new CodePrimitiveExpression(double.Parse(referenceName, CultureInfo.InvariantCulture))),
                ValidIndirectReference.FloatKeyIndexer => new CodeIndexerExpression(referencesReference, new CodePrimitiveExpression(float.Parse(referenceName, CultureInfo.InvariantCulture))),
                ValidIndirectReference.LongKeyIndexer => new CodeIndexerExpression(referencesReference, new CodePrimitiveExpression(long.Parse(referenceName, CultureInfo.InvariantCulture))),
                ValidIndirectReference.SByteKeyIndexer => new CodeIndexerExpression(referencesReference, new CodePrimitiveExpression(sbyte.Parse(referenceName, CultureInfo.InvariantCulture))),
                ValidIndirectReference.ShortKeyIndexer => new CodeIndexerExpression(referencesReference, new CodePrimitiveExpression(short.Parse(referenceName, CultureInfo.InvariantCulture))),
                ValidIndirectReference.UIntegerKeyIndexer => new CodeIndexerExpression(referencesReference, new CodePrimitiveExpression(uint.Parse(referenceName, CultureInfo.InvariantCulture))),
                ValidIndirectReference.ULongKeyIndexer => new CodeIndexerExpression(referencesReference, new CodePrimitiveExpression(ulong.Parse(referenceName, CultureInfo.InvariantCulture))),
                ValidIndirectReference.UShortKeyIndexer => new CodeIndexerExpression(referencesReference, new CodePrimitiveExpression(ushort.Parse(referenceName, CultureInfo.InvariantCulture))),
                ValidIndirectReference.VariableKeyIndexer => new CodeIndexerExpression(referencesReference, BuildVariable(referenceName)),
                ValidIndirectReference.VariableArrayIndexer => new CodeArrayIndexerExpression(referencesReference, BuildVariableArrayIndicesExpression(referenceName)),
                _ => throw _exceptionHelper.CriticalException("{EA025684-3832-4397-9DC6-B9626EA03C3E}")
            };

            CodeExpression GetParseMethodInvokeExpression(Type literalType)
                => new CodeMethodInvokeExpression
                (
                    new CodeTypeReferenceExpression(literalType),
                    PARSEMETHOD,
                    new CodeExpression[]
                    {
                        new CodePrimitiveExpression(referenceName),
                        new CodePropertyReferenceExpression(new CodeTypeReferenceExpression(typeof(CultureInfo)), INVARIANTCULTUREPROPERTY)
                    }
                );

            CodeExpression GetParseDateOnlyAndTimeOnlyMethodInvokeExpression(Type literalType)
                => new CodeMethodInvokeExpression
                (
                    new CodeTypeReferenceExpression(literalType),
                    PARSEMETHOD,
                    new CodeExpression[]
                    {
                        new CodePrimitiveExpression(referenceName),
                        new CodePropertyReferenceExpression(new CodeTypeReferenceExpression(typeof(CultureInfo)), INVARIANTCULTUREPROPERTY),
                        new CodeFieldReferenceExpression(new CodeTypeReferenceExpression(typeof(DateTimeStyles)), NONEFIELD)
                    }
                );

            CodeExpression GetParseGuidMethodInvokeExpression(Type literalType)
                => new CodeMethodInvokeExpression
                (
                    new CodeTypeReferenceExpression(literalType),
                    PARSEMETHOD,
                    new CodeExpression[]
                    {
                        new CodePrimitiveExpression(referenceName)
                    }
                );
        }

        private CodeExpression GetReferenceExpression(Function function)
        {
            if (function.ReferenceDefinition.Count != function.ReferenceName.Count)
                throw _exceptionHelper.CriticalException("{F67CF2EE-7AA6-48D3-B324-34C7CC4FA3E8}");

            if (function.CastReferenceAsList.Count != function.ReferenceName.Count)
                throw _exceptionHelper.CriticalException("{FD0F2944-E166-468C-A857-E4E5553F0CC2}");

            CodeExpression? referenceExpression = null;
            switch (function.ReferenceCategory)
            {
                case ReferenceCategories.This:
                    return new CodeThisReferenceExpression();
                case ReferenceCategories.Type:
                    return new CodeTypeReferenceExpression(function.TypeName);
                case ReferenceCategories.InstanceReference:
                    for (int i = 0; i < function.ReferenceName.Count; i++)
                    {
                        if (i == 0)
                            referenceExpression = GetReferenceExpression(new CodeThisReferenceExpression(), function.ReferenceName[i], function.ReferenceDefinition[i]);
                        else
                            referenceExpression = GetReferenceExpression(referenceExpression!, function.ReferenceName[i], function.ReferenceDefinition[i]);

                        if (function.CastReferenceAsList[i] != MiscellaneousConstants.TILDE)
                            referenceExpression = new CodeCastExpression(function.CastReferenceAsList[i], referenceExpression);
                    }
                    return referenceExpression!;
                case ReferenceCategories.StaticReference:
                    for (int i = 0; i < function.ReferenceName.Count; i++)
                    {
                        if (i == 0)
                            referenceExpression = GetReferenceExpression(new CodeTypeReferenceExpression(function.TypeName), function.ReferenceName[i], function.ReferenceDefinition[i]);
                        else
                            referenceExpression = GetReferenceExpression(referenceExpression!, function.ReferenceName[i], function.ReferenceDefinition[i]);

                        if (function.CastReferenceAsList[i] != MiscellaneousConstants.TILDE)
                            referenceExpression = new CodeCastExpression(function.CastReferenceAsList[i], referenceExpression);
                    }
                    return referenceExpression!;
                case ReferenceCategories.None:
                    throw _exceptionHelper.CriticalException("{3B60D79C-9AA7-4A12-9211-2AB239C4F9EA}");
                default:
                    throw _exceptionHelper.CriticalException("{05D1736A-B077-40DA-B2C4-FDA9C0698F63}");
            }
        }

        private CodeExpression GetReferenceExpression(VariableBase variable)
        {
            if (variable.ReferenceDefinition.Count != variable.ReferenceName.Count)
                throw _exceptionHelper.CriticalException("{B3E5CEB0-38FB-4EAC-AC94-50C31149B262}");

            if (variable.CastReferenceAsList.Count != variable.ReferenceName.Count)
                throw _exceptionHelper.CriticalException("{366D0C0F-A7E6-4A44-AC4D-543DA5114182}");

            CodeExpression? referenceExpression = null;
            switch (variable.ReferenceCategory)
            {
                case ReferenceCategories.This:
                    return new CodeThisReferenceExpression();
                case ReferenceCategories.Type:
                    return new CodeTypeReferenceExpression(variable.TypeName);
                case ReferenceCategories.InstanceReference:
                    for (int i = 0; i < variable.ReferenceName.Count; i++)
                    {
                        if (i == 0)
                            referenceExpression = GetReferenceExpression(new CodeThisReferenceExpression(), variable.ReferenceName[i], variable.ReferenceDefinition[i]);
                        else
                            referenceExpression = GetReferenceExpression(referenceExpression!, variable.ReferenceName[i], variable.ReferenceDefinition[i]);

                        if (variable.CastReferenceAsList[i] != MiscellaneousConstants.TILDE)
                            referenceExpression = new CodeCastExpression(variable.CastReferenceAsList[i], referenceExpression);
                    }
                    return referenceExpression!;
                case ReferenceCategories.StaticReference:
                    for (int i = 0; i < variable.ReferenceName.Count; i++)
                    {
                        if (i == 0)
                            referenceExpression = GetReferenceExpression(new CodeTypeReferenceExpression(variable.TypeName), variable.ReferenceName[i], variable.ReferenceDefinition[i]);
                        else
                            referenceExpression = GetReferenceExpression(referenceExpression!, variable.ReferenceName[i], variable.ReferenceDefinition[i]);

                        if (variable.CastReferenceAsList[i] != MiscellaneousConstants.TILDE)
                            referenceExpression = new CodeCastExpression(variable.CastReferenceAsList[i], referenceExpression);
                    }
                    return referenceExpression!;
                case ReferenceCategories.None:
                    throw _exceptionHelper.CriticalException("{8E5F951C-9351-4E1C-A591-2891077B359F}");
                default:
                    throw _exceptionHelper.CriticalException("{2B708346-B3F4-4791-8ED5-AA7F1D801A77}");
            }
        }
    }
}
