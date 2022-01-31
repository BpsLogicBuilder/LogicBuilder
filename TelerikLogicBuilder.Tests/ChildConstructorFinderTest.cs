using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense;
using ABIS.LogicBuilder.FlowBuilder.Services;
using ABIS.LogicBuilder.FlowBuilder.Services.Intellisense;
using Contoso.Forms.Parameters.DataForm;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TelerikLogicBuilder.Tests.Constants;
using Xunit;

namespace TelerikLogicBuilder.Tests
{
    public class ChildConstructorFinderTest
    {
        public ChildConstructorFinderTest()
        {
            Initialize();
        }

        #region Fields
        private IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
        [Trait(TraitTypes.TestCategory, TestCategories.IntegrationTest)]
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

        private void Initialize()
        {
            serviceProvider = new ServiceCollection()
                .AddSingleton<IEnumHelper, EnumHelper>()
                .AddSingleton<IExceptionHelper, ExceptionHelper>()
                .AddSingleton<IMemberAttributeReader, MemberAttributeReader>()
                .AddSingleton<IParameterAttributeReader, ParameterAttributeReader>()
                .AddSingleton<IStringHelper, StringHelper>()
                .AddSingleton<IPathHelper, PathHelper>()
                .AddSingleton<IXmlDocumentHelpers, XmlDocumentHelpers>()
                .AddSingleton<IReflectionHelper, ReflectionHelper>()
                .AddSingleton<ITypeHelper, TypeHelper>()
                .AddSingleton<IContextProvider, ContextProvider>()
                .AddSingleton<IChildConstructorFinder, ChildConstructorFinder>()
                .BuildServiceProvider();
        }
    }
}
