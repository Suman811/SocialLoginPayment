using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using PaymentGateway.Controllers;
using PaymentGateway.Domain.DataAccessLayer.DTO;
using PaymentGateway.Service.IService;
using System;
using System.Threading.Tasks;
using Xunit;

namespace PaymentGateway.Testing.Controllers
{
    public class ValuesControllerTests
    {
        private MockRepository mockRepository;

        private Mock<IJwtTokenService> mockJwtTokenService;
        private Mock<IConfiguration> mockConfiguration;
        private Mock<IUserService> mockUserService;
        private Mock<ILogger<ValuesController>> mockLogger;

        public ValuesControllerTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);

            this.mockJwtTokenService = this.mockRepository.Create<IJwtTokenService>();
            this.mockConfiguration = this.mockRepository.Create<IConfiguration>();
            this.mockUserService = this.mockRepository.Create<IUserService>();
            this.mockLogger = this.mockRepository.Create<ILogger<ValuesController>>();
        }

        private ValuesController CreateValuesController()
        {
            return new ValuesController(
                this.mockJwtTokenService.Object,
                this.mockConfiguration.Object,
                this.mockUserService.Object,
                this.mockLogger.Object);
        }

        [Fact]
        public async Task GoogleLogin_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var valuesController = this.CreateValuesController();
            LoginDto login = null;

            // Act
            var result = await valuesController.GoogleLogin(
                login);

            // Assert
            Assert.True(false);
            this.mockRepository.VerifyAll();
        }

        [Fact]
        public async Task FacebookLogin_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var valuesController = this.CreateValuesController();
            FbLoginDto fblogin = null;

            // Act
            var result = await valuesController.FacebookLogin(
                fblogin);

            // Assert
            Assert.True(false);
            this.mockRepository.VerifyAll();
        }
    }
}
