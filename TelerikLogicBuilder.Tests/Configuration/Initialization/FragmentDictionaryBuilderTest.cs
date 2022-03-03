using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration.Initialization;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Xml;
using Xunit;

namespace TelerikLogicBuilder.Tests.Configuration.Initialization
{
    public class FragmentDictionaryBuilderTest
    {
        public FragmentDictionaryBuilderTest()
		{
			serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
		}

		#region Fields
		private readonly IServiceProvider serviceProvider;
		#endregion Fields

		[Fact]
		public void CanCreateFragmentDictionary()
		{
			//arrange
			IFragmentDictionaryBuilder fragmentDictionaryBuilder = serviceProvider.GetRequiredService<IFragmentDictionaryBuilder>();

			//act
			var result = fragmentDictionaryBuilder.GetDictionary(GetDocument());

			//assert
			Assert.Equal(2, result.Count);

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
	}
}
