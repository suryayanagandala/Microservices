using Orders.CoreModels.Enums;

namespace Orders.CoreModels.Entities;

public sealed class Order
{
    public int OrderId { get; set; }

    public string UserId { get; set; } = default!;

    public string? FullName { get; set; }

    public string? PhoneNumber { get; set; }

    public string DeliveryAddress { get; set; } = default!;

    public decimal TotalAmount { get; set; }

    public DateOnly OrderDate { get; set; }

    public OrderStatus Status { get; set; } 
   
    public bool IsActive { get; set; } = true;

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset? UpdatedAt { get; set; }

    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
