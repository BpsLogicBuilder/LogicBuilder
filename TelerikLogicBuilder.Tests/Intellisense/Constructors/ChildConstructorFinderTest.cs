using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Constructors;
using Contoso.Forms.Parameters.DataForm;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xunit;

namespace TelerikLogicBuilder.Tests.Intellisense.Constructors
{
    public class ChildConstructorFinderTest
    {
        public ChildConstructorFinderTest()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }

        #region Fields
        private readonly IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
        public void AddChildConstructorsAddsExpectedConstructors()
        {
            //arrange
            IChildConstructorFinder finder = serviceProvider.GetRequiredService<IChildConstructorFinder>();
            Dictionary<string, Constructor> existingConstructors = new();
            ParameterInfo[] parameters = typeof(DataFormSettingsParameters).GetConstructors().First().GetParameters();

            //act
            finder.AddChildConstructors(existingConstructors, parameters);

            //assert
            Assert.NotEmpty(existingConstructors);
        }
    }
}
