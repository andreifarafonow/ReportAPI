using FluentValidation;
using ReportAPI.Models.DTO;

namespace ReportAPI.Models.Validation
{
    public class ReportDtoValidator : AbstractValidator<ReportDto>
    {
        public ReportDtoValidator()
        {
            RuleFor(x => x.Comment)
                .NotNull()
                .WithMessage(x => "Параметр 'comment' отсутствует в запросе.")
                .NotEmpty()
                .WithMessage(x => "Параметр 'comment' не должен быть пустым.")
                .MaximumLength(5000)
                .WithMessage(x => "Длина параметра 'comment' не должна превышать {MaxLength} символов.");

            RuleFor(x => x.HoursCount)
                .NotNull()
                .WithMessage(x => "Параметр 'hoursCount' отсутствует в запросе.")
                .GreaterThanOrEqualTo(1)
                .WithMessage(x => "Параметр 'hoursCount' не может иметь значение меньше 1");

            RuleFor(x => x.Date)
                .NotNull()
                .WithMessage(x => "Параметр 'date' отсутствует в запросе.")
                .NotEmpty()
                .WithMessage(x => "Параметр 'date' не должен быть пустым.");
        }
    }
}
