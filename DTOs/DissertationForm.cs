using System;
using System.ComponentModel.DataAnnotations;
using TeacherWebsiteBackEnd.Helpers;

namespace TeacherWebsiteBackEnd.DTOs
{
    public class DissertationForm
    {
        [Key]
        public int? Id { get; set; }
        [Required]
        [StringLength(256)]
        public string? Topic { get; set; }
        [StringLength(2048)]
        public string? Description { get; set; }
        [Required]
        [StringLength(32)]
        public string? Status { get; set; }
        [PossibleDate]
        public Int64? Date { get; set; }
    }
}