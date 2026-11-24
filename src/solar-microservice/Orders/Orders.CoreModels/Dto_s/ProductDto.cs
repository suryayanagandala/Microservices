namespace Orders.CoreModels.Dto_s;

public sealed record ProductDto
{
    public int ProductId { get; init; }

    public string ProductName { get; init; } = default!;

    public decimal Price { get; init; }

    public bool IsActive { get; init; }

    public string? ImageUrl { get; init; }
}
