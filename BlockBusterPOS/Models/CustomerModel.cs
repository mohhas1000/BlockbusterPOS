using System.ComponentModel.DataAnnotations;
using BlockBusterPOS.Interfaces.Models;

namespace BlockBusterPOS.Models;

public class CustomerModel : ICustomerModel
{
    [Required]
    public int Id { get; init; }

    [Required]
    public string Name { get; init; } = null!;

    [Required]
    public string PhoneNumber { get; init; } = null!;

    [Required]
    public bool IsClubMember { get; init; } = false!;
}
