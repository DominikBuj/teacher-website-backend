using System.ComponentModel.DataAnnotations;

namespace TeacherWebsiteBackEnd.Entities
{
    public class Text
    {
        [Key]
        [Required]
        public string Name { get; set; }
        [Required]
        public string Value { get; set; }

        public Text(string name, string value)
        {
            Name = name;
            Value = value;
        }
    }
}
