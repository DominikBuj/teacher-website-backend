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
        [StringLength(256)]
        public string? Topic { get; set; }
        [StringLength(2048)]
        public string? Description { get; set; }
        [Required]
        [Column(TypeName = "varchar(32)")]
        public DissertationStatus? Status { get; set; }
        [PossibleDate]
        public Int64? Date { get; set; }
    }
}
