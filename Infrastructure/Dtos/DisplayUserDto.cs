namespace Infrastructure.Dtos;

public class DisplayUserDto
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string Street { get; set; } = null!;
    public string City { get; set; } = null!;
    public string PostalCode { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string RoleName { get; set; } = null!;
}
