using System.ComponentModel.DataAnnotations;

namespace BlockBusterPOS.Configuration;

public class RentalPriceOptions
{
    public const string SectionName = "RentalPrice";

    [Required]
    public RentalOptions DVD { get; init; } = null!;

    [Required]
    public RentalOptions BluRay { get; init; } = null!;
}

public class RentalOptions
{
    public decimal BasePrice { get; set; }
    public decimal Discount { get; set; }
}