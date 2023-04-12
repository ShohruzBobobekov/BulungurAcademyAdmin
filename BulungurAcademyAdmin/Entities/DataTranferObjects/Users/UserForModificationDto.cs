using BulungurAcademyAdmin.Entities.Enum;
using System;

namespace BulungurAcademyAdmin.DataTranferObjects;

public record UserForModificationDto(
    Guid id,
    string? firstName,
    string? lastName,
    string? phoneNumber,
    UserStatus? status
    );
