using BlockBusterPOS.Enum;

namespace BlockBusterPOS.Dto;

public class RentalModelDto
{
    public RentalType Type { get; init; }

    public int Count { get; init; }

    public DateTime RentalDate { get; init; }
}
