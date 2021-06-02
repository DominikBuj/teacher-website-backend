using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TeacherWebsiteBackEnd.Entities
{
    public class Post
    {
        [Key]
        public int? Id { get; set; }
        [Required]
        [StringLength(200)]
        public string? Title { get; set; }
        [StringLength(200)]
        public string? Subtitle { get; set; }
        [Required]
        [StringLength(4000)]
        public string? Content { get; set; }
        [Required]
        public Int64? Date { get; set; }
        public IList<File>? Files { get; set; }
    }
}
