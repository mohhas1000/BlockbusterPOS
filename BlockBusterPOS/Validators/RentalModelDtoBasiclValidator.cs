using BlockBusterPOS.Dto;
using FluentValidation;

namespace BlockBusterPOS.Validators;

public class RentalModelDtoBasiclValidator : AbstractValidator<RentalModelDtoBasic>
{
    public RentalModelDtoBasiclValidator()
    {
        RuleFor(rental => rental.Count)
            .GreaterThanOrEqualTo(0)
            .WithMessage((rental, count) => $"The count for {rental.Type} rentals must be greater than zero.");
    }
}