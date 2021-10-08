using System;
using System.ComponentModel.DataAnnotations;
using TeacherWebsiteBackEnd.Helpers;

namespace TeacherWebsiteBackEnd.DTOs
{
    public class PublicationForm
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
        [StringLength(32)]
        public string? Type { get; set; }
        [Required]
        [StringLength(128)]
        public string? TypeName { get; set; }
        [StringLength(2048)]
        public string? Url { get; set; }
        [PossibleDate]
        public Int64? Date { get; set; }
    }
}
