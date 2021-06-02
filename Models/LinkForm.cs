#nullable enable
using System.ComponentModel.DataAnnotations;

namespace TeacherWebsiteBackEnd.Models
{
    public class LinkForm
    {
        [Key]
        public int? Id { get; set; }
        [Required]
        [StringLength(20)]
        public string? Type { get; set; }
        public string? Name { get; set; }
        [Required]
        public string? Url { get; set; }
        public string? IconUrl { get; set; }
        [StringLength(30)]
        public string? Color { get; set; }
    }
}
#nullable disable