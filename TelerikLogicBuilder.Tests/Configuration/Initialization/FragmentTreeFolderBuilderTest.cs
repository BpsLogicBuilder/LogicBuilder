using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration.Initialization;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Xml;
using Xunit;

namespace TelerikLogicBuilder.Tests.Configuration.Initialization
{
    public class FragmentTreeFolderBuilderTest
    {
        public FragmentTreeFolderBuilderTest()
		{
			Initialize();
		}

		#region Fields
		private IServiceProvider serviceProvider;
		#endregion Fields

		[Fact]
		public void CanCreateFragmentTreeFolder()
		{
			//arrange
			IFragmentTreeFolderBuilder fragmentTreeFolderBuilder = serviceProvider.GetRequiredService<IFragmentTreeFolderBuilder>();

			//act
			var result = fragmentTreeFolderBuilder.GetTreeFolder(GetDocument());

			//assert
			Assert.Equal("Editing", result.FolderNames[0].Name);
			Assert.Single(result.FileNames);
			Assert.Single(result.FolderNames);
			Assert.Single(result.FolderNames[0].FileNames);
			Assert.Empty(result.FolderNames[0].FolderNames);

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
