using BlockBusterPOS.Models;

namespace BlockBusterPOS.Interfaces.Repositories;

/// <summary>
/// Provides operations for managing customer transactions.
/// </summary>
public interface ITransactionRepository
{
    /// <summary>
    /// Gets a list of all customer transactions.
    /// </summary>
    /// <returns>An <see cref="IReadOnlyCollection{CustomerTransactionModel}"/> contaiing all customer transactions. </returns>
    IReadOnlyCollection<CustomerTransactionModel> ListCustomerTransactions();

    /// <summary>
    /// Gets a specific customer transaction by <paramref name="customerId"/>
    /// </summary>
    /// <param name="customerId"> The customer id whose transaction is to be retrieved. </param>
    /// <returns>A <see cref="CustomerTransactionModel"/> representing the customer transaction. </returns>
    CustomerTransactionModel? TryGetCustomerTransaction(int customerId);

    /// <summary>
    /// Creates a new customer transaction based on the provided <see cref="CustomerTransactionModel"/>.
    /// </summary>
    /// <param name="transaction">The details of the customer transaction to be created. </param>
    void CreateCustomerTransaction(CustomerTransactionModel transaction);
}
