using CarRentalMarketplaceAPI.DTOs.Rental;
using FluentValidation;

namespace CarRentalMarketplaceAPI.Validators;

public class CreateRentalDtoValidator : AbstractValidator<CreateRentalDto>
{
    public CreateRentalDtoValidator()
    {
        RuleFor(x => x.CarId)
            .NotEmpty().WithMessage("Maşın ID boş ola bilməz");

        RuleFor(x => x.StartDate)
            .NotEmpty().WithMessage("Başlama tarixi boş ola bilməz");

        RuleFor(x => x.EndDate)
            .NotEmpty().WithMessage("Bitmə tarixi boş ola bilməz")
            .GreaterThan(x => x.StartDate).WithMessage("Bitmə tarixi başlama tarixindən böyük olmalıdır");
    }
}
