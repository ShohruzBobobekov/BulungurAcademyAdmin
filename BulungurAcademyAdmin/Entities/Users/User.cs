using BulungurAcademyAdmin.Entities.Enum;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace BulungurAcademyAdmin.Entities;

public class User : Auditable
{

    [JsonPropertyName("telegramId")]
    public long? TelegramId { get; set; }
    [JsonPropertyName("firstName")]
    public string FirstName { get; set; }
    [JsonPropertyName("lastName")]
    public string LastName { get; set; }
    [JsonPropertyName("phone")]
    public string Phone { get; set; }
    [JsonPropertyName("status")]
    public UserStatus Status { get; set; }
    [JsonPropertyName("isActive")]
    public bool IsActive {
        get
        {
            if (Status == UserStatus.Active)return true;
            return false;
        }
        set
        {
            if(value==true) Status = UserStatus.Active;
            else Status = UserStatus.Inactive;
        }
    }
    public UserRole UserRole { get; set; }
    
    public ICollection<ExamApplicant>? ExamApplicants { get; set; }

    public User(string firstName, string lastName, string phone, long? telegramId, UserRole userRole,UserStatus status=UserStatus.Inactive )
    {
        TelegramId = telegramId;
        Id = Guid.NewGuid();
        FirstName = firstName;
        LastName = lastName;
        Phone = phone;
        UserRole = userRole;
        Status = status;
    }
    public override string ToString()
    {
        return FirstName+" "+LastName+" "+Phone;
    }
}
