namespace BulungurAcademyAdmin.DataTranferObjects;

public record UserForCreaterDto(
    string firstName,
    string? lastName,
    string phoneNumber,
    long? telegramId
    );

