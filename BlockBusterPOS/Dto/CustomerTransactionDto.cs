namespace BlockBusterPOS.Dto;

public class CustomerTransactionDto
{
    public CustomerModelDto Customer { get; init; } = null!;

    public IReadOnlyCollection<RentalModelDto> Rentals { get; init; } = null!;

    public decimal TotalRentalPrice { get; init; }
}