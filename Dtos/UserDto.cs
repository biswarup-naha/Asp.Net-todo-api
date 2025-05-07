using System;

namespace TodoApi.Dtos;

public record class UserDto
{
    public string? Id { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public long? Phone { get; set; }
}
