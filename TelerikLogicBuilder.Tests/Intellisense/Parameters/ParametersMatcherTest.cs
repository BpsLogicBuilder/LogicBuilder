using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Parameters;
using Contoso.Forms.Parameters.DataForm;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TelerikLogicBuilder.Tests.Constants;
using Xunit;

namespace TelerikLogicBuilder.Tests.Intellisense.Parameters
{
    public class ParametersMatcherTest
    {
        public ParametersMatcherTest()
        {
            Initialize();
        }

        #region Fields
        private IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
        public void ReturnsTrueForValidParameterMatch()
        {
            //arrange
            IParametersManager parametersManager = serviceProvider.GetRequiredService<IParametersManager>();
            IParametersMatcher parametersMatcher = serviceProvider.GetRequiredService<IParametersMatcher>();
            ParameterInfo[] parameters = typeof(DataFormSettingsParameters).GetConstructors().First().GetParameters();
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
            ParameterInfo[] dataFormParameters = typeof(DataFormSettingsParameters).GetConstructors().First().GetParameters();
            ParameterInfo[] formControlParameters = typeof(FormControlSettingsParameters).GetConstructors().First().GetParameters();
            ICollection<ParameterNodeInfoBase> dataFormParameterNodeInfos = parametersManager.GetParameterNodeInfos(dataFormParameters);
            ICollection<ParameterNodeInfoBase> formControlParameterNodeInfos = parametersManager.GetParameterNodeInfos(formControlParameters);

            //act
            bool match = parametersMatcher.MatchParameters(dataFormParameterNodeInfos.ToList(), formControlParameterNodeInfos.Select(p => p.Parameter).ToList());

            //assert
            Assert.False(match);
        }

        private void Initialize()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }
    }
}
