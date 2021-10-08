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
        [Column(TypeName = "varchar(32)")]
        public LinkType? Type { get; set; }
        [Required]
        [StringLength(128)]
        public string? TypeName { get; set; }
        [Required]
        [StringLength(2048)]
        public string? Url { get; set; }
        [StringLength(2048)]
        public string? IconUrl { get; set; }
        [Column(TypeName = "varchar(32)")]
        public LinkColor? Color { get; set; }
    }

}
