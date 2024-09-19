using BlockBusterPOS.Dto;
using BlockBusterPOS.Models;

namespace BlockBusterPOS.Interfaces.Services;

/// <summary>
/// Provides operations for managing customer transactions.
/// </summary>
public interface ITransactionService
{
    /// <summary>
    /// Gets a list of all customer transactions.
    /// </summary>
    /// <returns>A <see cref="IReadOnlyCollection{CustomerTransactionModel}"/>  containing all customer transactions. 
    /// Returns an empty collection if no transactions are found. </returns>
    IReadOnlyCollection<CustomerTransactionModel> ListCustomerTransactions();

    /// <summary>
    /// Gets a customer transaction by <paramref name="customerId"/>.
    /// </summary>
    /// <param name="customerId"> The customer id whose transaction is to be retrieved. </param>
    /// <returns>A <see cref="CustomerTransactionModel"/> representing the customer transaction,
    /// or <c>null</c> if the transaction is not found. </returns>
    CustomerTransactionModel? TryGetCustomerTransaction(int customerId);

    /// <summary>
    /// Creates a new customer transaction based on the provided <paramref name="input"/> 
    /// </summary>
    /// <param name="input">The details of the customer transaction to be created, provided as a <see cref="CreateCustomerTransactionDto"/> object. </param>
    void CreateCustomerTransaction(CustomerTransactionModel input);
}
