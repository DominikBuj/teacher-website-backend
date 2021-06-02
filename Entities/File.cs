using System.ComponentModel.DataAnnotations;

namespace TeacherWebsiteBackEnd.Entities
{
    public class File
    {

        [Key]
        public int? Id { get; set; }
        [Required]
        [StringLength(100)]
        public string? Name { get; set; }
        [Required]
        [StringLength(2000)]
        public string? Url { get; set; }
        public int? PostId { get; set; }
        public Post? Post { get; set; }
    }
}
