using System;
using System.Collections.Generic;

namespace PaymentGateway.Models;

public partial class PayPalTransaction
{
    public string TransactionId { get; set; } = null!;

    public int UserId { get; set; }

    public decimal Amount { get; set; }

    public string Currency { get; set; } = null!;

    public string? Status { get; set; }

    public DateTime CreatedDate { get; set; }

    public bool Paid { get; set; }

    public string? PayerId { get; set; }

    public virtual PUser User { get; set; } = null!;
}
