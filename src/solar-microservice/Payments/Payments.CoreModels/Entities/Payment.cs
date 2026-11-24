namespace Payments.CoreModels.Entities;

public sealed class Payment
{
    public int PaymentId { get; set; }

    public int OrderId { get; set; }

    public string PaymentMethod { get; set; } = default!;

    public string PaymentStatus { get; set; } = default!;

    public string TransactionId { get; set; } = default!;

    public decimal Amount { get; set; }

    public DateTimeOffset PaymentDate { get; set; }

    public bool IsActive { get; set; } = true;
}
