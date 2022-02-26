using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration.Initialization;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Xml;
using TelerikLogicBuilder.Tests.Constants;
using Xunit;

namespace TelerikLogicBuilder.Tests.Configuration.Initialization
{
    public class FragmentListInitializerTest
    {
        public FragmentListInitializerTest()
		{
			Initialize();
		}

		#region Fields
		private IServiceProvider serviceProvider;
		#endregion Fields

		[Fact]
		public void CanCreateFragmentList()
		{
			//arrange
			IFragmentListInitializer fragmentListInitializer = serviceProvider.GetRequiredService<IFragmentListInitializer>();
			ICreateProjectProperties createProjectProperties = serviceProvider.GetRequiredService<ICreateProjectProperties>();
			IConfigurationService configurationService = serviceProvider.GetRequiredService<IConfigurationService>();
			IPathHelper pathHelper = serviceProvider.GetRequiredService<IPathHelper>();
			IUpdateFragments updateFragments = serviceProvider.GetRequiredService<IUpdateFragments>();
			configurationService.ProjectProperties = createProjectProperties.Create
			(
				pathHelper.CombinePaths(TestFolders.LogicBuilderTests, this.GetType().Name),
				nameof(CanCreateFragmentList)
			);
			updateFragments.Update(GetDocument());

			//act
			var result = fragmentListInitializer.InitializeList();

			//assert
			Assert.Equal(2, result.Fragments.Count);
			Assert.Equal("Editing", result.FragmentsTreeFolder.FolderNames[0].Name);
			Assert.Single(result.FragmentsTreeFolder.FileNames);
			Assert.Single(result.FragmentsTreeFolder.FolderNames);
			Assert.Single(result.FragmentsTreeFolder.FolderNames[0].FileNames);
			Assert.Empty(result.FragmentsTreeFolder.FolderNames[0].FolderNames);

			static XmlDocument GetDocument()
			{
				XmlDocument xmlDocument = new();
				xmlDocument.LoadXml(@"<folder name=""Fragments"">
                                        <fragment name=""ParameterOperatorParameters"">
                                            <constructor name=""ParameterOperatorParameters"" visibleText =""ParameterOperatorParameters: parameterName=$it"">
                                            <genericArguments />
                                            <parameters>
                                                <literalParameter name=""parameterName"" >$it</literalParameter>
                                            </parameters>
                                            </constructor>
                                        </fragment>
										<folder name=""Editing"">
											<fragment name=""ParameterOperatorParameters1"">
											  <constructor name=""ParameterOperatorParameters"" visibleText=""ParameterOperatorParameters: parameterName=$it"">
												<genericArguments />
												<parameters>
												  <literalParameter name=""parameterName"">$it</literalParameter>
												</parameters>
											  </constructor>
											</fragment>
										</folder>
                                    </folder>");

				return xmlDocument;
			}
		}

		private void Initialize()
		{
			serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
		}
	}
}
