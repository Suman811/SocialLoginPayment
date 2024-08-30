using Stripe.Climate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PayPalCheckoutSdk.Orders;

namespace PaymentGateway.Service.IService
{
    public interface IPayPalService
    {
        Task<bool> VerifyTransactionAsync(string orderId, decimal expectedAmount, string expectedCurrency);
        Task<PayPalCheckoutSdk.Orders.Order> GetOrderDetailsAsync(string orderId);
        Task<bool> CaptureOrderAsync(string orderId);
    }
}
