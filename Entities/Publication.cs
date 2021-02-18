using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TeacherWebsiteBackEnd.Entities
{
    public class Publication
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string Publisher { get; set; }
        [Required]
        public string Type { get; set; }
        public string Link { get; set; }
        public Int64 Date { get; set; }
    }
}
