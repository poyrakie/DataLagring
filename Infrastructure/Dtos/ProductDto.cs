namespace Infrastructure.Dtos;

public class ProductDto
{
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public decimal Price { get; set; }
    public string CategoryName { get; set; } = null!;
    public string ManufacturerName { get; set; } = null!;
}
