using BlockBusterPOS.Interfaces.Models;
using System.ComponentModel.DataAnnotations;

namespace BlockBusterPOS.Models;

public class CustomerTransactionModel : ICustomerTransactionModel
{
    [Required]
    public CustomerModel Customer { get; init; } = null!;

    [Required]
    public IReadOnlyCollection<RentalModel> Rentals { get; set; } = null!;
  
    public decimal? TotalRentalPrice { get; set; }  
} 