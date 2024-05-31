using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace domain.Models
{
    [Table("Favorites")]
    public class FavoriteModel
    {
        [Key]
        public int FavoriteId { get; set; }

        [ForeignKey("UserId")]
        public int UserId { get; set; }

        [ForeignKey("VacancyId")]
        public int VacancyId { get; set; }

        [JsonIgnore]
        public UserModel? User { get; set; }

        [JsonIgnore]
        public VacancyModel? Vacancy { get; set; }
    }
}
