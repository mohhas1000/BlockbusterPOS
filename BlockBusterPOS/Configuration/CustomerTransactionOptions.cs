using BlockBusterPOS.Models;
using System.ComponentModel.DataAnnotations;

namespace BlockBusterPOS.Configuration;

public class CustomerTransactionOptions
{
    public const string SectionName = "CustomerTransaction";

    [Required]
    public IReadOnlyDictionary<int, CustomerTransactionModel>? TestData { get; init; } = null!;
}

