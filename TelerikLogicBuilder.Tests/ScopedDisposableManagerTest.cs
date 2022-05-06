using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using Xunit;

namespace TelerikLogicBuilder.Tests
{
    public class ScopedDisposableManagerTest
    {
        public ScopedDisposableManagerTest()
        {
            var services = new ServiceCollection();
            foreach(var service in ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection)
                services.Add(service);

            services.AddScoped<ScopedDisposable>();

            serviceProvider = services.BuildServiceProvider();
        }

        #region Fields
        private readonly IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
        public void CanCreateScopedDisposableManager()
        {
            //arrange
            using IScopedDisposableManager<ScopedDisposable> manager = serviceProvider.GetRequiredService<IScopedDisposableManager<ScopedDisposable>>();

            //assert
            Assert.NotNull(manager);
        }

        [Fact]
        public void DisposesOfScopedService()
        {
            //arrange
            IScopedDisposableManager<ScopedDisposable> manager = serviceProvider.GetRequiredService<IScopedDisposableManager<ScopedDisposable>>();
            ScopedDisposable scopedDisposable = manager.ScopedService;

            //act
            Assert.False(scopedDisposable.HasBeenDesposed);
            manager.Dispose();
            Assert.True(scopedDisposable.HasBeenDesposed);
        }
    }

    public sealed class ScopedDisposable : IDisposable
    {
        internal bool HasBeenDesposed { get; private set; }
        public void Dispose()
        {
            Console.WriteLine($"{nameof(ScopedDisposable)}.Dispose()");
            HasBeenDesposed = true;
        }
    }
}
