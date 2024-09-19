using BlockBusterPOS.Configuration;
using BlockBusterPOS.Enum;
using BlockBusterPOS.Models;

namespace BlockBusterPOS.Extensions;

internal static class FeeCalculatorExtensions
{
    private const int ClubMemberDiscountThreshold = 4;
    private const decimal ClubMemberBaseFee = 100m;

    public static decimal CalculateTotalRentalFee(this CustomerTransactionModel model, RentalPriceOptions rentalPriceOptions)
    {
        ArgumentNullException.ThrowIfNull(model.Rentals, nameof(model.Rentals));
        ArgumentNullException.ThrowIfNull(model.Customer, nameof(model.Customer));

        return model.Customer.IsClubMember
            ? CalculateTotalFeeForClubMembers(model.Rentals, rentalPriceOptions)
            : CalculateTotalFeeForNonClubMembers(model.Rentals, rentalPriceOptions);
    }

    private static decimal CalculateTotalFeeForClubMembers(IReadOnlyCollection<RentalModel> rentalModels, RentalPriceOptions rentalPriceOptions)
    {
        IEnumerable<RentalModel> rentalsToCalculate = rentalModels;
        decimal totalFee = 0m;

        if (rentalModels.Sum(x => x.Count) >= ClubMemberDiscountThreshold)
        {
            var remainingRentals = rentalModels                       //Rentals after the discount package
                .SelectMany(r => Enumerable.Repeat(r.Type, r.Count))  // Enumerable.Repeat creates a sequence of rental type for each RentalModel and r.count specifies the number of times it appears.
                .OrderBy(Type => Type == RentalType.BluRay)           // DVDS (false) come before blu-rays (true) due to ascending order. 
                .Skip(ClubMemberDiscountThreshold)
                .GroupBy(type => type)
                .Select(s => new RentalModel { Type = s.Key, Count = s.Count() });

            totalFee = ClubMemberBaseFee;

            rentalsToCalculate = remainingRentals;
        }

        totalFee += CalculateFeeForAllRentalTypes(rentalsToCalculate, rentalPriceOptions, isClubMember: true);

        return totalFee;
    }

    private static decimal CalculateFeeForAllRentalTypes(IEnumerable<RentalModel> rentalsToCalculate, RentalPriceOptions rentalPriceOptions, bool isClubMember)
    {
        return System.Enum.GetValues(typeof(RentalType))
             .Cast<RentalType>()
             .Select(rentalType => new
             {
                 RentalType = rentalType,
                 RentalPrice = rentalType switch
                 {
                     RentalType.BluRay => rentalPriceOptions.BluRay,
                     RentalType.DVD => rentalPriceOptions.DVD,
                     _ => throw new NotSupportedException($"Unsupported rental type '{rentalType.GetType().Name}'.")
                 }
             }).Sum(x => CalculateFeeForRentalType(rentalsToCalculate, x.RentalType, x.RentalPrice, isClubMember));
    }

    private static decimal CalculateFeeForRentalType(IEnumerable<RentalModel> rentalModels, RentalType rentalType, RentalOptions rentalOptions, bool isClubMember)
    {
        var totalFeebyType = rentalModels
            .Where(r => r.Type == rentalType)
            .Sum(r => r.Count * rentalOptions.BasePrice);

        if (isClubMember)
        {
            totalFeebyType *= (1 - rentalOptions.Discount / 100m);
        }

        return totalFeebyType;
    }

    private static decimal CalculateTotalFeeForNonClubMembers(IEnumerable<RentalModel> rentalModels, RentalPriceOptions rentalPriceOptions)
    {
        return CalculateFeeForAllRentalTypes(rentalModels, rentalPriceOptions, isClubMember: false);
    }
}

