using System;

namespace BulungurAcademyAdmin.DataTranferObjects;

public record ExamApplicantDto(
     Guid UserId,
     Guid ExamId,
     Guid? FirstSubjectId,
     Guid? SecondSubjectId
);