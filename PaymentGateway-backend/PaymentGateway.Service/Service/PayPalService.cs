using PaymentGateway.Service.IService;
using PayPalCheckoutSdk.Core;
using PayPalCheckoutSdk.Orders;
using PayPalHttp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentGateway.Service.Service
{
    public class PayPalService : IPayPalService
    {
        private readonly PayPalHttpClient _client;

        public PayPalService(string clientId, string clientSecret, bool isSandbox = true)
        {
            PayPalEnvironment environment = isSandbox
                ? new SandboxEnvironment(clientId, clientSecret)
                : new LiveEnvironment(clientId, clientSecret);

            _client = new PayPalHttpClient(environment);
        }

        public async Task<bool> VerifyTransactionAsync(string orderId, decimal expectedAmount, string expectedCurrency)
        {
            try
            {
                OrdersGetRequest request = new OrdersGetRequest(orderId);
                var response = await _client.Execute(request);
                var order = response.Result<Order>();

                if (order.Status == "COMPLETED" || order.Status == "APPROVED")
                {
                    var actualAmount = decimal.Parse(order.PurchaseUnits[0].AmountWithBreakdown.Value);
                    var actualCurrency = order.PurchaseUnits[0].AmountWithBreakdown.CurrencyCode;

                    return actualAmount == expectedAmount && actualCurrency == expectedCurrency;
                }

                return false;
            }
            catch (HttpException ex)
            {
                Console.WriteLine($"Error verifying PayPal transaction: {ex.Message}");
                return false;
            }
        }

        public async Task<Order> GetOrderDetailsAsync(string orderId)
        {
            try
            {
                OrdersGetRequest request = new OrdersGetRequest(orderId);
                var response = await _client.Execute(request);
                return response.Result<Order>();
            }
            catch (HttpException ex)
            {
                Console.WriteLine($"Error fetching PayPal order details: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> CaptureOrderAsync(string orderId)
        {
            try
            {
                OrdersCaptureRequest request = new OrdersCaptureRequest(orderId);
                var response = await _client.Execute(request);
                var capture = response.Result<Order>();

                return capture.Status == "COMPLETED";
            }
            catch (HttpException ex)
            {
                Console.WriteLine($"Error capturing PayPal order: {ex.Message}");
                return false;
            }
        }
    }
}
