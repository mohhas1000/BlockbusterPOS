using FluentValidation;
using BlockBusterPOS.Dto;

namespace BlockBusterPOS.Validators;

public class CreateCustomerTransactionDtoValidator : AbstractValidator<CreateCustomerTransactionDto>
{
    public CreateCustomerTransactionDtoValidator()
    {
        RuleFor(dto => dto.Customer.Id)
            .GreaterThan(0).WithMessage("{PropertyName} must be greater than zero.");

        RuleFor(dto => dto.Customer.Name)
            .NotEmpty().WithMessage("{PropertyName} is mandatory")
            .NotEqual("string").WithMessage("{PropertyName} should not be the default value 'string'");

        RuleFor(dto => dto.Customer.PhoneNumber)
            .NotEmpty().WithMessage("{PropertyName} is mandatory.")
            .Matches(@"^\+?[0-9]\d{1,16}$").WithMessage("{PropertyName} must be a valid phone number.");

        RuleForEach(dto => dto.Rentals).SetValidator(new RentalModelDtoBasiclValidator());
    }
}