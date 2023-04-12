using System;
using System.Text.Json.Serialization;

namespace BulungurAcademyAdmin.Entities;

public class Auditable
{
    public int Index { get; set; }

    [JsonPropertyName("id")]
    public Guid Id { get; set; }
    [JsonPropertyName("createdAt")]
    public DateTime CreatedAt { get; set; }
    [JsonPropertyName("updatedAt")]
    public DateTime UpdatedAt { get; set; }
}
