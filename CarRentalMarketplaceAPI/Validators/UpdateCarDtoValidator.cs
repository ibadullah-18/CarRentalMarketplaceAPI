using CarRentalMarketplaceAPI.DTOs.Car;
using FluentValidation;

namespace CarRentalMarketplaceAPI.Validators;

public class UpdateCarDtoValidator : AbstractValidator<UpdateCarDto>
{
    public UpdateCarDtoValidator()
    {
        RuleFor(x => x.Brand)
            .NotEmpty().WithMessage("Marka boş ola bilməz")
            .MaximumLength(50).WithMessage("Marka ən çox 50 simvol ola bilər");

        RuleFor(x => x.Model)
            .NotEmpty().WithMessage("Model boş ola bilməz")
            .MaximumLength(50).WithMessage("Model ən çox 50 simvol ola bilər");

        RuleFor(x => x.Year)
            .InclusiveBetween(1900, DateTime.UtcNow.Year + 1)
            .WithMessage("İl düzgün deyil");

        RuleFor(x => x.PricePerDay)
            .GreaterThan(0).WithMessage("Günlük qiymət 0-dan böyük olmalıdır");

        RuleFor(x => x.FuelType)
            .NotEmpty().WithMessage("Yanacaq növü boş ola bilməz");

        RuleFor(x => x.Transmission)
            .NotEmpty().WithMessage("Sürət qutusu boş ola bilməz");

        RuleFor(x => x.Mileage)
            .GreaterThanOrEqualTo(0).WithMessage("Yürüş mənfi ola bilməz");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Açıqlama boş ola bilməz")
            .MaximumLength(1000).WithMessage("Açıqlama ən çox 1000 simvol ola bilər");

        RuleFor(x => x.Location)
            .NotEmpty().WithMessage("Yerləşmə boş ola bilməz")
            .MaximumLength(100).WithMessage("Yerləşmə ən çox 100 simvol ola bilər");

        RuleFor(x => x.Color)
            .NotEmpty().WithMessage("Rəng boş ola bilməz")
            .MaximumLength(30).WithMessage("Rəng ən çox 30 simvol ola bilər");
    }
}