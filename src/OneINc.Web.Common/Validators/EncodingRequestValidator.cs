using FluentValidation;
using OneINc.Web.Common.Models.Requests;

namespace OneINc.Web.Common.Validators
{
    public class EncodingRequestValidator : AbstractValidator<EncodingRequest>
    {
        public EncodingRequestValidator() 
        {
            RuleFor(x => x).NotNull().WithMessage("Null request");
            RuleFor(x => x.Content).NotNull().NotEmpty().WithMessage("Empty content");
        }
    }
}
