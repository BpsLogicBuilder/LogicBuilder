using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Constructors;
using Contoso.Forms.Parameters.DataForm;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TelerikLogicBuilder.Tests.Constants;
using Xunit;

namespace TelerikLogicBuilder.Tests.Intellisense.Constructors
{
    public class ExistingConstructorFinderTest
    {
        public ExistingConstructorFinderTest()
        {
            Initialize();
        }

        #region Fields
        private IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
        public void ReturnsExistingConstructor()
        {
            //arrange
            IConstructorManager constructorManager = serviceProvider.GetRequiredService<IConstructorManager>();
            IExistingConstructorFinder existingConstructorFinder = serviceProvider.GetRequiredService<IExistingConstructorFinder>();
            IContextProvider contextProvider = serviceProvider.GetRequiredService<IContextProvider>();
            ConstructorInfo constructorInfo = typeof(DataFormSettingsParameters).GetConstructors().First();
            Constructor constructor = constructorManager.CreateConstructor(constructorInfo.Name, constructorInfo);
            Dictionary<string, Constructor> existingConstructors = new()
            {
                [constructorInfo.Name] = new Constructor
                (
                    constructor.Name,
                    constructor.TypeName,
                    constructor.Parameters,
                    constructor.GenericArguments,
                    constructor.Summary,
                    contextProvider
                )
            };

            //act
            Constructor foundConstructor = existingConstructorFinder.FindExisting(constructorInfo, existingConstructors);

            //assert
            Assert.NotNull(foundConstructor);
        }

        [Fact]
        public void ReturnsNullWhenConstructorNotFound()
        {
            //arrange
            IConstructorManager constructorManager = serviceProvider.GetRequiredService<IConstructorManager>();
            IExistingConstructorFinder existingConstructorFinder = serviceProvider.GetRequiredService<IExistingConstructorFinder>();
            IContextProvider contextProvider = serviceProvider.GetRequiredService<IContextProvider>();
            ConstructorInfo constructorInfo = typeof(DataFormSettingsParameters).GetConstructors().First();
            Constructor constructor = constructorManager.CreateConstructor(constructorInfo.Name, constructorInfo);
            Dictionary<string, Constructor> existingConstructors = new()
            {
                [constructorInfo.Name] = new Constructor
                (
                    constructor.Name,
                    constructor.TypeName,
                    constructor.Parameters,
                    constructor.GenericArguments,
                    constructor.Summary,
                    contextProvider
                )
            };

            //act
            Constructor foundConstructor = existingConstructorFinder.FindExisting(constructorInfo, existingConstructors);

            //assert
            Assert.NotNull(foundConstructor);
        }

        private void Initialize()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }
    }
}
