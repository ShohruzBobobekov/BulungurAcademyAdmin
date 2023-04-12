using System;

namespace BulungurAcademyAdmin.DataTranferObjects.Exams;

public record ExamForCreationDto(
    string name,
    DateTime examDate
    );
