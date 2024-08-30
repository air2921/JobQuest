using System.ComponentModel.DataAnnotations;

namespace common.DTO.ModelDTO;

public class LanguageDTO
{
    [Required]
    public string Language { get; set; } = null!;

    [Required]
    public int Level { get; set; }
}
