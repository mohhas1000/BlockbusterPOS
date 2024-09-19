using System.ComponentModel.DataAnnotations;

namespace BlockBusterPOS.Configuration;

public class OpenApiOptions
{
    public const string SectionName = "OpenApi";

    [Required]
    public string Title { get; set; } = null!;

    [Required]
    public string Version { get; set; } = null!;

    [Required]
    public string Description { get; set; } = null!;
}