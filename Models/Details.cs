#nullable enable
using System.ComponentModel.DataAnnotations;

namespace TeacherWebsiteBackEnd.Models
{
    public class Details
    {
        [StringLength(100)]
        public string? lightBackgroundUrl { get; set; }
        [StringLength(100)]
        public string? darkBackgroundUrl { get; set; }
    }
}
#nullable disable