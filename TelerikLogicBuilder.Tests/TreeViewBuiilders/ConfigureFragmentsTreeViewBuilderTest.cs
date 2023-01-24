using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.TreeViewBuiilders;
using ABIS.LogicBuilder.FlowBuilder.TreeViewBuiilders.Factories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Xml;
using Telerik.WinControls.UI;
using TelerikLogicBuilder.Tests.Mocks;
using Xunit;

namespace TelerikLogicBuilder.Tests.TreeViewBuiilders
{
    public class ConfigureFragmentsTreeViewBuilderTest
    {
        public ConfigureFragmentsTreeViewBuilderTest()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }

        #region Fields
        private readonly IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
        public void CreateConfigureFragmentsTreeViewBuilderThrows()
        {
            //assert
            Assert.Throws<InvalidOperationException>(() => serviceProvider.GetRequiredService<IConfigureFragmentsTreeViewBuilder>());
        }

        [Fact]
        public void CanBuildConfigureFragmentsTreeView()
        {
            //arrange
            ITreeViewBuilderFactory factory = serviceProvider.GetRequiredService<ITreeViewBuilderFactory>();
            RadTreeView radTreeView = new();
            XmlDocument xmlDocument = GetXmlDocument(@"<folder name=""Fragments"">
                                                          <folder name=""Lambda"">
                                                            <fragment name=""ParameterOperatorParameters"">
                                                              <constructor name=""ParameterOperatorParameters"" visibleText=""ParameterOperatorParameters: parameterName=$it"">
                                                                <genericArguments />
                                                                <parameters>
                                                                  <literalParameter name=""parameterName"">$it</literalParameter>
                                                                </parameters>
                                                              </constructor>
                                                            </fragment>
                                                          </folder>
                                                          <folder name=""Editing"">
                                                            <fragment name=""ParameterOperatorParameters1"">
                                                              <constructor name=""ParameterOperatorParameters"" visibleText=""ParameterOperatorParameters: parameterName=$it"">
                                                                <genericArguments />
                                                                <parameters>
                                                                  <literalParameter name=""parameterName"">$it</literalParameter>
                                                                </parameters>
                                                              </constructor>
                                                            </fragment>
                                                            <folder name=""Selectors"">
                                                              <fragment name=""DropDownTemplateControl"">
                                                                <constructor name=""FormControlSettingsParameters"" visibleText=""FormControlSettingsParameters: field=County;title=County;placeholder=County (required);stringFormat={0};Type: type;FieldValidationSettingsParameters: validationSetting;DropDownTemplateParameters: dropDownTemplate;fieldTypeSource=Enrollment.Domain.Entities.PersonalModel"">
                                                                  <genericArguments />
                                                                  <parameters>
                                                                    <literalParameter name=""field"">County</literalParameter>
                                                                    <literalParameter name=""title"">County</literalParameter>
                                                                    <literalParameter name=""placeholder"">County (required)</literalParameter>
                                                                    <literalParameter name=""stringFormat"">{0}</literalParameter>
                                                                    <literalParameter name=""fieldTypeSource"">Enrollment.Domain.Entities.PersonalModel</literalParameter>
                                                                  </parameters>
                                                                </constructor>
                                                              </fragment>
                                                            </folder>
                                                          </folder>
                                                        </folder>");

            //act
            factory.GetConfigureFragmentsTreeViewBuilder(new ConfigureFragmentsFormMock()).Build(radTreeView, xmlDocument);

            //assert
            Assert.Single(radTreeView.Nodes);
            Assert.Equal(2, radTreeView.Nodes[0].Nodes.Count);//Fragments nodes count
            Assert.Equal(2, radTreeView.Nodes[0].Nodes[0].Nodes.Count);//Editing nodes count (Editing comes first because of sorting)
            Assert.Equal(ImageIndexes.FILEIMAGEINDEX, radTreeView.Nodes[0].Nodes[0].Nodes[0].ImageIndex);//ParameterOperatorParameters1
            Assert.Equal(ImageIndexes.CLOSEDFOLDERIMAGEINDEX, radTreeView.Nodes[0].Nodes[0].Nodes[1].ImageIndex);//Selectors
            Assert.Single(radTreeView.Nodes[0].Nodes[1].Nodes);//Lambda nodes count
            Assert.Equal(ImageIndexes.FILEIMAGEINDEX, radTreeView.Nodes[0].Nodes[1].Nodes[0].ImageIndex);//ParameterOperatorParameters
        }

        private static XmlDocument GetXmlDocument(string xmlString)
        {
            XmlDocument xmlDocument = new();
            xmlDocument.LoadXml(xmlString);
            return xmlDocument;
        }
    }
}
