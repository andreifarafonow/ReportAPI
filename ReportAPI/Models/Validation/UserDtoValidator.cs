using FluentValidation;
using ReportAPI.Models.DTO;

namespace ReportAPI.Models.Validation
{
    public class UserDtoValidator : AbstractValidator<UserDto>
    {
        public UserDtoValidator()
        {
            RuleFor(x => x.Email)
                .NotNull()
                .WithMessage(x => "Параметр 'email' отсутствует в запросе.")
                .NotEmpty()
                .WithMessage(x => "Параметр 'email' не должен быть пустым.")
                .MaximumLength(254)
                .WithMessage(x => "Длина параметра 'email' не должна превышать {MaxLength} символов.");

            RuleFor(x => x.Name)
                .NotNull()
                .WithMessage(x => "Параметр 'name' отсутствует в запросе.")
                .NotEmpty()
                .WithMessage(x => "Параметр 'name' не должен быть пустым.")
                .MaximumLength(100)
                .WithMessage(x => "Длина параметра 'name' не должна превышать {MaxLength} символов.");

            RuleFor(x => x.LastName)
                .NotNull()
                .WithMessage(x => "Параметр 'lastName' отсутствует в запросе.")
                .NotEmpty()
                .WithMessage(x => "Параметр 'lastName' не должен быть пустым.")
                .MaximumLength(100)
                .WithMessage(x => "Длина параметра 'lastName' не должна превышать {MaxLength} символов.");

            RuleFor(x => x.Patronymic)
                .MaximumLength(100)
                .WithMessage(x => "Длина параметра 'patronymic' не должна превышать {MaxLength} символов.");
        }
    }
}
