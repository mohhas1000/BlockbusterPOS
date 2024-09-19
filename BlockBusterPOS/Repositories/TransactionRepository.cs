using BlockBusterPOS.Configuration;
using BlockBusterPOS.Interfaces.Repositories;
using BlockBusterPOS.Models;
using BlockBusterPOS.Extensions;
using Microsoft.Extensions.Options;

namespace BlockBusterPOS.Repositories;

public class TransactionRepository : ITransactionRepository
{
    private readonly CustomerTransactionOptions _customerTransactionOptions;
    private readonly RentalPriceOptions _rentalPriceOptions;

    private readonly Dictionary<int, CustomerTransactionModel> _customerTransactions = [];

    public TransactionRepository(IOptions<CustomerTransactionOptions> customerTransactionOptions, IOptions<RentalPriceOptions> rentalPriceOptions)
    {
        _customerTransactionOptions = customerTransactionOptions.Value;
        _rentalPriceOptions = rentalPriceOptions.Value;

        if (_customerTransactionOptions.TestData?.Any() == true)
        {
            _customerTransactions = _customerTransactionOptions.TestData
                .ToDictionary(
                data => data.Key,
                data =>
                {
                    var model = data.Value;
                    model.TotalRentalPrice ??= model.CalculateTotalRentalFee(_rentalPriceOptions);
                    return model;
                });
        }
    }

    public IReadOnlyCollection<CustomerTransactionModel> ListCustomerTransactions()
    {
        return _customerTransactions.Values.ToList().AsReadOnly();
    }

    public CustomerTransactionModel? TryGetCustomerTransaction(int customerId)
    {
        return _customerTransactions.FirstOrDefault(transaction => transaction.Key == customerId).Value;
    }

    public void CreateCustomerTransaction(CustomerTransactionModel input)
    {
        foreach (var rental in input.Rentals)
        {
            rental.RentalDate = DateTime.UtcNow;
        }

        if (_customerTransactions.TryGetValue(input.Customer.Id, out var customerTransaction))
        {
            customerTransaction.Rentals = customerTransaction.Rentals.Concat(input.Rentals).ToList();

            customerTransaction.TotalRentalPrice = customerTransaction.CalculateTotalRentalFee(_rentalPriceOptions);
        }
        else
        {
           input.TotalRentalPrice =  input.CalculateTotalRentalFee(_rentalPriceOptions);

            _customerTransactions.Add(input.Customer.Id, input);

        }
    }
}
