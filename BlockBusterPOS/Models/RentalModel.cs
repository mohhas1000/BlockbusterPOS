using BlockBusterPOS.Enum;
using BlockBusterPOS.Interfaces.Models;
using System.ComponentModel.DataAnnotations;

namespace BlockBusterPOS.Models;

public class RentalModel : IRentalModel
{
    [Required]
    public RentalType Type { get; init; }

    [Required]
    public int Count { get; init; }

    [Required]
    public DateTime RentalDate { get; set; }
}
