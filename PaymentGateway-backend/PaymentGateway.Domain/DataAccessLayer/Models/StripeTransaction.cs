using System;
using System.Collections.Generic;

namespace PaymentGateway.Models;

public partial class StripeTransaction
{
    public string TransactionId { get; set; } = null!;

    public int UserId { get; set; }

    public decimal Amount { get; set; }

    public string Currency { get; set; } = null!;

    public string? PaymentMethodId { get; set; }

    public string? PaymentMethodBrand { get; set; }

    public string? Description { get; set; }

    public string? Status { get; set; }

    public string? ReceiptUrl { get; set; }

    public DateTime CreatedDate { get; set; }

    public bool Paid { get; set; }

    public string? BalanceTransactionId { get; set; }

    public string? CustomerId { get; set; }

    public string? PaymentIntentId { get; set; }

    public string? SourceId { get; set; }

    public string? ShippingAddress { get; set; }

    public virtual PUser User { get; set; } = null!;
}
