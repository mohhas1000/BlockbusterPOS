using BlockBusterPOS.Interfaces.Repositories;
using BlockBusterPOS.Interfaces.Services;
using BlockBusterPOS.Models;

namespace BlockBusterPOS.Services;

public class TransactionService : ITransactionService
{
    private readonly ITransactionRepository _transactionRepository;

    public TransactionService(ITransactionRepository transactionRepository)
    {
        _transactionRepository = transactionRepository; 
    }

    public IReadOnlyCollection<CustomerTransactionModel> ListCustomerTransactions()
    {
        return _transactionRepository.ListCustomerTransactions();
    }

    public CustomerTransactionModel? TryGetCustomerTransaction(int customerId)
    {
        ArgumentNullException.ThrowIfNull(customerId, nameof(customerId));

        return _transactionRepository.TryGetCustomerTransaction(customerId);
    }

    public void CreateCustomerTransaction(CustomerTransactionModel input)
    {
        _transactionRepository.CreateCustomerTransaction(input);
    }
}

