using System.Text.Json.Serialization;

namespace BulungurAcademyAdmin.Entities;
public class Subject : Auditable
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    public Subject(string name)
    {
        Name = name;
    }
    public override string ToString()
    {
        return Name;
    }
}
