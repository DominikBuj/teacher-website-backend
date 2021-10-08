using System.ComponentModel.DataAnnotations;

namespace TeacherWebsiteBackEnd.Entities
{
    public class File
    {

        [Key]
        public int? Id { get; set; }
        [Required]
        [StringLength(128)]
        public string? Name { get; set; }
        [Required]
        [StringLength(2048)]
        public string? Url { get; set; }
        public int? PostId { get; set; }
        public Post? Post { get; set; }
    }
}
