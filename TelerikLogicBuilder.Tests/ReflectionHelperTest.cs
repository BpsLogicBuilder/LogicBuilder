using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace TelerikLogicBuilder.Tests
{
    public class ReflectionHelperTest
    {
        public ReflectionHelperTest()
        {
            Initialize();
        }

        #region Fields
        private IServiceProvider serviceProvider;
        #endregion Fields

        private class ReflectionHelperTestClass
        {
            public ReflectionHelperTestClass
            (
                string field,
                string domElementId,
                string title,
                string placeHolder,
                string type,
                object someObject,
                List<string> someList
            )
            {
                Field = field;
                DomElementId = domElementId;
                Title = title;
                Placeholder = placeHolder;
                Type = type;
                SomeObject = someObject;
                SomeList = someList;
            }

            public string Field { get; set; }
            public string DomElementId { get; set; }
            public string Title { get; set; }
            public string Placeholder { get; set; }
            public string Type { get; set; }
            public object SomeObject { get; set; }
            public List<string> SomeList { get; set; }
        }

        [Fact]
        public void ComplexParameterCountReturnsExpectedValue()
        {
            //arrange
            IReflectionHelper helper = serviceProvider.GetRequiredService<IReflectionHelper>();

            //act
            var result = helper.ComplexParameterCount(typeof(ReflectionHelperTestClass).GetConstructors().First().GetParameters());//FormControlSettingsParameters

            //assert
            Assert.Equal(2, result);
        }

        private void Initialize()
        {
            serviceProvider = new ServiceCollection()
                .AddSingleton<IExceptionHelper, ExceptionHelper>()
                .AddSingleton<ITypeHelper, TypeHelper>()
                .AddSingleton<IReflectionHelper, ReflectionHelper>()
                .BuildServiceProvider();
        }
    }
}
