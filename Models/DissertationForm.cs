#nullable enable
using System;
using System.ComponentModel.DataAnnotations;
using TeacherWebsiteBackEnd.Helpers;

namespace TeacherWebsiteBackEnd.Models
{
    public class DissertationForm
    {
        [Key]
        public int? Id { get; set; }
        [Required]
        [StringLength(100)]
        public string? Topic { get; set; }
        [StringLength(1000)]
        public string? Description { get; set; }
        [Required]
        [StringLength(20)]
        public string? Status { get; set; }
        [PossibleDate]
        public Int64? Date { get; set; }
    }
}
#nullable disable