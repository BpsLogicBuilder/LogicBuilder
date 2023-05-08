using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml;
using System.Xml.Schema;

namespace ABIS.LogicBuilder.FlowBuilder.XmlValidation
{
    internal static class Schemas
    {
        static Schemas()
        {
            shapeDataSchema = CreateShapeDataSchema();
            connectorDataSchema = CreateConnectorDataSchema();
            variablesSchema = CreateVariablesSchema();
            decisionsDataSchema = CreateDecisionsDataSchema();
            conditionsDataSchema = CreateConditionsDataSchema();
            functionDataSchema = CreateFunctionDataSchema();
            functionsSchema = CreateFunctionsSchema();
            functionsDataSchema = CreateFunctionsDataSchema();
            projectPropertiesSchema = CreateProjectPropertiesSchema();
            tableSchema = CreateTableSchema();
            constructorSchema = CreateConstructorSchema();
            parametersDataSchema = CreateParametersDataSchema();
            fragmentsSchema = CreateFragmentsSchema();

            //WriteSchema(@"C:\Test\shapeData.xsd", ShapeDataSchema);
            //WriteSchema("C:\\Test\\connectorData.xsd", ConnectorDataSchema);
            //WriteSchema("C:\\Test\\parametersData.xsd", ParametersDataSchema);
            //WriteSchema("C:\\Test\\fragments.xsd", FragmentsSchema);
            //WriteSchema("C:\\Test\\conditionsData.xsd", ConditionsDataSchema);
            //WriteSchema("C:\\Test\\decisionsData.xsd", DecisionsDataSchema);
            //WriteSchema("C:\\Test\\functionData.xsd", FunctionDataSchema);
            //WriteSchema("C:\\Test\\functionsData.xsd", FunctionsDataSchema);
            //WriteSchema(@"C:\Test\sourceTable.xsd", TableSchema);
            //WriteSchema("C:\\Test\\functions.xsd", FunctionsSchema);
            //WriteSchema("C:\\Test\\constructors.xsd", ConstructorSchema);
            //WriteSchema("C:\\Test\\ProjectProperties.xsd", ProjectPropertiesSchema);
            //WriteSchema("C:\\Test\\variables.xsd", VariablesSchema);
        }

        #region Variables
        private static readonly XmlSchema shapeDataSchema;
        private static readonly XmlSchema connectorDataSchema;
        private static readonly XmlSchema variablesSchema;
        private static readonly XmlSchema decisionsDataSchema;
        private static readonly XmlSchema conditionsDataSchema;
        private static readonly XmlSchema functionDataSchema;
        private static readonly XmlSchema functionsSchema;
        private static readonly XmlSchema functionsDataSchema;
        private static readonly XmlSchema projectPropertiesSchema;
        private static readonly XmlSchema tableSchema;
        private static readonly XmlSchema constructorSchema;
        private static readonly XmlSchema parametersDataSchema;
        private static readonly XmlSchema fragmentsSchema;
        private static readonly List<string> xmlSchemaErrors = new();
        #endregion Variables

        #region Properties
        internal static XmlSchema ShapeDataSchema => shapeDataSchema;
        internal static XmlSchema ConnectorDataSchema => connectorDataSchema;
        internal static XmlSchema VariablesSchema => variablesSchema;
        internal static XmlSchema DecisionsDataSchema => decisionsDataSchema;
        internal static XmlSchema ConditionsDataSchema => conditionsDataSchema;
        internal static XmlSchema FunctionDataSchema => functionDataSchema;
        internal static XmlSchema FunctionsSchema => functionsSchema;
        internal static XmlSchema FunctionsDataSchema => functionsDataSchema;
        internal static XmlSchema ProjectPropertiesSchema => projectPropertiesSchema;
        internal static XmlSchema TableSchema => tableSchema;
        internal static XmlSchema ConstructorSchema => constructorSchema;
        internal static XmlSchema ParametersDataSchema => parametersDataSchema;
        internal static XmlSchema FragmentsSchema => fragmentsSchema;
        #endregion Properties

        #region Methods
        private static string GetXmlSchemaValidationErrors()
        {
            string errors = string.Join(Environment.NewLine, xmlSchemaErrors);
            xmlSchemaErrors.Clear();
            return errors;
        }

        private static XmlSchema CreateShapeDataSchema()
        {
            //shapeData Element
            XmlSchemaElement elementShapeData = CreateSchemaElement("shapeData", "dataType");

            //dataType ComplexType
            XmlSchemaComplexType dataType = CreateComplexType("dataType",
                new XmlSchemaObject[]
                {
                    CreateSchemaElement("value", "string", 1, null, true, true)
                },
                new XmlSchemaAttribute[]
                {
                    AttributeName
                });

            return CreateCompiledXmlSchemaSet(new XmlSchemaObject[]
            {
                elementShapeData,
                dataType
            });

            //WriteSchema(@"C:\Test\shapeData.xsd", ShapeDataSchema);
        }
        #region Common Reusable Elements
        private static XmlSchemaAttribute AttributeName => CreateRequiredAttribute("name", "string", true);
        private static XmlSchemaAttribute AttributeDescription => CreateOptionalAttribute("description", "string", true);
        private static XmlSchemaAttribute AttributeVisibleText => CreateRequiredAttribute("visibleText", "string", true);
        #endregion Common  Elements

        #region Data Reusable Elements
        private static XmlSchemaComplexType LiteralListItemType => CreateComplexChoiceType("literalListItemType",
                new XmlSchemaElement[]
                {
                    CreateSchemaElement("variable", "variableType"),
                    CreateSchemaElement("function", "functionType"),
                    CreateSchemaElement("constructor", "constructorType")
                },
                null, 0, null, true, true);

        //objectListItemType
        private static XmlSchemaComplexType ObjectListItemType => CreateComplexChoiceType("objectListItemType",
                new XmlSchemaObject[]
                {
                    CreateSchemaElement("variable", "variableType"),
                    CreateSchemaElement("function", "functionType"),
                    CreateSchemaElement("constructor", "constructorType"),
                    CreateSchemaElement("literalList", "literalListType"),
                    CreateSchemaElement("objectList", "objectListType")
                },
                null, 1, 1);

        //literalParameterType
        private static XmlSchemaComplexType LiteralParameterType => CreateComplexChoiceType("literalParameterType",
                new XmlSchemaElement[]
                {
                    CreateSchemaElement("variable", "variableType"),
                    CreateSchemaElement("function", "functionType"),
                    CreateSchemaElement("constructor", "constructorType")
                },
                new XmlSchemaAttribute[] { AttributeName }, 0, null, true, true);

        //objectParameterType
        private static XmlSchemaComplexType ObjectParameterType => CreateComplexChoiceType("objectParameterType",
                new XmlSchemaObject[]
                {
                    CreateSchemaElement("variable", "variableType"),
                    CreateSchemaElement("function", "functionType"),
                    CreateSchemaElement("constructor", "constructorType"),
                    CreateSchemaElement("literalList", "literalListType"),
                    CreateSchemaElement("objectList", "objectListType")
                },
                new XmlSchemaAttribute[] { AttributeName }, 1, 1);

        //literalListParameterType
        private static XmlSchemaComplexType LiteralListParameterType => CreateComplexChoiceType("literalListParameterType",
                new XmlSchemaObject[]
                {
                    CreateSchemaElement("variable", "variableType"),
                    CreateSchemaElement("function", "functionType"),
                    CreateSchemaElement("constructor", "constructorType"),
                    CreateSchemaElement("literalList", "literalListType"),
                    CreateSchemaElement("objectList", "objectListType")
                },
                new XmlSchemaAttribute[] { AttributeName }, 1, 1);

        //objectListParameterType
        private static XmlSchemaComplexType ObjectListParameterType => CreateComplexChoiceType("objectListParameterType",
                new XmlSchemaObject[]
                {
                    CreateSchemaElement("variable", "variableType"),
                    CreateSchemaElement("function", "functionType"),
                    CreateSchemaElement("constructor", "constructorType"),
                    CreateSchemaElement("literalList", "literalListType"),
                    CreateSchemaElement("objectList", "objectListType")
                },
                new XmlSchemaAttribute[] { AttributeName }, 1, 1);

        //literalVariableType
        private static XmlSchemaComplexType LiteralVariableType => CreateComplexChoiceType("literalVariableType",
                new XmlSchemaElement[]
                {
                    CreateSchemaElement("variable", "variableType"),
                    CreateSchemaElement("function", "functionType"),
                    CreateSchemaElement("constructor", "constructorType")
                },
                null, 0, null, true, true);

        //objectVariableType
        private static XmlSchemaComplexType ObjectVariableType => CreateComplexChoiceType("objectVariableType",
                new XmlSchemaObject[]
                {
                    CreateSchemaElement("variable", "variableType"),
                    CreateSchemaElement("function", "functionType"),
                    CreateSchemaElement("constructor", "constructorType"),
                    CreateSchemaElement("literalList", "literalListType"),
                    CreateSchemaElement("objectList", "objectListType")
                },
                null, 1, 1);

        //literalListVariableType
        private static XmlSchemaComplexType LiteralListVariableType => CreateComplexChoiceType("literalListVariableType",
                new XmlSchemaObject[]
                {
                    CreateSchemaElement("variable", "variableType"),
                    CreateSchemaElement("function", "functionType"),
                    CreateSchemaElement("constructor", "constructorType"),
                    CreateSchemaElement("literalList", "literalListType"),
                    CreateSchemaElement("objectList", "objectListType")
                },
                null, 1, 1);

        //objectListVariableType
        private static XmlSchemaComplexType ObjectListVariableType => CreateComplexChoiceType("objectListVariableType",
                new XmlSchemaObject[]
                {
                    CreateSchemaElement("variable", "variableType"),
                    CreateSchemaElement("function", "functionType"),
                    CreateSchemaElement("constructor", "constructorType"),
                    CreateSchemaElement("literalList", "literalListType"),
                    CreateSchemaElement("objectList", "objectListType")
                },
                null, 1, 1);

        //objectListType
        private static XmlSchemaComplexType ObjectListType => CreateComplexType("objectListType",
                new XmlSchemaObject[]
                {
                    CreateSchemaElement("object", "objectListItemType", 0, null, true)
                },
                new XmlSchemaAttribute[] { AttributeObjectType, AttributeListType, AttributeVisibleText });

        //literalListType
        private static XmlSchemaComplexType LiteralListType => CreateComplexType("literalListType",
                new XmlSchemaObject[]
                {
                    CreateSchemaElement("literal", "literalListItemType", 0, null, true)
                },
                new XmlSchemaAttribute[] { AttributeLiteralType, AttributeListType, AttributeVisibleText });

        private static XmlSchemaComplexType FunctionType => CreateComplexType("functionType",
                new XmlSchemaElement[]
                {
                    CreateSchemaElement("genericArguments", "genericArgumentsType", 1, 1, false, false, new XmlSchemaObject[]
                    {
                        CreateUniqueConstraint("genericArgumentNameKey1", "./literalParameter|./objectParameter|./literalListParameter|./objectListParameter", "@genericArgumentName")
                    }),
                    CreateSchemaElement("parameters", "parametersType", 1, 1)
                },
                new XmlSchemaAttribute[] { AttributeName, AttributeVisibleText });

        //variableType ComplexType
        private static XmlSchemaComplexType VariableType => CreateComplexType("variableType",
            null,
            new XmlSchemaAttribute[] { AttributeName, AttributeVisibleText });

        //constructorType ComplexType
        private static XmlSchemaComplexType ConstructorType => CreateComplexType("constructorType",
            new XmlSchemaElement[]
            {
                    CreateSchemaElement("genericArguments", "genericArgumentsType", 1, 1, false, false, new XmlSchemaObject[]
                    {
                        CreateUniqueConstraint("genericArgumentNameKey", "./literalParameter|./objectParameter|./literalListParameter|./objectListParameter", "@genericArgumentName")
                    }),
                    CreateSchemaElement("parameters", "parametersType", 1, 1)
            },
            new XmlSchemaAttribute[] { AttributeName, AttributeVisibleText });

        //parametersType ComplexType
        private static XmlSchemaComplexType ParametersType => CreateComplexChoiceType("parametersType",
            new XmlSchemaElement[]
            {
                    CreateSchemaElement("literalParameter", "literalParameterType"),
                    CreateSchemaElement("objectParameter", "objectParameterType"),
                    CreateSchemaElement("literalListParameter", "literalListParameterType"),
                    CreateSchemaElement("objectListParameter", "objectListParameterType")
            },
            null, 0, null, true);

        //genericArgumentsType ComplexType
        private static XmlSchemaComplexType GenericArgumentsType => CreateComplexChoiceType("genericArgumentsType",
            new XmlSchemaElement[]
            {
                    CreateSchemaElement("literalParameter", "literalGenericConfigType"),
                    CreateSchemaElement("objectParameter", "objectGenericConfigType"),
                    CreateSchemaElement("literalListParameter", "literalListGenericConfigType"),
                    CreateSchemaElement("objectListParameter", "objectListGenericConfigType")
            },
            null, 0, null, true);

        //assertFunctionType ComplexType
        private static XmlSchemaComplexType AssertFunctionType => CreateComplexType("assertFunctionType",
            new XmlSchemaElement[]
            {
                    CreateSchemaElement("variable", "variableType", 1, 1),
                    CreateSchemaElement("variableValue", "variableValueType", 1, 1)
            },
            new XmlSchemaAttribute[] { AttributeName, AttributeVisibleText });

        //variableValueType ComplexType
        private static XmlSchemaComplexType VariableValueType => CreateComplexChoiceType("variableValueType",
           new XmlSchemaObject[]
           {
                    CreateSchemaElement("literalVariable", "literalVariableType"),
                    CreateSchemaElement("objectVariable", "objectVariableType"),
                    CreateSchemaElement("literalListVariable", "literalListVariableType"),
                    CreateSchemaElement("objectListVariable", "objectListVariableType")
           },
           null, 1, 1);

        //literalGenericConfigType ComplexType
        private static XmlSchemaComplexType LiteralGenericConfigType => CreateComplexType("literalGenericConfigType",
            new XmlSchemaElement[]
            {
                    CreateSchemaElement("literalType", "LiteralType", 1, 1),
                    CreateSchemaElement("control", "LiteralInputStyle", 1, 1),
                    CreateSchemaElement("useForEquality", "boolean", 1, 1, false, true),
                    CreateSchemaElement("useForHashCode", "boolean", 1, 1, false, true),
                    CreateSchemaElement("useForToString", "boolean", 1, 1, false, true),
                    CreateSchemaElement("propertySource", "string", 1, 1, false, true),
                    CreateSchemaElement("propertySourceParameter", "string", 1, 1, false, true),
                    CreateSchemaElement("defaultValue", "string", 1, 1, false, true),
                    CreateSchemaElement("domain", "domainType", 1, 1, false, false, new XmlSchemaObject[] { CreateUniqueConstraint("itemValue", "./item", ".") })
            },
            new XmlSchemaAttribute[] { AttributeGenericArgumentName });

        //objectGenericConfigType ComplexType
        private static XmlSchemaComplexType ObjectGenericConfigType => CreateComplexType("objectGenericConfigType",
            new XmlSchemaElement[]
            {
                    CreateSchemaElement("objectType", "AsemblyQualifiedNameType", 1, 1),
                    CreateSchemaElement("useForEquality", "boolean", 1, 1, false, true),
                    CreateSchemaElement("useForHashCode", "boolean", 1, 1, false, true),
                    CreateSchemaElement("useForToString", "boolean", 1, 1, false, true)
            },
            new XmlSchemaAttribute[] { AttributeGenericArgumentName });

        //literalListGenericConfigType ComplexType
        private static XmlSchemaComplexType LiteralListGenericConfigType => CreateComplexType("literalListGenericConfigType",
            new XmlSchemaElement[]
            {
                    CreateSchemaElement("literalType", "LiteralType", 1, 1),
                    CreateSchemaElement("listType", "ListType", 1, 1),
                    CreateSchemaElement("control", "ListInputStyle", 1, 1),
                    CreateSchemaElement("elementControl", "LiteralInputStyle", 1, 1),
                    CreateSchemaElement("propertySource", "string", 1, 1, false, true),
                    CreateSchemaElement("propertySourceParameter", "string", 1, 1, false, true),
                    CreateSchemaElement("defaultValue", "domainType", 1, 1, false, false,
                                            new XmlSchemaObject[]
                                            {
                                               CreateUniqueConstraint("itemValue1", "./item", ".")
                                            }),
                    CreateSchemaElement("domain", "domainType", 1, 1, false, false,
                                            new XmlSchemaObject[]
                                            {
                                               CreateUniqueConstraint("itemValue2", "./item", ".")
                                            })
            },
            new XmlSchemaAttribute[] { AttributeGenericArgumentName });

        //objectListGenericConfigType ComplexType
        private static XmlSchemaComplexType ObjectListGenericConfigType => CreateComplexType("objectListGenericConfigType",
            new XmlSchemaElement[]
            {
                    CreateSchemaElement("objectType", "AsemblyQualifiedNameType", 1, 1),
                    CreateSchemaElement("listType", "ListType", 1, 1),
                    CreateSchemaElement("control", "ListInputStyle", 1, 1)
            },
            new XmlSchemaAttribute[] { AttributeGenericArgumentName });

        private static XmlSchemaComplexType TextType => CreateComplexChoiceType("textType",
                new XmlSchemaElement[]
                {
                    CreateSchemaElement("variable", "variableType"),
                    CreateSchemaElement("function", "functionType"),
                    CreateSchemaElement("constructor", "constructorType")
                },
                null, 0, null, true, true);

        //metaObjectType
        private static XmlSchemaComplexType MetaObjectType => CreateComplexChoiceType("metaObjectType",
                new XmlSchemaObject[]
                {
                    CreateSchemaElement("variable", "variableType"),
                    CreateSchemaElement("function", "functionType"),
                    CreateSchemaElement("constructor", "constructorType"),
                    CreateSchemaElement("literalList", "literalListType"),
                    CreateSchemaElement("objectList", "objectListType")
                },
                new XmlSchemaAttribute[] { AttributeObjectType }, 1, 1);

        private static XmlSchemaComplexType DomainType => CreateComplexType("domainType",
                new XmlSchemaElement[]
                {
                    CreateSchemaElement("item", "string", 0, null, true, true)
                },
                null);

        //genericArgumentName Attribute
        private static XmlSchemaAttribute AttributeGenericArgumentName => CreateRequiredAttribute("genericArgumentName", "string", true);

        //literalType Attribute
        private static XmlSchemaAttribute AttributeLiteralType => CreateRequiredAttribute("literalType", "ParameterArgumentType");

        //objectType Attribute
        private static XmlSchemaAttribute AttributeObjectType => CreateRequiredAttribute("objectType", "AsemblyQualifiedNameType");

        //listType Attribute
        private static XmlSchemaAttribute AttributeListType => CreateRequiredAttribute("listType", "ListType");

        //LiteralType
        private static XmlSchemaSimpleType LiteralType => CreateEnumRestriction("LiteralType", new string[]
        {
                "Decimal",
                "String",
                "Integer",
                "Boolean",
                "DateTime",
                "DateTimeOffset",
                "DateOnly",
                "Date",
                "TimeSpan",
                "TimeOnly",
                "TimeOfDay",
                "Guid",
                "Byte",
                "Short",
                "Long",
                "Float",
                "Double",
                "Char",
                "SByte",
                "UShort",
                "UInteger",
                "ULong",
                "NullableDecimal",
                "NullableInteger",
                "NullableBoolean",
                "NullableDateTime",
                "NullableDateTimeOffset",
                "NullableDateOnly",
                "NullableDate",
                "NullableTimeSpan",
                "NullableTimeOnly",
                "NullableTimeOfDay",
                "NullableGuid",
                "NullableByte",
                "NullableShort",
                "NullableLong",
                "NullableFloat",
                "NullableDouble",
                "NullableChar",
                "NullableSByte",
                "NullableUShort",
                "NullableUInteger",
                "NullableULong"
        });

        //LiteralTypeExt
        private static XmlSchemaSimpleType LiteralTypeExt => CreateEnumRestriction("LiteralTypeExt", new string[]
        {
                "Any"
        });

        //ParameterArgumentType
        private static XmlSchemaSimpleType ParameterArgumentType => CreateSimpleTypeUnion("ParameterArgumentType", "LiteralType", "LiteralTypeExt");

        //AsemblyQualifiedNameType
        private static XmlSchemaSimpleType AssemblyQualifiedNameType => CreatePatternRestriction("AsemblyQualifiedNameType", RegularExpressions.FULLYQUALIFIEDCLASSNAME);

        //LiteralInputStyle
        private static XmlSchemaSimpleType LiteralInputStyle => CreateEnumRestriction("LiteralInputStyle", new string[]
        {
                "SingleLineTextBox",
                "MultipleLineTextBox",
                "DropDown",
                "TypeAutoComplete",
                "DomainAutoComplete",
                "PropertyInput",
                "ParameterSourcedPropertyInput",
                "ParameterSourceOnly"
        });

        //ListInputStyle
        private static XmlSchemaSimpleType ListInputStyle => CreateEnumRestriction("ListInputStyle", new string[]
        {
                "ListForm",
                "HashSetForm"
        });

        //ListType
        private static XmlSchemaSimpleType ListType => CreateEnumRestriction("ListType", new string[]
        {
            "Array",
            "GenericList",
            "GenericCollection",
            "IGenericList",
            "IGenericCollection",
            "IGenericEnumerable"
        });
        #endregion Data Reusable Elements

        private static XmlSchema CreateConnectorDataSchema()
        {
            //connector Element
            XmlSchemaElement elementConnnector = CreateSchemaElement("connector", "connectorType");

            //connectorType ComplexType
            XmlSchemaComplexType connectorType = CreateComplexType("connectorType",
                new XmlSchemaObject[]
                {
                    CreateSchemaElement("text", "textType", 1, 1),
                    CreateSchemaElement("metaObject", "metaObjectType", 0, 1)
                },
                new XmlSchemaAttribute[]
                {
                    CreateRequiredAttribute("name", "short", true),
                    CreateRequiredAttribute("connectorCategory", "short", true)
                });

            return CreateCompiledXmlSchemaSet(new XmlSchemaObject[]
            {
                elementConnnector,
                connectorType,
                TextType,
                LiteralListItemType,
                ObjectListItemType,
                LiteralParameterType,
                ObjectParameterType,
                LiteralListParameterType,
                ObjectListParameterType,
                ObjectListType,
                LiteralListType,
                FunctionType,
                VariableType,
                ConstructorType,
                ParametersType,
                GenericArgumentsType,
                LiteralGenericConfigType,
                ObjectGenericConfigType,
                LiteralListGenericConfigType,
                ObjectListGenericConfigType,
                MetaObjectType,
                DomainType,
                LiteralType,
                LiteralTypeExt,
                ParameterArgumentType,
                AssemblyQualifiedNameType,
                LiteralInputStyle,
                ListInputStyle,
                ListType
            });

            //WriteSchema("C:\\Test\\connectorData.xsd", ConnectorDataSchema);
        }

        private static XmlSchema CreateParametersDataSchema()
        {
            //function Element
            XmlSchemaElement elementFunction = CreateSchemaElement("function", "functionType");

            //constructor Element
            XmlSchemaElement elementConstructor = CreateSchemaElement("constructor", "constructorType");

            //literalList Element
            XmlSchemaElement elementLiteralList = CreateSchemaElement("literalList", "literalListType");

            //objectList Element
            XmlSchemaElement elementObjectList = CreateSchemaElement("objectList", "objectListType");

            return CreateCompiledXmlSchemaSet(new XmlSchemaObject[]
            {
                elementFunction,
                elementConstructor,
                elementLiteralList,
                elementObjectList,
                LiteralListItemType,
                ObjectListItemType,
                LiteralParameterType,
                ObjectParameterType,
                LiteralListParameterType,
                ObjectListParameterType,
                ObjectListType,
                LiteralListType,
                ConstructorType,
                FunctionType,
                VariableType,
                ParametersType,
                GenericArgumentsType,
                LiteralGenericConfigType,
                ObjectGenericConfigType,
                LiteralListGenericConfigType,
                ObjectListGenericConfigType,
                DomainType,
                LiteralType,
                LiteralTypeExt,
                ParameterArgumentType,
                AssemblyQualifiedNameType,
                LiteralInputStyle,
                ListInputStyle,
                ListType
            });

            //WriteSchema("C:\\Test\\parametersData.xsd", ParametersDataSchema);
        }

        private static XmlSchema CreateFragmentsSchema()
        {
            //folder Element
            XmlSchemaElement elementFolder = CreateSchemaElement("folder", "folderType", false, new XmlSchemaObject[]
            {
                CreateUniqueConstraint("fragmentNameKey", ".//fragment", "@name"),
                CreateUniqueConstraint("folderNameKey", "./folder", "@name")
            });

            //subFolder Element
            XmlSchemaElement elementSubFolder = CreateSchemaElement("folder", "folderType", 0, null, true, false, new XmlSchemaObject[]
            {
                CreateUniqueConstraint("subFolderNameKey", "./folder", "@name")
            });

            XmlSchemaElement fragmentElement = CreateSchemaElement("fragment", "fragmentType", 0, null, true);

            //folderType ComplexType
            XmlSchemaComplexType folderType = CreateComplexType("folderType",
                new XmlSchemaObject[]
                {

                    fragmentElement,
                    elementSubFolder
                },
                new XmlSchemaAttribute[]
                {
                    AttributeName
                });

            XmlSchemaComplexType fragmentType = CreateComplexChoiceType("fragmentType",
                new XmlSchemaObject[]
                {
                    CreateSchemaElement("variable", "variableType"),
                    CreateSchemaElement("function", "functionType"),
                    CreateSchemaElement("constructor", "constructorType"),
                    CreateSchemaElement("literalList", "literalListType"),
                    CreateSchemaElement("objectList", "objectListType")
                },
                new XmlSchemaAttribute[] { AttributeName, AttributeDescription }, 1, 1);


            return CreateCompiledXmlSchemaSet(new XmlSchemaObject[]
            {
                elementFolder,
                folderType,
                fragmentType,
                LiteralListItemType,
                ObjectListItemType,
                LiteralParameterType,
                ObjectParameterType,
                LiteralListParameterType,
                ObjectListParameterType,
                ObjectListType,
                LiteralListType,
                ConstructorType,
                FunctionType,
                VariableType,
                ParametersType,
                GenericArgumentsType,
                LiteralGenericConfigType,
                ObjectGenericConfigType,
                LiteralListGenericConfigType,
                ObjectListGenericConfigType,
                DomainType,
                LiteralType,
                LiteralTypeExt,
                ParameterArgumentType,
                AssemblyQualifiedNameType,
                LiteralInputStyle,
                ListInputStyle,
                ListType
            });

            //WriteSchema("C:\\Test\\fragments.xsd", FragmentsSchema);
        }

        private static XmlSchema CreateConditionsDataSchema()
        {
            //conditions Element
            XmlSchemaElement elementConditions = CreateSchemaElement("conditions", "conditionsType");

            //conditionsType ComplexType
            XmlSchemaComplexType conditionsType = CreateComplexChoiceType("conditionsType",
                new XmlSchemaElement[]
                {
                    CreateSchemaElement("and", "andType"),
                    CreateSchemaElement("or", "orType"),
                    CreateSchemaElement("function", "functionType"),
                    CreateSchemaElement("not", "notType")
                },
                null, 1, 1);

            //notType ComplexType
            XmlSchemaComplexType notType = CreateComplexType("notType",
                new XmlSchemaElement[]
                {
                    CreateSchemaElement("function", "functionType", 1, 1)
                },
                null);

            //andType ComplexType
            XmlSchemaComplexType andType = CreateComplexChoiceType("andType",
                new XmlSchemaElement[]
                {
                    CreateSchemaElement("not", "notType"),
                    CreateSchemaElement("function", "functionType")
                },
                null, 2, null, true);

            //orType ComplexType
            XmlSchemaComplexType orType = CreateComplexChoiceType("orType",
                new XmlSchemaElement[]
                {
                    CreateSchemaElement("not", "notType"),
                    CreateSchemaElement("function", "functionType")
                },
                null, 2, null, true);

            return CreateCompiledXmlSchemaSet(new XmlSchemaObject[]
            {
                elementConditions,
                conditionsType,
                notType,
                andType,
                orType,
                ConstructorType,
                FunctionType,
                VariableType,
                LiteralListItemType,
                ObjectListItemType,
                LiteralParameterType,
                ObjectParameterType,
                LiteralListParameterType,
                ObjectListParameterType,
                ObjectListType,
                LiteralListType,
                ParametersType,
                GenericArgumentsType,
                LiteralGenericConfigType,
                ObjectGenericConfigType,
                LiteralListGenericConfigType,
                ObjectListGenericConfigType,
                DomainType,
                LiteralType,
                LiteralTypeExt,
                ParameterArgumentType,
                AssemblyQualifiedNameType,
                LiteralInputStyle,
                ListInputStyle,
                ListType
            });

            //WriteSchema("C:\\Test\\conditionsData.xsd", ConditionsDataSchema);
        }

        private static XmlSchema CreateDecisionsDataSchema()
        {
            //decisions Element
            XmlSchemaElement elementDecisions = CreateSchemaElement("decisions", "decisionsType");

            //decisionsType ComplexType
            XmlSchemaComplexType decisionsType = CreateComplexChoiceType("decisionsType",
                new XmlSchemaElement[]
                {
                    CreateSchemaElement("and", "andDecisionType"),
                    CreateSchemaElement("or", "orDecisionType"),
                    CreateSchemaElement("decision", "decisionType"),
                    CreateSchemaElement("not", "notType")
                },
                null, 1, 1);

            //notType ComplexType
            XmlSchemaComplexType notType = CreateComplexType("notType",
                new XmlSchemaElement[]
                {
                    CreateSchemaElement("decision", "decisionType", 1, 1)
                },
                null);

            //andDecisionType ComplexType
            XmlSchemaComplexType andDecisionType = CreateComplexChoiceType("andDecisionType",
                new XmlSchemaElement[]
                {
                    CreateSchemaElement("not", "notType"),
                    CreateSchemaElement("decision", "decisionType")
                },
                null, 2, null, true);

            //orDecisionType ComplexType
            XmlSchemaComplexType orDecisionType = CreateComplexChoiceType("orDecisionType",
                new XmlSchemaElement[]
                {
                    CreateSchemaElement("not", "notType"),
                    CreateSchemaElement("decision", "decisionType")
                },
                null, 2, null, true);

            //andFunctionType ComplexType
            XmlSchemaComplexType andFunctionType = CreateComplexChoiceType("andFunctionType",
                new XmlSchemaElement[]
                {
                    CreateSchemaElement("not", "notFunctionType"),
                    CreateSchemaElement("function", "functionType")
                },
                null, 2, null, true);

            //orFunctionType ComplexType
            XmlSchemaComplexType orFunctionType = CreateComplexChoiceType("orFunctionType",
                new XmlSchemaElement[]
                {
                    CreateSchemaElement("not", "notFunctionType"),
                    CreateSchemaElement("function", "functionType")
                },
                null, 2, null, true);

            //notFunctionType ComplexType
            XmlSchemaComplexType notFunctionType = CreateComplexType("notFunctionType",
                new XmlSchemaElement[]
                {
                    CreateSchemaElement("function", "functionType", 1, 1)
                },
                null);

            //decisionType ComplexType
            XmlSchemaComplexType decisionType = CreateComplexChoiceType("decisionType",
                new XmlSchemaElement[]
                {
                    CreateSchemaElement("and", "andFunctionType"),
                    CreateSchemaElement("or", "orFunctionType"),
                    CreateSchemaElement("function", "functionType"),
                    CreateSchemaElement("not", "notFunctionType")
                },
                new XmlSchemaAttribute[] { AttributeName, AttributeVisibleText }, 1, 1);

            return CreateCompiledXmlSchemaSet(new XmlSchemaObject[]
            {
                elementDecisions,
                decisionsType,
                notType,
                andDecisionType,
                orDecisionType,
                andFunctionType,
                orFunctionType,
                notFunctionType,
                decisionType,
                ConstructorType,
                FunctionType,
                VariableType,
                LiteralListItemType,
                ObjectListItemType,
                LiteralParameterType,
                ObjectParameterType,
                LiteralListParameterType,
                ObjectListParameterType,
                ObjectListType,
                LiteralListType,
                ParametersType,
                GenericArgumentsType,
                LiteralGenericConfigType,
                ObjectGenericConfigType,
                LiteralListGenericConfigType,
                ObjectListGenericConfigType,
                DomainType,
                LiteralType,
                LiteralTypeExt,
                ParameterArgumentType,
                AssemblyQualifiedNameType,
                LiteralInputStyle,
                ListInputStyle,
                ListType
            });

            //WriteSchema("C:\\Test\\decisionsData.xsd", DecisionsDataSchema);
        }

        private static XmlSchema CreateFunctionDataSchema()
        {
            //function Element
            XmlSchemaElement elementFunction = CreateSchemaElement("function", "functionType");

            //assertFunction Element
            XmlSchemaElement elementAssertFunction = CreateSchemaElement("assertFunction", "assertFunctionType");

            //retractFunction Element
            XmlSchemaElement elementRetractFunction = CreateSchemaElement("retractFunction", "retractFunctionType");

            //not Element
            XmlSchemaElement elementNot = CreateSchemaElement("not", "notType");

            //retractFunctionType ComplexType
            XmlSchemaComplexType retractFunctionType = CreateComplexType("retractFunctionType",
                new XmlSchemaElement[]
                {
                    CreateSchemaElement("variable", "variableType", 1, 1)
                },
                new XmlSchemaAttribute[] { AttributeName, AttributeVisibleText });

            //notType ComplexType
            XmlSchemaComplexType notType = CreateComplexType("notType",
                new XmlSchemaElement[]
                {
                    CreateSchemaElement("function", "functionType", 1, 1)
                },
                null);

            return CreateCompiledXmlSchemaSet(new XmlSchemaObject[]
            {
                elementFunction,
                elementAssertFunction,
                elementRetractFunction,
                elementNot,
                ConstructorType,
                FunctionType,
                notType,
                VariableType,
                LiteralListItemType,
                ObjectListItemType,
                LiteralParameterType,
                ObjectParameterType,
                LiteralListParameterType,
                ObjectListParameterType,
                LiteralVariableType,
                ObjectVariableType,
                LiteralListVariableType,
                ObjectListVariableType,
                ObjectListType,
                LiteralListType,
                AssertFunctionType,
                retractFunctionType,
                VariableValueType,
                ParametersType,
                GenericArgumentsType,
                LiteralGenericConfigType,
                ObjectGenericConfigType,
                LiteralListGenericConfigType,
                ObjectListGenericConfigType,
                DomainType,
                LiteralType,
                LiteralTypeExt,
                ParameterArgumentType,
                AssemblyQualifiedNameType,
                LiteralInputStyle,
                ListInputStyle,
                ListType
            });

            //WriteSchema("C:\\Test\\functionsData.xsd", FunctionsDataSchema);
        }

        private static XmlSchema CreateFunctionsDataSchema()
        {
            //functions Element
            XmlSchemaElement elementFunctions = CreateSchemaElement("functions", "functionsType");

            //functionsType ComplexType
            XmlSchemaComplexType functionsType = CreateComplexChoiceType("functionsType",
                new XmlSchemaElement[]
                {
                    CreateSchemaElement("function", "functionType"),
                    CreateSchemaElement("assertFunction", "assertFunctionType"),
                    CreateSchemaElement("retractFunction", "retractFunctionType")
                },
                null, 1, null, true);

            //retractFunctionType ComplexType
            XmlSchemaComplexType retractFunctionType = CreateComplexType("retractFunctionType",
                new XmlSchemaElement[]
                {
                    CreateSchemaElement("variable", "variableType", 1, 1)
                },
                new XmlSchemaAttribute[] { AttributeName, AttributeVisibleText });

            return CreateCompiledXmlSchemaSet(new XmlSchemaObject[]
            {
                elementFunctions,
                functionsType,
                ConstructorType,
                FunctionType,
                VariableType,
                LiteralListItemType,
                ObjectListItemType,
                LiteralParameterType,
                ObjectParameterType,
                LiteralListParameterType,
                ObjectListParameterType,
                LiteralVariableType,
                ObjectVariableType,
                LiteralListVariableType,
                ObjectListVariableType,
                ObjectListType,
                LiteralListType,
                AssertFunctionType,
                retractFunctionType,
                VariableValueType,
                ParametersType,
                GenericArgumentsType,
                LiteralGenericConfigType,
                ObjectGenericConfigType,
                LiteralListGenericConfigType,
                ObjectListGenericConfigType,
                DomainType,
                LiteralType,
                LiteralTypeExt,
                ParameterArgumentType,
                AssemblyQualifiedNameType,
                LiteralInputStyle,
                ListInputStyle,
                ListType
            });

            //WriteSchema("C:\\Test\\functionsData.xsd", FunctionsDataSchema);
        }

        private static XmlSchema CreateTableSchema()
        {
            //tables Element
            XmlSchemaElement elementTables = new()
            {
                Name = "tables",
                SchemaType = CreateComplexChoiceType(null,
                            new XmlSchemaElement[]
                            {
                                CreateSchemaElement("rulesTable", "rulesTableType"),
                                CreateSchemaElement("ruleSetTable", "ruleSetTableType")
                            },
                            null, 0, null, true)
            };

            //rulesTableType ComplextType
            XmlSchemaComplexType rulesTableType = CreateComplexType("rulesTableType",
            new XmlSchemaElement[]
            {
                    CreateSchemaElement("Condition", "string", 1, 1, false, true),
                    CreateSchemaElement("ConditionVisible", "string", 1, 1, false, true),
                    CreateSchemaElement("Action", "string", 1, 1, false, true),
                    CreateSchemaElement("ActionVisible", "string", 1, 1, false, true),
                    CreateSchemaElement("Priority", "string", 1, 1, false, true),
                    CreateSchemaElement("PriorityVisible", "string", 1, 1, false, true),
                    CreateSchemaElement("ReEvaluate", "boolean", 1, 1, false, true),
                    CreateSchemaElement("Active", "boolean", 1, 1, false, true)
            },
            null);

            //ruleSetTableType ComplextType
            XmlSchemaComplexType ruleSetTableType = CreateComplexType("ruleSetTableType",
            new XmlSchemaElement[]
            {
                    CreateSchemaElement("Chaining", "RuleChainingBehavior", 1, 1)
            },
            null);

            //RuleChainingBehavior
            XmlSchemaSimpleType ruleChainingBehavior = CreateEnumRestriction("RuleChainingBehavior", new string[]
            {
                    "Full",
                    "None",
                    "UpdateOnly"
            });

            return CreateCompiledXmlSchemaSet(new XmlSchemaObject[]
            {
                elementTables,
                rulesTableType,
                ruleSetTableType,
                ruleChainingBehavior
            });

            //WriteSchema(@"C:\Test\sourceTable.xsd", TableSchema);
        }

        private static XmlSchemaAttribute CreateRequiredAttribute(string attributeName, string schemaTypeName, bool schemaUriRequired = false)
        {
            XmlSchemaAttribute attribute = new()
            {
                Name = attributeName,
                Use = XmlSchemaUse.Required,

                SchemaTypeName = schemaUriRequired
                ? new XmlQualifiedName(schemaTypeName, SchemaConstants.NAMESPACEURI)
                : new XmlQualifiedName(schemaTypeName)
            };

            return attribute;
        }

        private static XmlSchemaAttribute CreateOptionalAttribute(string attributeName, string schemaTypeName, bool schemaUriRequired = false)
        {
            XmlSchemaAttribute attribute = new()
            {
                Name = attributeName,
                Use = XmlSchemaUse.Optional,

                SchemaTypeName = schemaUriRequired
                ? new XmlQualifiedName(schemaTypeName, SchemaConstants.NAMESPACEURI)
                : new XmlQualifiedName(schemaTypeName)
            };

            return attribute;
        }

        private static XmlSchemaUnique CreateUniqueConstraint(string constraintName, string selectorXPath, string fieldXPath)
        {
            XmlSchemaUnique uniqueConstraint = new()
            {
                Name = constraintName,
                Selector = new XmlSchemaXPath() { XPath = selectorXPath }
            };
            uniqueConstraint.Fields.Add(new XmlSchemaXPath
            {
                XPath = fieldXPath
            });

            return uniqueConstraint;
        }

        private static XmlSchemaElement CreateSchemaElement(string elementName, string schemaTypeName, int minOccurs, int? maxOccurs, bool unbounded = false, bool schenaUriRequired = false, XmlSchemaObject[]? constraints = null)
        {
            XmlSchemaElement element = new()
            {
                Name = elementName
            };
            element.SchemaTypeName = (schenaUriRequired)
                ? new XmlQualifiedName(schemaTypeName, SchemaConstants.NAMESPACEURI)
                : element.SchemaTypeName = new XmlQualifiedName(schemaTypeName);

            element.MinOccurs = minOccurs;

            if (!unbounded)
            {
                if (!maxOccurs.HasValue)
                    throw new CriticalLogicBuilderException(string.Format(CultureInfo.InvariantCulture, Strings.invalidArgumentTextFormat, "{696486FE-74C3-4D36-B04F-79A4ED428690}"));

                element.MaxOccurs = maxOccurs.Value;
            }
            else
                element.MaxOccursString = "unbounded";

            if (constraints != null)
            {
                constraints.ToList()
                    .ForEach(c =>
                    {
                        element.Constraints.Add(c);
                    });
            }
            return element;
        }

        private static XmlSchemaElement CreateSchemaElement(string elementName, string schemaTypeName, bool schemaUriRequired = false, XmlSchemaObject[]? constraints = null)
        {
            XmlSchemaElement element = new()
            {
                Name = elementName,
                SchemaTypeName = schemaUriRequired
                    ? new XmlQualifiedName(schemaTypeName, SchemaConstants.NAMESPACEURI)
                    : new XmlQualifiedName(schemaTypeName)
            };

            if (constraints != null)
            {
                constraints.ToList()
                    .ForEach(c =>
                    {
                        element.Constraints.Add(c);
                    });
            }

            return element;
        }

        private static XmlSchemaComplexType CreateComplexType(string elementName, XmlSchemaObject[] sequenceElements, XmlSchemaAttribute[] attributes)
        {
            XmlSchemaComplexType complexType = new()
            {
                Name = elementName,
                Particle = sequenceElements?.Aggregate(new XmlSchemaSequence(), (seq, next) =>
                {
                    seq.Items.Add(next);
                    return seq;
                })
            };

            attributes?.ToList().ForEach(attribute => complexType.Attributes.Add(attribute));

            return complexType;
        }

        private static XmlSchemaComplexType CreateComplexType(string? elementName, XmlSchemaElement[]? sequenceElements, XmlSchemaAttribute[]? attributes)
        {
            XmlSchemaComplexType complexType = new()
            {
                Name = elementName,
                Particle = sequenceElements?.Aggregate(new XmlSchemaSequence(), (seq, next) =>
                {
                    seq.Items.Add(next);
                    return seq;
                })
            };

            attributes?.ToList().ForEach(attribute => complexType.Attributes.Add(attribute));

            return complexType;
        }

        private static XmlSchemaComplexType ExtendComplexType(string elementName, string baseElementName, XmlSchemaElement[] sequenceElements, XmlSchemaAttribute[]? attributes)
        {
            XmlSchemaComplexContentExtension getExtension(XmlSchemaElement[] seqElents, XmlSchemaAttribute[]? attrs)
            {
                XmlSchemaComplexContentExtension baseExtension = new()
                {
                    BaseTypeName = new XmlQualifiedName(baseElementName),
                    Particle = seqElents?.Aggregate(new XmlSchemaSequence(), (seq, next) =>
                    {
                        seq.Items.Add(next);
                        return seq;
                    })
                };

                attrs?.ToList().ForEach(attribute => baseExtension.Attributes.Add(attribute));
                return baseExtension;
            }

            XmlSchemaComplexType complexType = new()
            {
                Name = elementName,
                ContentModel = new XmlSchemaComplexContent
                {
                    Content = getExtension(sequenceElements, attributes)
                }
            };

            return complexType;
        }

        private static XmlSchemaComplexType CreateComplexChoiceType(string? elementName, XmlSchemaElement[] choiceElements, XmlSchemaAttribute[]? attributes, int minOccurs, int? maxOccurs, bool unbounded = false, bool isMixed = false)
        {
            XmlSchemaComplexType complexType = new()
            {
                IsMixed = isMixed,
                Name = elementName
            };

            if (choiceElements != null)
            {
                XmlSchemaChoice choice = choiceElements.Aggregate(new XmlSchemaChoice(), (seq, next) =>
                {
                    seq.Items.Add(next);
                    return seq;
                });

                choice.MinOccurs = minOccurs;
                if (!unbounded)
                {
                    if (!maxOccurs.HasValue)
                        throw new CriticalLogicBuilderException(string.Format(CultureInfo.InvariantCulture, Strings.invalidArgumentTextFormat, "{C694E883-AEB8-4A07-AF60-480832C5F2E8}"));

                    choice.MaxOccurs = maxOccurs.Value;
                }
                else
                    choice.MaxOccursString = "unbounded";

                complexType.Particle = choice;
            }

            if (attributes != null)
            {
                foreach (XmlSchemaAttribute attribute in attributes)
                    complexType.Attributes.Add(attribute);
            }

            return complexType;
        }

        private static XmlSchemaComplexType CreateComplexChoiceType(string elementName, XmlSchemaObject[] choiceElements, XmlSchemaAttribute[]? attributes, int minOccurs, int? maxOccurs, bool unbounded = false, bool isMixed = false)
        {
            XmlSchemaComplexType complexType = new()
            {
                IsMixed = isMixed,
                Name = elementName
            };

            if (choiceElements != null)
            {
                XmlSchemaChoice choice = choiceElements.Aggregate(new XmlSchemaChoice(), (seq, next) =>
                {
                    seq.Items.Add(next);
                    return seq;
                });

                choice.MinOccurs = minOccurs;
                if (!unbounded)
                {
                    if (!maxOccurs.HasValue)
                        throw new CriticalLogicBuilderException(string.Format(CultureInfo.InvariantCulture, Strings.invalidArgumentTextFormat, "{4491B342-9E2E-4E83-9A0E-FBDFF0535B7C}"));

                    choice.MaxOccurs = maxOccurs.Value;
                }
                else
                    choice.MaxOccursString = "unbounded";

                complexType.Particle = choice;
            }

            if (attributes != null)
            {
                foreach (XmlSchemaAttribute attribute in attributes)
                    complexType.Attributes.Add(attribute);
            }

            return complexType;
        }

        private static XmlSchemaChoice GetChoiceParticle(XmlSchemaElement[] choiceElements, int minOccurs, int? maxOccurs, bool unbounded = false)
        {
            XmlSchemaChoice choice = choiceElements.Aggregate(new XmlSchemaChoice(), (seq, next) =>
            {
                seq.Items.Add(next);
                return seq;
            });

            choice.MinOccurs = minOccurs;
            if (!unbounded)
            {
                if (!maxOccurs.HasValue)
                    throw new CriticalLogicBuilderException(string.Format(CultureInfo.InvariantCulture, Strings.invalidArgumentTextFormat, "{FEB8E682-95C3-4A79-9DFB-ACB39A0CF435}"));

                choice.MaxOccurs = maxOccurs.Value;
            }
            else
                choice.MaxOccursString = "unbounded";

            return choice;
        }

        private static XmlSchemaSimpleType CreateEnumRestriction(string simpleTypeName, string[] enums)
        {
            XmlSchemaSimpleTypeRestriction restriction = new()
            {
                BaseTypeName = new XmlQualifiedName("string", SchemaConstants.NAMESPACEURI)
            };

            enums.ToList().ForEach(s => AddEnumeration(restriction.Facets, s));

            return new XmlSchemaSimpleType
            {
                Name = simpleTypeName,
                Content = restriction
            };
        }

        private static XmlSchemaSimpleType CreateSimpleTypeUnion(string name, params string[] memberTypes) => new()
        {
            Name = name,
            Content = new XmlSchemaSimpleTypeUnion
            {
                MemberTypes = memberTypes.Select(mt => new XmlQualifiedName(mt)).ToArray()
            }
        };

        private static XmlSchemaSimpleType CreatePatternRestriction(string simpleTypeName, string patternString)
        {
            XmlSchemaSimpleTypeRestriction restrictionPattern = new()
            {
                BaseTypeName = new XmlQualifiedName("string", SchemaConstants.NAMESPACEURI)
            };

            restrictionPattern.Facets.Add(new XmlSchemaPatternFacet
            {
                Value = patternString
            });

            return new XmlSchemaSimpleType
            {
                Name = simpleTypeName,
                Content = restrictionPattern
            };
        }

        #region Config Reusables
        //literalParameterType ComplexType
        private static XmlSchemaComplexType LiteralParameterType_Config => CreateComplexType("literalParameterType",
            new XmlSchemaElement[]
            {
                    CreateSchemaElement("literalType", "ParameterArgumentType", 1, 1),
                    CreateSchemaElement("control", "LiteralInputStyle", 1, 1),
                    CreateSchemaElement("optional", "boolean", 1, 1, false, true),
                    CreateSchemaElement("useForEquality", "boolean", 1, 1, false, true),
                    CreateSchemaElement("useForHashCode", "boolean", 1, 1, false, true),
                    CreateSchemaElement("useForToString", "boolean", 1, 1, false, true),
                    CreateSchemaElement("propertySource", "string", 1, 1, false, true),
                    CreateSchemaElement("propertySourceParameter", "string", 1, 1, false, true),
                    CreateSchemaElement("defaultValue", "string", 1, 1, false, true),
                    CreateSchemaElement("domain", "domainType", 1, 1, false, false, new XmlSchemaObject[] { CreateUniqueConstraint("itemValue", "./item", ".") }),
                    CreateSchemaElement("comments", "string", 1, 1, false, true)
            },
            new XmlSchemaAttribute[] { AttributeName });

        //objectParameterType ComplexType
        private static XmlSchemaComplexType ObjectParameterType_Config => CreateComplexType("objectParameterType",
            new XmlSchemaElement[]
            {
                    CreateSchemaElement("objectType", "AsemblyQualifiedNameType", 1, 1),
                    CreateSchemaElement("optional", "boolean", 1, 1, false, true),
                    CreateSchemaElement("useForEquality", "boolean", 1, 1, false, true),
                    CreateSchemaElement("useForHashCode", "boolean", 1, 1, false, true),
                    CreateSchemaElement("useForToString", "boolean", 1, 1, false, true),
                    CreateSchemaElement("comments", "string", 1, 1, false, true)
            },
            new XmlSchemaAttribute[] { AttributeName });

        //literalListParameterType ComplexType
        private static XmlSchemaComplexType LiteralListParameterType_Config => CreateComplexType("literalListParameterType",
            new XmlSchemaElement[]
            {
                    CreateSchemaElement("literalType", "ParameterArgumentType", 1, 1),
                    CreateSchemaElement("listType", "ListType", 1, 1),
                    CreateSchemaElement("control", "ListInputStyle", 1, 1),
                    CreateSchemaElement("elementControl", "LiteralInputStyle", 1, 1),
                    CreateSchemaElement("optional", "boolean", 1, 1, false, true),
                    CreateSchemaElement("propertySource", "string", 1, 1, false, true),
                    CreateSchemaElement("propertySourceParameter", "string", 1, 1, false, true),
                    CreateSchemaElement("defaultValue", "domainType", 1, 1, false, false,
                                            new XmlSchemaObject[]
                                            {
                                               CreateUniqueConstraint("itemValue1", "./item", ".")
                                            }),
                    CreateSchemaElement("domain", "domainType", 1, 1, false, false,
                                            new XmlSchemaObject[]
                                            {
                                               CreateUniqueConstraint("itemValue2", "./item", ".")
                                            }),
                    CreateSchemaElement("comments", "string", 1, 1, false, true)
            },
            new XmlSchemaAttribute[] { AttributeName });

        //objectListParameterType ComplexType
        private static XmlSchemaComplexType ObjectListParameterType_Config => CreateComplexType("objectListParameterType",
            new XmlSchemaElement[]
            {
                    CreateSchemaElement("objectType", "AsemblyQualifiedNameType", 1, 1),
                    CreateSchemaElement("listType", "ListType", 1, 1),
                    CreateSchemaElement("control", "ListInputStyle", 1, 1),
                    CreateSchemaElement("optional", "boolean", 1, 1, false, true),
                    CreateSchemaElement("comments", "string", 1, 1, false, true)
            },
            new XmlSchemaAttribute[] { AttributeName });

        //genericParameterType ComplexType
        private static XmlSchemaComplexType GenericParameterType_Config => CreateComplexType("genericParameterType",
            new XmlSchemaElement[]
            {
                    CreateSchemaElement("genericArgumentName", "string", 1, 1, false, true),
                    CreateSchemaElement("optional", "boolean", 1, 1, false, true),
                    CreateSchemaElement("comments", "string", 1, 1, false, true)
            },
            new XmlSchemaAttribute[] { AttributeName });

        //genericParameterType ComplexType
        private static XmlSchemaComplexType GenericListParameterType_Config => CreateComplexType("genericListParameterType",
            new XmlSchemaElement[]
            {
                    CreateSchemaElement("genericArgumentName", "string", 1, 1, false, true),
                    CreateSchemaElement("listType", "ListType", 1, 1),
                    CreateSchemaElement("control", "ListInputStyle", 1, 1),
                    CreateSchemaElement("optional", "boolean", 1, 1, false, true),
                    CreateSchemaElement("comments", "string", 1, 1, false, true)
            },
            new XmlSchemaAttribute[] { AttributeName });

        //genericArgumentsType ComplexType
        private static XmlSchemaComplexType GenericArgumentsType_Config => CreateComplexType("genericArgumentsType",
                new XmlSchemaElement[]
                {
                    CreateSchemaElement("item", "string", 0, null, true, true)
                },
                null);
        #endregion Config Reusables

        private static XmlSchema CreateFunctionsSchema()
        {
            //forms Element
            XmlSchemaElement elementForms = CreateSchemaElement("forms", "formsType", false, new XmlSchemaObject[] { CreateUniqueConstraint("functionNameKey", ".//function", "@name") });

            //functionType ComplexType
            XmlSchemaComplexType functionType = CreateComplexType("functionType",
                new XmlSchemaElement[]
                {
                    CreateSchemaElement("memberName", "string", 1, 1, false, true),
                    CreateSchemaElement("functionCategory", "FunctionCategories", 1, 1),
                    CreateSchemaElement("typeName", "string", 1, 1, false, true),
                    CreateSchemaElement("referenceName", "string", 1, 1, false, true),
                    CreateSchemaElement("referenceDefinition", "string", 1, 1, false, true),
                    CreateSchemaElement("castReferenceAs", "string", 1, 1, false, true),
                    CreateSchemaElement("referenceCategory", "ReferenceCategory", 1, 1),
                    CreateSchemaElement("parametersLayout", "ParametersLayout", 1, 1),
                    CreateSchemaElement(
                                            "parameters",
                                            "parametersType",
                                            1, 1,
                                            false,
                                            false,
                                            new XmlSchemaObject[]
                                            {
                                                CreateUniqueConstraint("parameterNameKey", "./literalParameter|./objectParameter|./literalListParameter|./objectListParameter|./genericParameter|./genericListParameter", "@name")
                                            }
                                       ),
                    CreateSchemaElement("genericArguments", "genericArgumentsType", 1, 1, false, false,
                                            new XmlSchemaObject[]
                                            {
                                               CreateUniqueConstraint("itemArg", "./item", ".")
                                            }),
                    CreateSchemaElement("returnType", "functionReturnType", 1, 1),
                    CreateSchemaElement("summary", "string", 1, 1, false, true)
                },
                new XmlSchemaAttribute[] { AttributeName });

            //folderType ComplexType
            XmlSchemaComplexType folderType = CreateComplexType("folderType",
                new XmlSchemaElement[]
                {
                    CreateSchemaElement("function", "functionType", 0, null, true),
                    CreateSchemaElement("folder", "folderType", 0, null, true, false,
                                            new XmlSchemaObject[]
                                            {
                                                CreateUniqueConstraint("subFolderNameKey", "./folder", "@name")
                                            }
                                       )
                },
                new XmlSchemaAttribute[] { AttributeName });

            //formType ComplexType
            XmlSchemaComplexType formType = CreateComplexType("formType",
                new XmlSchemaElement[]
                {
                    CreateSchemaElement("folder", "folderType", 1, 1, false, false,
                                            new XmlSchemaObject[]
                                            {
                                                CreateUniqueConstraint("folderNameKey", "./folder", "@name")
                                            }
                                       )
                },
                new XmlSchemaAttribute[] { AttributeName });

            //formsType ComplexType
            XmlSchemaComplexType formsType = CreateComplexType("formsType",
                new XmlSchemaElement[]
                {
                    CreateSchemaElement("form", "formType", 1, null, true, false)
                },
                null);

            //parametersType ComplexType
            XmlSchemaComplexType parametersType = CreateComplexChoiceType("parametersType",
                new XmlSchemaElement[]
                {
                    CreateSchemaElement("literalParameter", "literalParameterType"),
                    CreateSchemaElement("objectParameter", "objectParameterType"),
                    CreateSchemaElement("literalListParameter", "literalListParameterType"),
                    CreateSchemaElement("objectListParameter", "objectListParameterType"),
                    CreateSchemaElement("genericParameter", "genericParameterType"),
                    CreateSchemaElement("genericListParameter", "genericListParameterType")
                },
                null, 0, null, true);

            //functionReturnType ComplexType
            XmlSchemaComplexType functionReturnType = CreateComplexChoiceType("functionReturnType",
                new XmlSchemaElement[]
                {
                    CreateSchemaElement("literal", "literalReturnType"),
                    CreateSchemaElement("object", "objectReturnType"),
                    CreateSchemaElement("literalList", "literalListReturnType"),
                    CreateSchemaElement("objectList", "objectListReturnType"),
                    CreateSchemaElement("generic", "genericReturnType"),
                    CreateSchemaElement("genericList", "genericListReturnType")
                },
                null, 1, 1);

            //literalReturnType ComplexType
            XmlSchemaComplexType literalReturnType = CreateComplexType("literalReturnType",
                new XmlSchemaElement[]
                {
                    CreateSchemaElement("literalType", "LiteralFunctionReturnType", 1, 1)
                },
                null);

            //objectReturnType ComplexType
            XmlSchemaComplexType constructorReturnType = CreateComplexType("objectReturnType",
                new XmlSchemaElement[]
                {
                    CreateSchemaElement("objectType", "AsemblyQualifiedNameType", 1, 1)
                },
                null);

            //literalListReturnType ComplexType
            XmlSchemaComplexType literalListReturnType = CreateComplexType("literalListReturnType",
                new XmlSchemaElement[]
                {
                    CreateSchemaElement("literalType", "LiteralFunctionReturnType", 1, 1),
                    CreateSchemaElement("listType", "ListType", 1, 1)
                },
                null);

            //objectListReturnType ComplexType
            XmlSchemaComplexType constructorListReturnType = CreateComplexType("objectListReturnType",
                new XmlSchemaElement[]
                {
                    CreateSchemaElement("objectType", "AsemblyQualifiedNameType", 1, 1),
                    CreateSchemaElement("listType", "ListType", 1, 1)
                },
                null);

            //genericReturnType ComplexType
            XmlSchemaComplexType genericReturnType = CreateComplexType("genericReturnType",
                new XmlSchemaElement[]
                {
                    CreateSchemaElement("genericArgumentName", "string", 1, 1, false, true)
                },
                null);

            //genericListReturnType ComplexType
            XmlSchemaComplexType genericListReturnType = CreateComplexType("genericListReturnType",
                new XmlSchemaElement[]
                {
                    CreateSchemaElement("genericArgumentName", "string", 1, 1, false, true),
                    CreateSchemaElement("listType", "ListType", 1, 1)
                },
                null);

            //domainType ComplexType
            XmlSchemaComplexType domainType = CreateComplexType("domainType",
                new XmlSchemaElement[]
                {
                    CreateSchemaElement("item", "string", 0, null, true, true)
                },
                null);

            //FunctionCategories
            XmlSchemaSimpleType functionCategories = CreateEnumRestriction("FunctionCategories", new string[]
            {
                "Standard",
                "DialogForm",
                "MultipleChoice",
                "Assert",
                "Retract",
                "BinaryOperator",
                "RuleChainingUpdate",
                "Cast"
            });

            //ReferenceCategory
            XmlSchemaSimpleType referenceCategory = CreateEnumRestriction("ReferenceCategory", new string[]
            {
                "InstanceReference",
                "StaticReference",
                "Type",
                "This",
                "None"
            });

            //ParametersLayout
            XmlSchemaSimpleType parametersLayout = CreateEnumRestriction("ParametersLayout", new string[]
            {
                "Sequential",
                "Binary"
            });

            //LiteralFunctionReturnType
            XmlSchemaSimpleType literalFunctionReturnType = CreateEnumRestriction("LiteralFunctionReturnType", new string[]
            {
                "Void",
                "String",
                "Decimal",
                "Integer",
                "Boolean",
                "DateTime",
                "DateTimeOffset",
                "DateOnly",
                "Date",
                "TimeSpan",
                "TimeOnly",
                "TimeOfDay",
                "Guid",
                "Byte",
                "Short",
                "Long",
                "Float",
                "Double",
                "Char",
                "SByte",
                "UShort",
                "UInteger",
                "ULong",
                "NullableDecimal",
                "NullableInteger",
                "NullableBoolean",
                "NullableDateTime",
                "NullableDateTimeOffset",
                "NullableDateOnly",
                "NullableDate",
                "NullableTimeSpan",
                "NullableTimeOnly",
                "NullableTimeOfDay",
                "NullableGuid",
                "NullableByte",
                "NullableShort",
                "NullableLong",
                "NullableFloat",
                "NullableDouble",
                "NullableChar",
                "NullableSByte",
                "NullableUShort",
                "NullableUInteger",
                "NullableULong"
            });


            //ParameterArgumentType
            XmlSchemaSimpleType parameterArgumentType = CreateEnumRestriction("ParameterArgumentType", new string[]
            {
                "Decimal",
                "String",
                "Integer",
                "Boolean",
                "DateTime",
                "DateTimeOffset",
                "DateOnly",
                "Date",
                "TimeSpan",
                "TimeOnly",
                "TimeOfDay",
                "Guid",
                "Byte",
                "Short",
                "Long",
                "Float",
                "Double",
                "Char",
                "SByte",
                "UShort",
                "UInteger",
                "ULong",
                "NullableDecimal",
                "NullableInteger",
                "NullableBoolean",
                "NullableDateTime",
                "NullableDateTimeOffset",
                "NullableDateOnly",
                "NullableDate",
                "NullableTimeSpan",
                "NullableTimeOnly",
                "NullableTimeOfDay",
                "NullableGuid",
                "NullableByte",
                "NullableShort",
                "NullableLong",
                "NullableFloat",
                "NullableDouble",
                "NullableChar",
                "NullableSByte",
                "NullableUShort",
                "NullableUInteger",
                "NullableULong",
                "Any"
            });

            //AsemblyQualifiedNameType
            XmlSchemaSimpleType asemblyQualifiedNameType = CreatePatternRestriction("AsemblyQualifiedNameType", RegularExpressions.FULLYQUALIFIEDCLASSNAME);

            //LiteralInputStyle
            XmlSchemaSimpleType literalInputStyle = CreateEnumRestriction("LiteralInputStyle", new string[]
            {
                "SingleLineTextBox",
                "MultipleLineTextBox",
                "DropDown",
                "TypeAutoComplete",
                "DomainAutoComplete",
                "PropertyInput",
                "ParameterSourcedPropertyInput",
                "ParameterSourceOnly"
            });

            //ListInputStyle
            XmlSchemaSimpleType listInputStyle = CreateEnumRestriction("ListInputStyle", new string[]
            {
                "ListForm",
                "HashSetForm"
            });

            //ListType
            XmlSchemaSimpleType listType = CreateEnumRestriction("ListType", new string[]
            {
                "Array",
                "GenericList",
                "GenericCollection",
                "IGenericList",
                "IGenericCollection",
                "IGenericEnumerable"
            });

            return CreateCompiledXmlSchemaSet(new XmlSchemaObject[]
            {
                elementForms,
                functionType,
                folderType,
                formType,
                formsType,
                parametersType,
                LiteralParameterType_Config,
                ObjectParameterType_Config,
                LiteralListParameterType_Config,
                ObjectListParameterType_Config,
                GenericParameterType_Config,
                GenericListParameterType_Config,
                functionReturnType,
                literalReturnType,
                constructorReturnType,
                literalListReturnType,
                constructorListReturnType,
                genericReturnType,
                genericListReturnType,
                domainType,
                GenericArgumentsType_Config,
                functionCategories,
                referenceCategory,
                parametersLayout,
                literalFunctionReturnType,
                parameterArgumentType,
                asemblyQualifiedNameType,
                literalInputStyle,
                listInputStyle,
                listType
            });

            //WriteSchema("C:\\Test\\functions.xsd", FunctionsSchema);
        }

        private static XmlSchema CreateConstructorSchema()
        {
            //form Element
            XmlSchemaElement elementForm = CreateSchemaElement("form", "formType", false,
                new XmlSchemaObject[]
                {
                    CreateUniqueConstraint("constructorNameKey", ".//constructor", "@name"),
                    CreateUniqueConstraint("formFolderNameKey", "./folder", "@name")
                });

            //formType ComplexType
            XmlSchemaComplexType formType = CreateComplexType("formType",
                new XmlSchemaElement[]
                {
                    CreateSchemaElement("constructor", "constructorType", 0, null, true),
                    CreateSchemaElement("folder", "folderType", 0, null, true, false, new XmlSchemaObject[] { CreateUniqueConstraint("folderNameKey", "./folder", "@name") })
                },
                null);
            //formType ComplexType

            //folderType ComplexType
            XmlSchemaComplexType folderType = CreateComplexType("folderType",
                new XmlSchemaElement[]
                {
                    CreateSchemaElement("constructor", "constructorType", 0, null, true),
                    CreateSchemaElement("folder", "folderType", 0, null, true, false,
                                                new XmlSchemaObject[]
                                                {
                                                    CreateUniqueConstraint("subFolderNameKey", "./folder", "@name")
                                                })
                },
                new XmlSchemaAttribute[] { AttributeName });

            //constructorType ComplexType
            XmlSchemaComplexType constructorType = CreateComplexType("constructorType",
                new XmlSchemaElement[]
                {
                    CreateSchemaElement("typeName", "string", 1, 1, false, true),
                    CreateSchemaElement("parameters", "parametersType", 1, 1, false, false,
                                                new XmlSchemaObject[]
                                                {
                                                    CreateUniqueConstraint("parameterNameKey", "./literalParameter|./objectParameter|./literalListParameter|./objectListParameter|./genericParameter|./genericListParameter", "@name")
                                                }),
                    CreateSchemaElement("genericArguments", "genericArgumentsType", 1, 1, false, false,
                                            new XmlSchemaObject[]
                                            {
                                               CreateUniqueConstraint("itemArg", "./item", ".")
                                            }),
                    CreateSchemaElement("summary", "string", 1, 1, false, true) },
                new XmlSchemaAttribute[] { CreateRequiredAttribute("name", "ConstructorNameType") });

            //parametersType ComplexType
            XmlSchemaComplexType parametersType = CreateComplexChoiceType("parametersType",
                new XmlSchemaElement[]
                {
                    CreateSchemaElement("literalParameter", "literalParameterType"),
                    CreateSchemaElement("objectParameter", "objectParameterType"),
                    CreateSchemaElement("literalListParameter", "literalListParameterType"),
                    CreateSchemaElement("objectListParameter", "objectListParameterType"),
                    CreateSchemaElement("genericParameter", "genericParameterType"),
                    CreateSchemaElement("genericListParameter", "genericListParameterType")
                },
                null, 0, null, true);

            //domainType ComplexType
            XmlSchemaComplexType domainType = CreateComplexType("domainType",
                new XmlSchemaElement[]
                {
                    CreateSchemaElement("item", "string", 0, null, true, true)
                },
                null);

            //ParametersLayout
            XmlSchemaSimpleType parametersLayout = CreateEnumRestriction("ParametersLayout", new string[]
            {
                "Sequential",
                "Binary"
            });

            //ParameterArgumentType
            XmlSchemaSimpleType parameterArgumentType = CreateEnumRestriction("ParameterArgumentType", new string[]
            {
                "Decimal",
                "String",
                "Integer",
                "Boolean",
                "DateTime",
                "DateTimeOffset",
                "DateOnly",
                "Date",
                "TimeSpan",
                "TimeOnly",
                "TimeOfDay",
                "Guid",
                "Byte",
                "Short",
                "Long",
                "Float",
                "Double",
                "Char",
                "SByte",
                "UShort",
                "UInteger",
                "ULong",
                "NullableDecimal",
                "NullableInteger",
                "NullableBoolean",
                "NullableDateTime",
                "NullableDateTimeOffset",
                "NullableDateOnly",
                "NullableDate",
                "NullableTimeSpan",
                "NullableTimeOnly",
                "NullableTimeOfDay",
                "NullableGuid",
                "NullableByte",
                "NullableShort",
                "NullableLong",
                "NullableFloat",
                "NullableDouble",
                "NullableChar",
                "NullableSByte",
                "NullableUShort",
                "NullableUInteger",
                "NullableULong",
                "Any"
            });

            //ConstructorNameType
            XmlSchemaSimpleType constructorNameType = CreatePatternRestriction("ConstructorNameType", RegularExpressions.XMLATTRIBUTE);

            //AsemblyQualifiedNameType
            XmlSchemaSimpleType asemblyQualifiedNameType = CreatePatternRestriction("AsemblyQualifiedNameType", RegularExpressions.FULLYQUALIFIEDCLASSNAME);

            //LiteralInputStyle
            XmlSchemaSimpleType literalInputStyle = CreateEnumRestriction("LiteralInputStyle", new string[]
            {
                "SingleLineTextBox",
                "MultipleLineTextBox",
                "DropDown",
                "TypeAutoComplete",
                "DomainAutoComplete",
                "PropertyInput",
                "ParameterSourcedPropertyInput",
                "ParameterSourceOnly"
            });

            //ListInputStyle
            XmlSchemaSimpleType listInputStyle = CreateEnumRestriction("ListInputStyle", new string[]
            {
                "ListForm",
                "HashSetForm"
            });

            //ListType
            XmlSchemaSimpleType listType = CreateEnumRestriction("ListType", new string[]
            {
                "Array",
                "GenericList",
                "GenericCollection",
                "IGenericList",
                "IGenericCollection",
                "IGenericEnumerable"
            });

            return CreateCompiledXmlSchemaSet(new XmlSchemaObject[]
            {
                elementForm,
                formType,
                folderType,
                constructorType,
                parametersType,
                LiteralParameterType_Config,
                ObjectParameterType_Config,
                LiteralListParameterType_Config,
                ObjectListParameterType_Config,
                GenericParameterType_Config,
                GenericListParameterType_Config,
                domainType,
                GenericArgumentsType_Config,
                parametersLayout,
                parameterArgumentType,
                constructorNameType,
                asemblyQualifiedNameType,
                literalInputStyle,
                listInputStyle,
                listType
            });

            //WriteSchema("C:\\Test\\constructors.xsd", ConstructorSchema);
        }

        private static XmlSchema CreateCompiledXmlSchemaSet(XmlSchemaObject[] schemaObjects)
        {
            XmlSchemaSet schemaSet = new();
            schemaSet.ValidationEventHandler += new ValidationEventHandler(ValidateSchema);
            schemaSet.Add(schemaObjects.Aggregate(new XmlSchema(), (sch, next) =>
            {
                sch.Items.Add(next);
                return sch;
            }));

            string schemaValidationErrors = GetXmlSchemaValidationErrors();

            if (schemaValidationErrors.Length != 0)
            {
                throw new CriticalLogicBuilderException(schemaValidationErrors);
            }

            schemaSet.Compile();

            return schemaSet.Schemas().OfType<XmlSchema>().Single();
        }

        private static XmlSchema CreateProjectPropertiesSchema()
        {
            //ProjectProperties Element
            XmlSchemaElement elementProjectProperties = new()
            {
                Name = "ProjectProperties",
                SchemaType = CreateComplexType(null,
                            new XmlSchemaElement[]
                            {
                                CreateSchemaElement("useSharePoint", "boolean", 1, 1, false, true),
                                CreateSchemaElement("web", "string", 1, 1, false, true),
                                CreateSchemaElement("documentLibrary", "string", 1, 1, false, true),
                                CreateSchemaElement("useDefaultCredentials", "boolean", 1, 1, false, true),
                                CreateSchemaElement("userName", "string", 1, 1, false, true),
                                CreateSchemaElement("domain", "string", 1, 1, false, true),
                                CreateSchemaElement("applications", "applicationsType", 1, 1, false, false, new XmlSchemaObject[]
                                                    {
                                                        CreateUniqueConstraint("applicationNameKey", "./application", "@name"),
                                                        CreateUniqueConstraint("applicationNicknameKey", "./application", "@nickname")
                                                    }),
                                CreateSchemaElement("questionsHierarchyObjectTypes", "questionsHierarchyObjectTypesType", 1, 1, false, false, new XmlSchemaObject[]
                                                    {
                                                        CreateUniqueConstraint("groupName", "./objectTypesGroup", "@name")
                                                    }),
                                CreateSchemaElement("inputQuestionsHierarchyObjectTypes", "inputQuestionsHierarchyObjectTypesType", 1, 1, false, false, new XmlSchemaObject[]
                                                    {
                                                        CreateUniqueConstraint("groupName1", "./objectTypesGroup", "@name")
                                                    }),
                                CreateSchemaElement("connectorObjectTypes", "connectorObjectTypesType", 1, 1, false, false, new XmlSchemaObject[]
                                                    {
                                                        CreateUniqueConstraint("objectTypeName2", "./objectType", ".")
                                                    })
                            },
                            null)
            };

            //applicationsType ComplexType
            XmlSchemaComplexType applicationsType = CreateComplexType("applicationsType",
                new XmlSchemaElement[]
                {
                    CreateSchemaElement("application", "applicationType", 1, null, true)
                },
                null);

            //applicationType ComplexType
            XmlSchemaComplexType applicationType = CreateComplexType("applicationType",
                new XmlSchemaElement[]
                {
                    CreateSchemaElement("activityAssembly", "string", 1, 1, false, true),
                    CreateSchemaElement("activityAssemblyPath", "string", 1, 1, false, true),
                    CreateSchemaElement("runtime", "RuntimeType", 1, 1),
                    CreateSchemaElement("loadAssemblyPaths", "loadAssemblyPathsType", 1, 1),
                    CreateSchemaElement("activityClass", "string", 1, 1, false, true),
                    CreateSchemaElement("applicationExecutable", "string", 1, 1, false, true),
                    CreateSchemaElement("applicationExecutablePath", "string", 1, 1, false, true),
                    CreateSchemaElement("startupArguments", "startupArgumentsType", 1, 1),
                    CreateSchemaElement("resourceFile", "string", 1, 1, false, true),
                    CreateSchemaElement("resourceFileDeploymentPath", "string", 1, 1, false, true),
                    CreateSchemaElement("rulesFile", "string", 1, 1, false, true),
                    CreateSchemaElement("rulesDeploymentPath", "string", 1, 1, false, true),
                    CreateSchemaElement("excludedModules", "modulesType", 1, 1),
                    CreateSchemaElement("remoteDeployment", "remoteDeploymentType", 1, 1),
                    CreateSchemaElement("webApiDeployment", "webApiDeploymentType", 1, 1)
                },
                new XmlSchemaAttribute[]
                {
                    CreateRequiredAttribute("name", "applicationNameType"),
                    CreateRequiredAttribute("nickname", "string", true)
                });

            //applicationNameType
            XmlSchemaSimpleType applicationNameType = CreateEnumRestriction("applicationNameType", new string[]
            {
                "App01",
                "App02",
                "App03",
                "App04",
                "App05",
                "App06",
                "App07",
                "App08",
                "App09",
                "App10"
            });

            //startupArgumentsType ComplexType
            XmlSchemaComplexType startupArgumentsType = CreateComplexType("startupArgumentsType",
                new XmlSchemaElement[]
                {
                    CreateSchemaElement("argument", "string", 0, null, true, true)
                },
                null);

            //loadAssemblyPathsType ComplexType
            XmlSchemaComplexType loadAssemblyPathsType = CreateComplexType("loadAssemblyPathsType",
                new XmlSchemaElement[]
                {
                    CreateSchemaElement("path", "string", 0, null, true, true)
                },
                null);

            //modulesType ComplexType
            XmlSchemaComplexType modulesType = CreateComplexType("modulesType",
                new XmlSchemaElement[]
                {
                    CreateSchemaElement("module", "string", 0, null, true, true)
                },
                null);

            //headerType ComplexType
            XmlSchemaComplexType headerType = CreateComplexType("headerType",
                null,
                new XmlSchemaAttribute[]
                {
                    CreateRequiredAttribute("name", "string", true),
                    CreateRequiredAttribute("namespace", "string", true),
                    CreateRequiredAttribute("value", "string", true)
                });

            //addressHeadersType ComplexType
            XmlSchemaComplexType addressHeadersType = CreateComplexType("addressHeadersType",
                new XmlSchemaElement[]
                {
                    CreateSchemaElement("header", "headerType", 0, null, true)
                },
                null);

            //remoteDeploymentType ComplexType
            XmlSchemaComplexType remoteDeploymentType = CreateComplexType("remoteDeploymentType",
                new XmlSchemaElement[]
                {
                    CreateSchemaElement("userName", "string", 1, 1, false, true),
                    CreateSchemaElement("password", "string", 1, 1, false, true),
                    CreateSchemaElement("addressHeaders", "addressHeadersType", 1, 1)
                },
                new XmlSchemaAttribute[]
                {
                    CreateRequiredAttribute("endPointAddress", "string", true),
                    CreateRequiredAttribute("securityMode", "SecurityModeInputStyle"),
                    CreateRequiredAttribute("dnsValue", "string", true)
                });

            //webApiDeploymentType ComplexType
            XmlSchemaComplexType webApiDeploymentType = CreateComplexType("webApiDeploymentType",
                new XmlSchemaElement[]
                {
                    CreateSchemaElement("postFileDataUrl", "UrlType", 1, 1),
                    CreateSchemaElement("postVariablesMetaDataUrl", "UrlType", 1, 1),
                    CreateSchemaElement("deleteRulesUrl", "UrlType", 1, 1),
                    CreateSchemaElement("deleteAllRulesUrl", "UrlType", 1, 1)
                },
                null);


            //questionsHierarchyObjectTypesGroupType ComplexType
            XmlSchemaComplexType questionsHierarchyObjectTypesGroupType = CreateComplexType("questionsHierarchyObjectTypesGroupType",
                new XmlSchemaElement[]
                {
                    CreateSchemaElement("objectType", "AsemblyQualifiedNameType", 0, null, true)
                },
                new XmlSchemaAttribute[]
                {
                    CreateRequiredAttribute("name", "QuestionsHierarchy")
                });

            //inputQuestionsHierarchyObjectTypesGroupType ComplexType
            XmlSchemaComplexType inputQuestionsHierarchyObjectTypesGroupType = CreateComplexType("inputQuestionsHierarchyObjectTypesGroupType",
                new XmlSchemaElement[]
                {
                    CreateSchemaElement("objectType", "AsemblyQualifiedNameType", 0, null, true)
                },
                new XmlSchemaAttribute[]
                {
                    CreateRequiredAttribute("name", "InputQuestionsHierarchy")
                });

            //inputQuestionsHierarchyObjectTypesType ComplexType
            XmlSchemaComplexType inputQuestionsHierarchyObjectTypesType = CreateComplexType("inputQuestionsHierarchyObjectTypesType",
                new XmlSchemaElement[]
                {
                    CreateSchemaElement("objectTypesGroup", "inputQuestionsHierarchyObjectTypesGroupType", 4, 4, false, false, new XmlSchemaObject[] { CreateUniqueConstraint("objectTypeName3", "./objectType", ".") })
                },
                null);

            //questionsHierarchyObjectTypesType ComplexType
            XmlSchemaComplexType questionsHierarchyObjectTypesType = CreateComplexType("questionsHierarchyObjectTypesType",
                new XmlSchemaElement[]
                {
                    CreateSchemaElement("objectTypesGroup", "questionsHierarchyObjectTypesGroupType", 5, 5, false, false, new XmlSchemaObject[] { CreateUniqueConstraint("objectTypeName4", "./objectType", ".") })
                },
                null);

            //connectorObjectTypesType ComplexType
            XmlSchemaComplexType connectorObjectTypesType = CreateComplexType("connectorObjectTypesType",
                new XmlSchemaElement[]
                {
                    CreateSchemaElement("objectType", "AsemblyQualifiedNameType", 0, null, true)
                },
                null);

            //AsemblyQualifiedNameType
            XmlSchemaSimpleType assemblyQualifiedNameType = CreatePatternRestriction("AsemblyQualifiedNameType", RegularExpressions.FULLYQUALIFIEDCLASSNAME);

            //UrlType
            XmlSchemaSimpleType urlType = CreatePatternRestriction("UrlType", RegularExpressions.URL);

            //SecurityModeInputStyle
            XmlSchemaSimpleType securityModeInputStyle = CreateEnumRestriction("SecurityModeInputStyle", new string[]
            {
                "Message",
                "None"
            });

            //InputQuestionsHierarchy
            XmlSchemaSimpleType inputQuestionsHierarchy = CreateEnumRestriction("InputQuestionsHierarchy", new string[]
            {
                "Form",
                "Row",
                "Column",
                "Question"
            });

            //QuestionsHierarchy
            XmlSchemaSimpleType questionsHierarchy = CreateEnumRestriction("QuestionsHierarchy", new string[]
            {
                "Form",
                "Row",
                "Column",
                "Question",
                "Answer"
            });

            //RuntimeType
            XmlSchemaSimpleType runtimeType = CreateEnumRestriction("RuntimeType", new string[]
            {
                "NetFramework",
                "NetCore",
                "Xamarin",
                "NetNative"
            });

            return CreateCompiledXmlSchemaSet(new XmlSchemaObject[]
            {
                elementProjectProperties,
                applicationsType,
                applicationType,
                applicationNameType,
                startupArgumentsType,
                loadAssemblyPathsType,
                modulesType,
                headerType,
                addressHeadersType,
                remoteDeploymentType,
                webApiDeploymentType,
                questionsHierarchyObjectTypesGroupType,
                inputQuestionsHierarchyObjectTypesGroupType,
                inputQuestionsHierarchyObjectTypesType,
                questionsHierarchyObjectTypesType,
                connectorObjectTypesType,
                assemblyQualifiedNameType,
                urlType,
                securityModeInputStyle,
                inputQuestionsHierarchy,
                questionsHierarchy,
                runtimeType
            });

            //WriteSchema("C:\\Test\\ProjectProperties.xsd", ProjectPropertiesSchema);
        }

        private static XmlSchema CreateVariablesSchema()
        {
            //questions Element
            XmlSchemaElement elementFolder = CreateSchemaElement("folder", "folderType", false, new XmlSchemaObject[]
            {
                CreateUniqueConstraint("variableNameKey", ".//literalVariable|.//objectVariable|.//literalListVariable|.//objectListVariable", "@name"),
                CreateUniqueConstraint("folderNameKey", "./folder", "@name")
            });

            //variableType ComplexType
            XmlSchemaComplexType variableBaseType = CreateComplexType("variableBaseType",
                new XmlSchemaElement[]
                {
                    CreateSchemaElement("memberName", "string", 1, 1, false, true),
                    CreateSchemaElement("variableCategory", "VariableCategory", 1, 1),
                    CreateSchemaElement("castVariableAs", "string", 1, 1, false, true),
                    CreateSchemaElement("typeName", "string", 1, 1, false, true),
                    CreateSchemaElement("referenceName", "string", 1, 1, false, true),
                    CreateSchemaElement("referenceDefinition", "string", 1, 1, false, true),
                    CreateSchemaElement("castReferenceAs", "string", 1, 1, false, true),
                    CreateSchemaElement("referenceCategory", "ReferenceCategory", 1, 1),
                    CreateSchemaElement("evaluation", "VariableEvaluation", 1, 1),
                    CreateSchemaElement("comments", "string", 1, 1, false, true),
                    CreateSchemaElement("metadata", "string", 1, 1, false, true)
                },
                new XmlSchemaAttribute[]
                {
                    AttributeName
                });

            //subFolder Element
            XmlSchemaElement elementSubFolder = CreateSchemaElement("folder", "folderType", 0, null, true, false, new XmlSchemaObject[]
            {
                CreateUniqueConstraint("subFolderNameKey", "./folder", "@name")
            });

            //folderType ComplexType
            XmlSchemaComplexType folderType = CreateComplexType("folderType",
                new XmlSchemaObject[]
                {

                    GetChoiceParticle(new XmlSchemaElement[]
                    {
                        CreateSchemaElement("literalVariable", "literalVariableType"),
                        CreateSchemaElement("objectVariable", "objectVariableType"),
                        CreateSchemaElement("literalListVariable", "literalListVariableType"),
                        CreateSchemaElement("objectListVariable", "objectListVariableType")
                    }, 0, null, true),
                    elementSubFolder
                },
                new XmlSchemaAttribute[]
                {
                    AttributeName
                });

            //domainType ComplexType
            XmlSchemaComplexType domainType = CreateComplexType("domainType",
                new XmlSchemaElement[]
                {
                    CreateSchemaElement("item", "string", 0, null, true, true)
                },
                null);

            //domain Element
            XmlSchemaElement elementDomain = CreateSchemaElement("domain", "domainType", 1, 1, false, false, new XmlSchemaObject[]
            {
                CreateUniqueConstraint("itemValue", "./item", ".")
            });

            //literalVariableType ComplexType
            XmlSchemaComplexType literalVariableType = ExtendComplexType("literalVariableType", "variableBaseType",
                new XmlSchemaElement[]
                {
                    CreateSchemaElement("literalType", "LiteralType", 1, 1),
                    CreateSchemaElement("control", "LiteralVariableInputStyle", 1, 1),
                    CreateSchemaElement("propertySource", "string", 1, 1, false, true),
                    CreateSchemaElement("defaultValue", "string", 1, 1, false, true),
                    elementDomain
                },
                null);

            //objectVariableType ComplexType
            XmlSchemaComplexType objectVariableType = ExtendComplexType("objectVariableType", "variableBaseType",
                new XmlSchemaElement[]
                {
                    CreateSchemaElement("objectType", "AsemblyQualifiedNameType", 1, 1)
                },
                null);

            //literalListVariableType ComplexType
            XmlSchemaComplexType literalListVariableType = ExtendComplexType("literalListVariableType", "variableBaseType",
                new XmlSchemaElement[]
                {
                    CreateSchemaElement("literalType", "LiteralType", 1, 1),
                    CreateSchemaElement("listType", "ListType", 1, 1),
                    CreateSchemaElement("control", "ListInputStyle", 1, 1),
                    CreateSchemaElement("elementControl", "LiteralVariableInputStyle", 1, 1),
                    CreateSchemaElement("propertySource", "string", 1, 1, false, true),
                    CreateSchemaElement("defaultValue", "domainType", 1, 1, false, false, new XmlSchemaObject[]
                    {
                        CreateUniqueConstraint("itemValue1", "./item", ".")
                    }),
                    CreateSchemaElement("domain", "domainType", 1, 1, false, false, new XmlSchemaObject[]
                    {
                        CreateUniqueConstraint("itemValue2", "./item", ".")
                    })
                },
                null);

            //objectListVariableType ComplexType
            XmlSchemaComplexType objectListVariableType = ExtendComplexType("objectListVariableType", "variableBaseType",
                new XmlSchemaElement[]
                {
                    CreateSchemaElement("objectType", "AsemblyQualifiedNameType", 1, 1),
                    CreateSchemaElement("listType", "ListType", 1, 1),
                    CreateSchemaElement("control", "ListInputStyle", 1, 1)
                },
                null);

            //VariableCategory
            XmlSchemaSimpleType variableCategory = CreateEnumRestriction("VariableCategory", new string[]
            {
                "Property",
                "Field",
                "ArrayIndexer",
                "StringKeyIndexer",
                "IntegerKeyIndexer",
                "BooleanKeyIndexer",
                "DateTimeKeyIndexer",
                "DateTimeOffsetKeyIndexer",
                "DateOnlyKeyIndexer",
                "DateKeyIndexer",
                "TimeSpanKeyIndexer",
                "TimeOnlyKeyIndexer",
                "TimeOfDayKeyIndexer",
                "GuidKeyIndexer",
                "ByteKeyIndexer",
                "ShortKeyIndexer",
                "LongKeyIndexer",
                "FloatKeyIndexer",
                "DoubleKeyIndexer",
                "DecimalKeyIndexer",
                "CharKeyIndexer",
                "SByteKeyIndexer",
                "UShortKeyIndexer",
                "UIntegerKeyIndexer",
                "ULongKeyIndexer",
                "VariableKeyIndexer",
                "VariableArrayIndexer"
            });

            //ReferenceCategory
            XmlSchemaSimpleType referenceCategory = CreateEnumRestriction("ReferenceCategory", new string[]
            {
                "InstanceReference",
                "StaticReference",
                "Type",
                "This"
            });

            //VariableEvaluation
            XmlSchemaSimpleType variableEvaluation = CreateEnumRestriction("VariableEvaluation", new string[]
            {
                "Implemented",
                "Automatic"
            });

            //LiteralType
            XmlSchemaSimpleType literalType = CreateEnumRestriction("LiteralType", new string[]
            {
                "Decimal",
                "String",
                "Integer",
                "Boolean",
                "DateTime",
                "DateTimeOffset",
                "DateOnly",
                "Date",
                "TimeSpan",
                "TimeOnly",
                "TimeOfDay",
                "Guid",
                "Byte",
                "Short",
                "Long",
                "Float",
                "Double",
                "Char",
                "SByte",
                "UShort",
                "UInteger",
                "ULong",
                "NullableDecimal",
                "NullableInteger",
                "NullableBoolean",
                "NullableDateTime",
                "NullableDateTimeOffset",
                "NullableDateOnly",
                "NullableDate",
                "NullableTimeSpan",
                "NullableTimeOnly",
                "NullableTimeOfDay",
                "NullableGuid",
                "NullableByte",
                "NullableShort",
                "NullableLong",
                "NullableFloat",
                "NullableDouble",
                "NullableChar",
                "NullableSByte",
                "NullableUShort",
                "NullableUInteger",
                "NullableULong"
            });

            //AsemblyQualifiedNameType
            XmlSchemaSimpleType asemblyQualifiedNameType = CreatePatternRestriction("AsemblyQualifiedNameType", RegularExpressions.FULLYQUALIFIEDCLASSNAME);

            //LiteralVariableInputStyle
            XmlSchemaSimpleType literalVariableInputStyle = CreateEnumRestriction("LiteralVariableInputStyle", new string[]
            {
                "SingleLineTextBox",
                "MultipleLineTextBox",
                "DropDown",
                "TypeAutoComplete",
                "DomainAutoComplete",
                "PropertyInput"
            });

            //ListInputStyle
            XmlSchemaSimpleType listInputStyle = CreateEnumRestriction("ListInputStyle", new string[]
            {
                "ListForm",
                "HashSetForm"
            });

            //ListType
            XmlSchemaSimpleType listType = CreateEnumRestriction("ListType", new string[]
            {
                "Array",
                "GenericList",
                "GenericCollection",
                "IGenericList",
                "IGenericCollection",
                "IGenericEnumerable"
            });

            return CreateCompiledXmlSchemaSet(new XmlSchemaObject[]
            {
                elementFolder,
                variableBaseType,
                folderType,
                domainType,
                literalVariableType,
                objectVariableType,
                literalListVariableType,
                objectListVariableType,
                variableCategory,
                referenceCategory,
                variableEvaluation,
                literalType,
                asemblyQualifiedNameType,
                literalVariableInputStyle,
                listType,
                listInputStyle
            });

            //WriteSchema("C:\\Test\\variables.xsd", VariablesSchema);
        }

        private static void AddEnumeration(XmlSchemaObjectCollection restrictionFacets, string enumerationName)
        {
            XmlSchemaEnumerationFacet enumeration = new()
            {
                Value = enumerationName
            };
            restrictionFacets.Add(enumeration);
        }

        internal static string GetSchema(XmlSchema schema)
        {
            XmlNamespaceManager nsmgr = new(new NameTable());
            nsmgr.AddNamespace(SchemaConstants.NAMESPACEPREFIX, SchemaConstants.NAMESPACEURI);
            ServiceInterfaces.IXmlDocumentHelpers xmlDocumentHelpers = Microsoft.Extensions.DependencyInjection.ServiceProviderServiceExtensions.GetRequiredService<ServiceInterfaces.IXmlDocumentHelpers>(Program.ServiceProvider);

            System.Text.StringBuilder stringBuilder = new();
            using (XmlWriter xmlTextWriter = xmlDocumentHelpers.CreateFormattedXmlWriterWithDeclaration(stringBuilder))
            {
                schema.Write(xmlTextWriter, nsmgr);
            }
            return stringBuilder.ToString();
        }

        //private static void WriteSchema(string fullPath, XmlSchema xmlSchema)
        //{
        //    using System.IO.StreamWriter sr = new(fullPath, false, System.Text.Encoding.Unicode);
        //    sr.Write(GetSchema(xmlSchema));
        //}
        #endregion Methods

        #region EventHandlers
        private static void ValidateSchema(object? sender, ValidationEventArgs e)
        {
            if (e.Severity == XmlSeverityType.Error)
            {
                xmlSchemaErrors.Add(e.Message);
            }
            else
            {
                //Debug.Print(e.Message);
            }
        }
        #endregion EventHandlers
    }
}
