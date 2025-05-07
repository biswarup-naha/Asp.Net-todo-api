using System;
using System.ComponentModel.DataAnnotations;

namespace TodoApi.Dtos;

public record class RegisterDto
{
    [Required]
    public string? Name { get; set; }

    [Required]
    [Range(1000000000, 9999999999)]
    public long? Phone { get; set; }

    [Required]
    [EmailAddress]
    public string? Email { get; set; }

    [Required]
    [MinLength(8)]
    public string? Password { get; set; }
}
