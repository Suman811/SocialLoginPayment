using System;
using System.Collections.Generic;

namespace PaymentGateway.Models;

public partial class PUser
{
    public int UserId { get; set; }

    public string? Name { get; set; }

    public string? Email { get; set; }

    public string? Password { get; set; }

    public string? FacebookId { get; set; }

    public string? GoogleId { get; set; }

    public bool? IsDeleted { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? CreatedBy { get; set; }

    public virtual ICollection<PayPalTransaction> PayPalTransactions { get; set; } = new List<PayPalTransaction>();

    public virtual ICollection<StripeTransaction> StripeTransactions { get; set; } = new List<StripeTransaction>();
}
