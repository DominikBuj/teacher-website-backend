using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TeacherWebsiteBackEnd.Helpers;

namespace TeacherWebsiteBackEnd.Entities
{
    public enum DissertationStatus
    {
        Proposed,
        InProgress,
        Completed
    }

    public class Dissertation
    {
        [Key]
        public int? Id { get; set; }
        [Required]
        [StringLength(200)]
        public string? Topic { get; set; }
        [StringLength(2000)]
        public string? Description { get; set; }
        [Required]
        [Column(TypeName = "varchar(20)")]
        public DissertationStatus? Status { get; set; }
        [PossibleDate]
        public Int64? Date { get; set; }
    }
}
