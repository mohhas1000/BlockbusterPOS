using BlockBusterPOS.Models;
using BlockBusterPOS.Enum;
using System.ComponentModel.DataAnnotations;

namespace BlockBusterPOS.Interfaces.Models;

/// <summary>
///  Provides properties for a transcation that details a customer's rented movies.
/// </summary>
public interface ICustomerTransactionModel
{
    /// <summary>
    /// Gets the customer assoicated with the transaction.
    /// </summary>
    CustomerModel Customer { get; }

    /// <summary>
    /// Gets the collection of rentals for the customer.
    /// Each rental item includes details such as the type of rental (e.g., DVD, Blu-ray) 
    /// and the quantity rented. The rental type is specified using the <see cref="RentalType"/> enum.
    /// </summary>
    [Required]
    public IReadOnlyCollection<RentalModel> Rentals { get; }
}