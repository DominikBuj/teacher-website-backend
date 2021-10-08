#nullable enable
using System.ComponentModel.DataAnnotations;

namespace TeacherWebsiteBackEnd.Models
{
    public class Details
    {
        [StringLength(2048)]
        public string? lightBackgroundUrl { get; set; }
        [StringLength(2048)]
        public string? darkBackgroundUrl { get; set; }
    }
}
#nullable disable