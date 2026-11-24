namespace Orders.CoreModels.Entities;

public sealed class OrderItem
{
    public int OrderItemId { get; set; }

    public int OrderId { get; set; }

    public int ProductId { get; set; }

    public int Quantity { get; set; } = 1;

    public decimal UnitPrice { get; set; }

    public decimal TotalPrice { get; private set; }

    public Order Order { get; set; } = default!;
}
