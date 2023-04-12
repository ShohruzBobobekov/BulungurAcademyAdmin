using System;
using System.Text.Json.Serialization;

namespace BulungurAcademyAdmin.Entities;
public class ExamApplicant : Auditable
{
#pragma warning disable

    [JsonPropertyName("userId")]
    public Guid UserId { get; set; }
    [JsonPropertyName("examId")]
    public Guid ExamId { get; set; }
    [JsonPropertyName("firstSubjectId")]
    public Guid? FirstSubjectId { get; set; }
    [JsonPropertyName("secondSubjectId")]
    public Guid? SecondSubjectId { get; set;}
    [JsonPropertyName("isArrived")]
    public bool? IsArrived { get; set; } = false;
    [JsonPropertyName("isPayed")]
    public bool? IsPayed { get; set; } = false;
    [JsonPropertyName("user")]
    public User User { get; set; }
    [JsonPropertyName("exam")]
    public Exam Exam { get; set; }
    [JsonPropertyName("firstSubject")]
    public Subject FirstSubject { get; set; }
    [JsonPropertyName("secondSubject")]
    public Subject SecondSubject { get; set; }
}
