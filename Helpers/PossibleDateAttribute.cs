using System;
using System.ComponentModel.DataAnnotations;

namespace TeacherWebsiteBackEnd.Helpers
{
    public class PossibleDateAttribute : RangeAttribute
    {
        public PossibleDateAttribute() : base(typeof(Int64), "0", DateTimeOffset.Now.ToUnixTimeMilliseconds().ToString()) { }
    }
}
