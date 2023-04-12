using System;

namespace BulungurAcademyAdmin.DataTranferObjects;

public record ExamForModificationDto(
    Guid Id,
    string? name,
    DateTime? examDate
    );
