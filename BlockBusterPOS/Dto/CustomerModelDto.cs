namespace BlockBusterPOS.Dto;

public class CustomerModelDto
{
    public int Id { get; init; }

    public string Name { get; init; } = null!;

    public string PhoneNumber { get; init; } = null!;

    public bool IsClubMember { get; init; } = false;
}

