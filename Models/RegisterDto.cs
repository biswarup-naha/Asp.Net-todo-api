using System;
using System.ComponentModel.DataAnnotations;

namespace TodoApi.Models;

public class RegisterDto
{
    [Required]
    public string? Name { get; set; }

    [Required]
    [Range(1000000000, 9999999999)]
    public string? Phone { get; set; }

    [Required]
    [EmailAddress]
    public string? Email { get; set; }

    [Required]
    [MinLength(8)]
    public string? Password { get; set; }
}
