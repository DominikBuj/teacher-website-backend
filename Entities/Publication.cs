using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TeacherWebsiteBackEnd.Helpers;

namespace TeacherWebsiteBackEnd.Entities
{
    public enum PublicationType
    {
        Book,
        Article,
        Dissertation,
        Other
    }

    public class Publication
    {
        [Key]
        public int? Id { get; set; }
        [Required]
        [StringLength(256)]
        public string? Title { get; set; }
        [StringLength(256)]
        public string? Subtitle { get; set; }
        [StringLength(128)]
        public string? Publisher { get; set; }
        [Required]
        [Column(TypeName = "varchar(32)")]
        public PublicationType? Type { get; set; }
        [Required]
        [StringLength(128)]
        public string? TypeName { get; set; }
        [StringLength(2048)]
        public string? Url { get; set; }
        [PossibleDate]
        public Int64? Date { get; set; }
    }
}
