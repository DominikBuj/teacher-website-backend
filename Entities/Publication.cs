using System;
using System.ComponentModel.DataAnnotations;
using TeacherWebsiteBackEnd.Helpers;

namespace TeacherWebsiteBackEnd.Entities
{
    public class Publication
    {
        [Key]
        public int? Id { get; set; }
        [Required]
        [StringLength(200)]
        public string? Title { get; set; }
        [StringLength(200)]
        public string? Subtitle { get; set; }
        [StringLength(10)]
        public string? Publisher { get; set; }
        [Required]
        [StringLength(10)]
        public string? Type { get; set; }
        [StringLength(2000)]
        public string? Url { get; set; }
        [PossibleDate]
        public Int64? Date { get; set; }
    }
}
