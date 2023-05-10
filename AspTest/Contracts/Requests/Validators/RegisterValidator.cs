using AspTest.Contracts.Requests.Models;
using AspTest.Types;
using FluentValidation;

namespace AspTest.Contracts.Requests.Validators;

public class RegisterValidator : AbstractValidator<Register>
{
    private readonly string[] _groupCodes =
    {
        UserGroupCode.User,
        UserGroupCode.Admin
    };

    public RegisterValidator()
    {
        RuleFor(x => x.Login)
            .NotEmpty();

        RuleFor(x => x.Password)
            .NotEmpty();

        RuleFor(x => x.GroupCode)
            .Must(x => _groupCodes.Contains(x)).WithMessage("Invalid group code");
    }
}