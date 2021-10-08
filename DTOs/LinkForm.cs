using System.ComponentModel.DataAnnotations;

namespace TeacherWebsiteBackEnd.DTOs
{
    public class LinkForm
    {
        [Key]
        public int? Id { get; set; }
        [Required]
        [StringLength(32)]
        public string? Type { get; set; }
        [Required]
        [StringLength(128)]
        public string? TypeName { get; set; }
        [Required]
        [StringLength(2048)]
        public string? Url { get; set; }
        [StringLength(2048)]
        public string? IconUrl { get; set; }
        [StringLength(32)]
        public string? Color { get; set; }
    }
}