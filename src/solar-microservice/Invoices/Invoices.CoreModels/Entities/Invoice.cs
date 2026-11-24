namespace Invoices.CoreModels.Entities;

public sealed class Invoice
{
    public int InvoiceId { get; set; }

    public int OrderId { get; set; }

    public string CustomerName { get; set; } = default!;

    public string CustomerEmail { get; set; } = default!;

    public DateTimeOffset InvoiceDate { get; set; }

    public decimal Amount { get; set; }

    public decimal? Tax { get; set; }

    public decimal TotalAmount { get; set; }

    public string PaymentStatus { get; set; } = default!;

    public string OrderStatus { get; set; } = default!;

    public string PaymentMethod { get; set; } = default!;

    public bool IsActive { get; set; } = true;
}
