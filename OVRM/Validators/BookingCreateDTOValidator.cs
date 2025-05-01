using FluentValidation;
using OVRM.DTO;

namespace OVRM.Validators
{
    public class BookingCreateDTOValidator:AbstractValidator<BookingCreateDTO>
    {
        public BookingCreateDTOValidator()
        {
            RuleFor(x => x.StartDate)
                .NotEmpty().WithMessage("End date is required.")
                .Must(BeAValidDate).LessThan(x => x.EndDate).WithMessage("Start date must be less than end date.");

            RuleFor(x => x.EndDate)
                .NotEmpty().WithMessage("End date is required.")
                .Must(BeAValidDate).GreaterThan(x => x.StartDate).WithMessage("End date must be greater than start date.");
            RuleFor(x => x.VehicleId)
                .NotEmpty().WithMessage("Vehicle ID is required.");
        }
        private bool BeAValidDate(DateTime date)
        {
            return date != default(DateTime);
        }
    }
}
