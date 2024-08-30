using Moq;
using PaymentGateway.Controllers;
using PaymentGateway.Domain.DataAccessLayer.DTO;
using PaymentGateway.Service.IService;
using System;
using System.Threading.Tasks;
using Xunit;

namespace PaymentGateway.Testing.Controllers
{
    public class PayPalControllerTests
    {
        private MockRepository mockRepository;

        private Mock<IPayPalService> mockPayPalService;
        private Mock<ITransactionService> mockTransactionService;

        public PayPalControllerTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);

            this.mockPayPalService = this.mockRepository.Create<IPayPalService>();
            this.mockTransactionService = this.mockRepository.Create<ITransactionService>();
        }

        private PayPalController CreatePayPalController()
        {
            return new PayPalController(
                this.mockPayPalService.Object,
                this.mockTransactionService.Object);
        }

        [Fact]
        public async Task ProcessRequest_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var payPalController = this.CreatePayPalController();
            PayPalRequest request = null;

            // Act
            var result = await payPalController.ProcessRequest(
                request);

            // Assert
            Assert.True(false);
            this.mockRepository.VerifyAll();
        }
    }
}
