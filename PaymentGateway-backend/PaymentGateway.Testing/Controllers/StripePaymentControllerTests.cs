using Microsoft.Extensions.Configuration;
using Moq;
using PaymentGateway.Controllers;
using PaymentGateway.Domain.DataAccessLayer.DTO;
using PaymentGateway.Service.IService;
using System;
using System.Threading.Tasks;
using Xunit;

namespace PaymentGateway.Testing.Controllers
{
    public class StripePaymentControllerTests
    {
        private MockRepository mockRepository;

        private Mock<IStripeService> mockStripeService;
        private Mock<IConfiguration> mockConfiguration;
        private Mock<ITransactionService> mockTransactionService;

        public StripePaymentControllerTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);

            this.mockStripeService = this.mockRepository.Create<IStripeService>();
            this.mockConfiguration = this.mockRepository.Create<IConfiguration>();
            this.mockTransactionService = this.mockRepository.Create<ITransactionService>();
        }

        private StripePaymentController CreateStripePaymentController()
        {
            return new StripePaymentController(
                this.mockStripeService.Object,
                this.mockConfiguration.Object,
                this.mockTransactionService.Object);
        }

        [Fact]
        public async Task CreatePayment_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var stripePaymentController = this.CreateStripePaymentController();
            PaymentRequest request = null;

            // Act
            var result = await stripePaymentController.CreatePayment(
                request);

            // Assert
            Assert.True(false);
            this.mockRepository.VerifyAll();
        }
    }
}
