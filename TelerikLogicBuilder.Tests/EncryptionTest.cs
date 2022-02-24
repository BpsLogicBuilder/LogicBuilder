using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using TelerikLogicBuilder.Tests.Constants;
using Xunit;
using FlowBuilder = ABIS.LogicBuilder.FlowBuilder;

namespace TelerikLogicBuilder.Tests
{
    public class EncryptionTest
    {
        public EncryptionTest()
        {
            Initialize();
        }

        [Fact]
        public void CanEncryptAndDecrypt()
        {
            const string toEnCrypt = "Today is a good day.";
            //arrange
            IEncryption helper = serviceProvider.GetRequiredService<IEncryption>();
            string encypted = helper.Encrypt(toEnCrypt);
            string decrypted = helper.Decrypt(encypted);

            //assert
            Assert.Equal(toEnCrypt, decrypted);
        }

        #region Fields
        private IServiceProvider serviceProvider;
        #endregion Fields

        private void Initialize()
        {
            serviceProvider = FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }
    }
}
