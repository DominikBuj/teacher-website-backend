using System.ComponentModel.DataAnnotations;

namespace TeacherWebsiteBackEnd.Entities
{
    public class Text
    {
        [Key]
        [Required]
        public string? Name { get; set; }
        [Required(AllowEmptyStrings = true)]
        [StringLength(2048)]
        public string? Value { get; set; }
    }
}
