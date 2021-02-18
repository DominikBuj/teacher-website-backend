using System;
using System.ComponentModel.DataAnnotations;

namespace TeacherWebsiteBackEnd.Entities
{
    public class Update
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string Subtitle { get; set; }
        [Required]
        public string Content { get; set; }
        [Required]
        public Int64 Date { get; set; }
    }
}
