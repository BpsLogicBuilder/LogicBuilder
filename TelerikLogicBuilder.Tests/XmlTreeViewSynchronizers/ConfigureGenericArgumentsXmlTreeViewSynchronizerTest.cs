using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlTreeViewSynchronizers;
using ABIS.LogicBuilder.FlowBuilder.Services.XmlTreeViewSynchronizers;
using ABIS.LogicBuilder.FlowBuilder.XmlTreeViewSynchronizers.Factories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Xml;
using Telerik.WinControls.UI;
using TelerikLogicBuilder.Tests.Mocks;
using Xunit;

namespace TelerikLogicBuilder.Tests.XmlTreeViewSynchronizers
{
    public class ConfigureGenericArgumentsXmlTreeViewSynchronizerTest
    {
        public ConfigureGenericArgumentsXmlTreeViewSynchronizerTest()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }

        #region Fields
        private readonly IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
        public void CreateConfigureGenericArgumentsXmlTreeViewSynchronizerThrows()
        {
            //assert
            Assert.Throws<InvalidOperationException>(() => serviceProvider.GetRequiredService<IConfigureGenericArgumentsXmlTreeViewSynchronizer>());
        }

        [Fact]
        public void ReplaceArgumentNodeSucceeds()
        {
            //arrange
            IXmlDocumentHelpers xmlDocumentHelpers = serviceProvider.GetRequiredService<IXmlDocumentHelpers>();
            IServiceFactory serviceFactory = serviceProvider.GetRequiredService<IServiceFactory>();
            IXmlTreeViewSynchronizerFactory xmlTreeViewSynchronizerFactory = serviceProvider.GetRequiredService<IXmlTreeViewSynchronizerFactory>();
            ITreeViewXmlDocumentHelper treeViewXmlDocumentHelper = serviceFactory.GetTreeViewXmlDocumentHelper
            (
                SchemaName.ParametersDataSchema
            );

            treeViewXmlDocumentHelper.LoadXmlDocument(GetXmlDocument(constructorXml).OuterXml);
            RadTreeView treeView = new()
            {
                Nodes =
                {
                    new RadTreeNode
                    {
                        Text = "GenericResponse`2",
                        Name = rootNodeXPath,
                        ImageIndex = ImageIndexes.TYPEIMAGEINDEX,
                        Nodes =
                        {
                            new RadTreeNode
                            {
                                Text = "App01",
                                Name = $"{rootNodeXPath}/literalParameter[@genericArgumentName=\"A\"]",
                                ImageIndex = ImageIndexes.LITERALPARAMETERIMAGEINDEX
                            },
                            new RadTreeNode
                            {
                                Text = "App02",
                                Name = $"{rootNodeXPath}/objectParameter[@genericArgumentName=\"B\"]",
                                ImageIndex = ImageIndexes.OBJECTPARAMETERIMAGEINDEX
                            }
                        }
                    }
                }
            };

            IConfigureGenericArgumentsXmlTreeViewSynchronizer synchronizer = xmlTreeViewSynchronizerFactory.GetConfigureGenericArgumentsXmlTreeViewSynchronizer
            (
                new ConfigureGenericArgumentsMock
                (
                    treeViewXmlDocumentHelper.ValidateXmlDocument,
                    treeView,
                    treeViewXmlDocumentHelper.XmlTreeDocument
                )
            );

            XmlElement newArgumentNode = xmlDocumentHelpers.AddElementToDoc
            (
                treeViewXmlDocumentHelper.XmlTreeDocument,
                xmlDocumentHelpers.ToXmlElement(@"<objectParameter genericArgumentName=""A"">
                                                                <objectType>Contoso.Test.Business.Responses.TypeNotFound</objectType>
                                                                <useForEquality>false</useForEquality>
                                                                <useForHashCode>false</useForHashCode>
                                                                <useForToString>true</useForToString>
                                                            </objectParameter>")
            );

            //act
            synchronizer.ReplaceArgumentNode(treeView.Nodes[0].Nodes[0], newArgumentNode);

            //assert
            Assert.Equal(2, treeView.Nodes[0].Nodes.Count);
            Assert.Equal(ImageIndexes.OBJECTPARAMETERIMAGEINDEX, treeView.Nodes[0].Nodes[0].ImageIndex);
            Assert.Equal(ImageIndexes.OBJECTPARAMETERIMAGEINDEX, treeView.Nodes[0].Nodes[1].ImageIndex);
        }

        [Fact]
        public void ReplaceArgumentNodeThrowsIfResultingXmlIsInvalid()
        {
            //arrange
            IXmlDocumentHelpers xmlDocumentHelpers = serviceProvider.GetRequiredService<IXmlDocumentHelpers>();
            IServiceFactory serviceFactory = serviceProvider.GetRequiredService<IServiceFactory>();
            IXmlTreeViewSynchronizerFactory xmlTreeViewSynchronizerFactory = serviceProvider.GetRequiredService<IXmlTreeViewSynchronizerFactory>();
            ITreeViewXmlDocumentHelper treeViewXmlDocumentHelper = serviceFactory.GetTreeViewXmlDocumentHelper
            (
                SchemaName.ParametersDataSchema
            );

            treeViewXmlDocumentHelper.LoadXmlDocument(GetXmlDocument(constructorXml).OuterXml);
            RadTreeView treeView = new()
            {
                Nodes =
                {
                    new RadTreeNode
                    {
                        Text = "GenericResponse`2",
                        Name = rootNodeXPath,
                        ImageIndex = ImageIndexes.TYPEIMAGEINDEX,
                        Nodes =
                        {
                            new RadTreeNode
                            {
                                Text = "App01",
                                Name = $"{rootNodeXPath}/literalParameter[@genericArgumentName=\"A\"]",
                                ImageIndex = ImageIndexes.LITERALPARAMETERIMAGEINDEX
                            },
                            new RadTreeNode
                            {
                                Text = "App02",
                                Name = $"{rootNodeXPath}/objectParameter[@genericArgumentName=\"B\"]",
                                ImageIndex = ImageIndexes.OBJECTPARAMETERIMAGEINDEX
                            }
                        }
                    }
                }
            };

            IConfigureGenericArgumentsXmlTreeViewSynchronizer synchronizer = xmlTreeViewSynchronizerFactory.GetConfigureGenericArgumentsXmlTreeViewSynchronizer
            (
                new ConfigureGenericArgumentsMock
                (
                    treeViewXmlDocumentHelper.ValidateXmlDocument,
                    treeView,
                    treeViewXmlDocumentHelper.XmlTreeDocument
                )
            );

            XmlElement newArgumentNode = xmlDocumentHelpers.AddElementToDoc
            (
                treeViewXmlDocumentHelper.XmlTreeDocument,
                xmlDocumentHelpers.ToXmlElement(@"<objectParameter genericArgumentName=""B"">
                                                                <objectType>Contoso.Test.Business.Responses.TypeNotFound</objectType>
                                                                <useForEquality>false</useForEquality>
                                                                <useForHashCode>false</useForHashCode>
                                                                <useForToString>true</useForToString>
                                                            </objectParameter>")
            );

            //assert
            Assert.Throws<LogicBuilderException>
            (
                () => synchronizer.ReplaceArgumentNode(treeView.Nodes[0].Nodes[0], newArgumentNode)
            );
        }

        private static XmlDocument GetXmlDocument(string xmlString)
        {
            XmlDocument xmlDocument = new();
            xmlDocument.LoadXml(xmlString);
            return xmlDocument;
        }

        private static readonly string rootNodeXPath = $"/{XmlDataConstants.CONSTRUCTORELEMENT}/{XmlDataConstants.GENERICARGUMENTSELEMENT}";
        private static readonly string constructorXml = @"<constructor name=""GenericResponse"" visibleText=""GenericResponse"">
                                                        <genericArguments>
                                                            <literalParameter genericArgumentName=""A"">
                                                                <literalType>String</literalType>
                                                                <control>SingleLineTextBox</control>
                                                                <useForEquality>false</useForEquality>
                                                                <useForHashCode>false</useForHashCode>
                                                                <useForToString>true</useForToString>
                                                                <propertySource />
                                                                <propertySourceParameter />
                                                                <defaultValue />
                                                                <domain>
						                                            <item>true</item>
						                                            <item>false</item>
					                                            </domain>
                                                            </literalParameter>
                                                            <objectParameter genericArgumentName=""B"">
                                                                <objectType>Contoso.Test.Business.Responses.TypeNotFound</objectType>
                                                                <useForEquality>false</useForEquality>
                                                                <useForHashCode>false</useForHashCode>
                                                                <useForToString>true</useForToString>
                                                            </objectParameter>
                                                        </genericArguments>
                                                        <parameters>
                                                            <literalParameter name=""aProperty"">SomeValue</literalParameter>
                                                            <objectParameter name=""bProperty"">
                                                                <constructor name=""TestResponseA"" visibleText=""TestResponseA"" >
                                                                    <genericArguments />
                                                                    <parameters>
                                                                      <literalParameter name=""stringProperty""> XX</literalParameter>
                                                                    </parameters>
                                                                </constructor>
                                                            </objectParameter>
                                                        </parameters>
                                                    </constructor>";
    }
}
