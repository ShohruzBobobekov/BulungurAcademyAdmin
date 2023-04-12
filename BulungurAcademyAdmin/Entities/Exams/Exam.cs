using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace BulungurAcademyAdmin.Entities;

public class Exam : Auditable
{
    [JsonPropertyName("examName")]
    public string? ExamName { get; set; }
    //[JsonIgnore]
    public string SubjectsNames
    {
        get
        {
            string s = " ";
            if (Subjects is null) return s;
            foreach (var item in Subjects)
            {
                s = s + item.Name + ", ";
            }
            return s;
        }

    }
    [JsonPropertyName("examDate")]
    public DateTime ExamDate { get; set; }
    //[JsonIgnore]
    public ICollection<ExamApplicant>? ExamApplicants { get; set; }
    [JsonPropertyName("subjects")]
    //[JsonIgnore]
    public ICollection<Subject>? Subjects { get; set; }

    public Exam() { }
    public Exam(string? examName, DateTime examDate = default)
    {
        ExamName = examName;
        ExamDate = examDate;
    }
    public override string ToString()
    {
        return ExamName;
    }
}
