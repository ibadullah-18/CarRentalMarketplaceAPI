using CarRentalMarketplaceAPI.DTOs.Auth;
using FluentValidation;

namespace CarRentalMarketplaceAPI.Validators;

public class RegisterDtoValidator : AbstractValidator<RegisterDto>
{
    public RegisterDtoValidator()
    {
        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage("Ad və soyad boş ola bilməz")
            .MinimumLength(3).WithMessage("Ad və soyad ən az 3 simvol olmalıdır")
            .MaximumLength(100).WithMessage("Ad və soyad ən çox 100 simvol ola bilər");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email boş ola bilməz")
            .EmailAddress().WithMessage("Email formatı düzgün deyil");

        RuleFor(x => x.Phone)
            .NotEmpty().WithMessage("Telefon nömrəsi boş ola bilməz")
            .MinimumLength(7).WithMessage("Telefon nömrəsi çox qısadır")
            .MaximumLength(20).WithMessage("Telefon nömrəsi çox uzundur");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Şifrə boş ola bilməz")
            .MinimumLength(6).WithMessage("Şifrə ən az 6 simvol olmalıdır")
            .Matches(@"^(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).+$")
            .WithMessage("Şifrədə ən az 1 böyük hərf, 1 rəqəm və 1 xüsusi simvol olmalıdır");

        RuleFor(x => x.DriverLicenseNumber)
            .NotEmpty().WithMessage("Sürücülük vəsiqəsi nömrəsi boş ola bilməz")
            .MinimumLength(5).WithMessage("Sürücülük vəsiqəsi nömrəsi çox qısadır")
            .MaximumLength(50).WithMessage("Sürücülük vəsiqəsi nömrəsi çox uzundur");
    }
}
