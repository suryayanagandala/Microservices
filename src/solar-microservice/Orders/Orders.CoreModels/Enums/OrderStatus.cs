namespace Orders.CoreModels.Enums;

public enum OrderStatus
{
    CREATED,
    PAYMENT_PENDING,
    PAID,
    COD_PENDING,
    CONFIRMED,
    SHIPPED,
    DELIVERED,
    CANCELLED,
    PAYMENT_FAILED
}
