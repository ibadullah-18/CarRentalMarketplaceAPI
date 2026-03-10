using CarRentalMarketplaceAPI.DTOs.Cart;
using FluentValidation;

namespace CarRentalMarketplaceAPI.Validators
{
    public class AddCartItemDtoValidator : AbstractValidator<AddCartItemDto>
    {
        public AddCartItemDtoValidator()
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
}