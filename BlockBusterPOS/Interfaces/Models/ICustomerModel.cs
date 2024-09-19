namespace BlockBusterPOS.Interfaces.Models;

/// <summary>
/// Represents a customer model used for rental transactions.
/// </summary>
public interface ICustomerModel
{
    /// <summary>
    /// Gets the id associated with customer.
    /// </summary>
    int Id { get; }

    /// <summary>
    /// Gets the name of the customer.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Gets the phone number of the customer.
    /// </summary>
    string PhoneNumber { get; }

    /// <summary>
    /// Gets the club member status of the customer. 
    /// </summary>
    bool IsClubMember { get; }
}