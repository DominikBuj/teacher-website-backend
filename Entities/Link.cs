using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeacherWebsiteBackEnd.Entities
{
    public enum LinkType
    {
        Orcid,
        LinkedIn,
        ResearchGate,
        Other
    }

    public enum LinkColor
    {
        lightpink,
        lightcoral,
        lightcyan,
        lightgoldenrodyellow
    }

    public class Link
    {
        [Key]
        public int? Id { get; set; }
        [Required]
        [Column(TypeName = "varchar(20)")]
        public LinkType? Type { get; set; }
        [Required]
        [StringLength(200)]
        public string? Name { get; set; }
        [Required]
        [StringLength(2000)]
        public string? Url { get; set; }
        public string? IconUrl { get; set; }
        [Column(TypeName = "varchar(30)")]
        public LinkColor? Color { get; set; }
    }
}
