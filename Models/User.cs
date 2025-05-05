using System;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TodoApi.Models;

public class User
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public required string Id { get; set; }

    [BsonElement("name")]
    [Required]
    [StringLength(50)]
    public required string Name { get; set; }

    [BsonElement("email")]
    [Required]
    [EmailAddress]
    public required string Email { get; set; }

    [BsonElement("password")]
    [Required]
    [MinLength(8)]
    public required string Password { get; set; }

    [BsonElement("phone")]
    [Required]
    [Range(1000000000, 9999999999)]
    public long Phone { get; set; }
}
