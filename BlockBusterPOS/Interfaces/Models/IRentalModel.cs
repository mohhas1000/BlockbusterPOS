using BlockBusterPOS.Enum;
using System.ComponentModel.DataAnnotations;

namespace BlockBusterPOS.Interfaces.Models;

/// <summary>
/// Represents a rental transaction for a movie.
/// </summary>
public interface IRentalModel
{
    /// <summary>
    /// Gets the type of rental <see cref="RentalType"/>
    /// </summary>
    [Required]
    public RentalType Type { get; }

    /// <summary>
    /// Gets the amount of movies rented.
    /// </summary>
    [Required]
    public int Count { get; }

    /// <summary>
    /// Gets the date the rental transaction was made.
    /// </summary>
    [Required]
    public DateTime RentalDate { get; }
}