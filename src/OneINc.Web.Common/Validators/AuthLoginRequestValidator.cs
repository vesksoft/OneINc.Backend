using FluentValidation;
using OneINc.Web.Common.Models.Requests;

namespace OneINc.Web.Common.Validators
{
    public class AuthLoginRequestValidator : AbstractValidator<AuthLoginRequest>
    {
        public AuthLoginRequestValidator()
        {
            RuleFor(x => x).NotNull().WithMessage("Null request");
            RuleFor(x => x.Username).NotNull().NotEmpty().WithMessage("Username field is empty");
            RuleFor(x => x.Password).NotNull().NotEmpty().WithMessage("Password field is empty");
        }
    }
}
