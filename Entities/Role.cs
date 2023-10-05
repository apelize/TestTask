using System.Text.Json.Serialization;

namespace Entities;

public class Role
{
    public required int Id { get; set; }
    public required string Access { get; set; }
    [JsonIgnore]
    public List<User> Users { get; set; }
}