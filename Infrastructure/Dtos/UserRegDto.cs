﻿using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Dtos;

public class UserRegDto
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Street { get; set; } = null!;
    public string City { get; set; } = null!;
    public string PostalCode { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
}
