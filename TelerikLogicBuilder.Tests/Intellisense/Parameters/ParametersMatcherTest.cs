using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Parameters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xunit;

namespace TelerikLogicBuilder.Tests.Intellisense.Parameters
{
    public class ParametersMatcherTest
    {
        public ParametersMatcherTest()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }

        #region Fields
        private readonly IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
        public void ReturnsTrueForValidParameterMatch()
        {
            //arrange
            IParametersManager parametersManager = serviceProvider.GetRequiredService<IParametersManager>();
            IParametersMatcher parametersMatcher = serviceProvider.GetRequiredService<IParametersMatcher>();
            ParameterInfo[] parameters = typeof(TestClassWithChildContructor).GetConstructors().First().GetParameters();
            ICollection<ParameterNodeInfoBase> parameterNodeInfos = parametersManager.GetParameterNodeInfos(parameters);

            //act
            bool match = parametersMatcher.MatchParameters(parameterNodeInfos.ToList(), parameterNodeInfos.Select(p => p.Parameter).ToList());

            //assert
            Assert.True(match);
        }

        [Fact]
        public void ReturnsFalseForInalidParameterMatch()
        {
            //arrange
            IParametersManager parametersManager = serviceProvider.GetRequiredService<IParametersManager>();
            IParametersMatcher parametersMatcher = serviceProvider.GetRequiredService<IParametersMatcher>();
            ParameterInfo[] dataFormParameters = typeof(TestClassWithChildContructor).GetConstructors().First().GetParameters();
            ParameterInfo[] formControlParameters = typeof(ChildContructor).GetConstructors().First().GetParameters();
            ICollection<ParameterNodeInfoBase> dataFormParameterNodeInfos = parametersManager.GetParameterNodeInfos(dataFormParameters);
            ICollection<ParameterNodeInfoBase> formControlParameterNodeInfos = parametersManager.GetParameterNodeInfos(formControlParameters);

            //act
            bool match = parametersMatcher.MatchParameters(dataFormParameterNodeInfos.ToList(), formControlParameterNodeInfos.Select(p => p.Parameter).ToList());

            //assert
            Assert.False(match);
        }

        private class TestClassWithChildContructor
        {
            public TestClassWithChildContructor(string stringProperty, ChildContructor childContructor)
            {
                StringProperty = stringProperty;
                ChildContructor = childContructor;
            }

            public string StringProperty { get; set; }
            public ChildContructor ChildContructor { get; set; }
        }

        private class ChildContructor
        {
            public ChildContructor(string stringProperty)
            {
                StringProperty = stringProperty;
            }

            public string StringProperty { get; set; }
        }
    }
}
