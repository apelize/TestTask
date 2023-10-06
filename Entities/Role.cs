using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

#pragma warning disable 1591

namespace Entities;

public class Role
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public required string Access { get; set; }
    [JsonIgnore]
    public List<User> Users {get; set;}
}