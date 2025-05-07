using System;
using System.ComponentModel.DataAnnotations;

namespace TodoApi.Dtos;

public record class LoginDto
{
    [Required]
    [EmailAddress]
    public string? Email;

    [Required]
    [MinLength(8)]
    public string? Password;
}

