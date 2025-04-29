using System;

namespace TodoApi.Models;

public class JwtSettings
{
    public string Secret { get; set; }
    public int ExpiryInMinutes { get; set; }
}
