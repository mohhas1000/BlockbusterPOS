using BlockBusterPOS.Models;

namespace BlockBusterPOS.Dto;

public class CreateCustomerTransactionDto
{
    /// <example>
    /// {
    ///     "id": 0,
    ///     "name": "string",
    ///     "phoneNumber": "string",
    ///     "isClubMember": false
    /// }
    /// </example>
    public CustomerModel Customer { get; init; } = null!;

    /// <example>
    /// [
    ///     {
    ///         "type": "DVD",
    ///         "count": 2
    ///     },
    ///     {
    ///         "type": "BluRay",
    ///         "count": 1
    ///     }
    /// ]
    /// </example>
    public IReadOnlyCollection<RentalModelDtoBasic> Rentals { get; init; } = null!;
}
