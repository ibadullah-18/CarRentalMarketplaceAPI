using CarRentalMarketplaceAPI.DTOs.Review;
using FluentValidation;

namespace CarRentalMarketplaceAPI.Validators
{
    public class CreateReviewDtoValidator : AbstractValidator<CreateReviewDto>
    {
        public CreateReviewDtoValidator()
        {
            RuleFor(x => x.CarId)
                .NotEmpty().WithMessage("Maşın ID boş ola bilməz");

            RuleFor(x => x.Rating)
                .InclusiveBetween(1, 5).WithMessage("Rating 1 ilə 5 arasında olmalıdır");

            RuleFor(x => x.Comment)
                .NotEmpty().WithMessage("Rəy boş ola bilməz")
                .MaximumLength(1000).WithMessage("Rəy ən çox 1000 simvol ola bilər");
        }
    }
}