using System;

namespace BulungurAcademyAdmin.DataTranferObjects;

public record ExamDto(
    Guid id,
    string? ExamName,
    DateTime ExamDate
    );
