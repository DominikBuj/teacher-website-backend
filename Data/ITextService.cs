using System.Collections.Generic;
using TeacherWebsiteBackEnd.Entities;

namespace TeacherWebsiteBackEnd.Data
{
    public interface ITextService
    {
        IEnumerable<Text> GetTexts();
        Text GetTextByName(string name);
        Text AddText(Text text);
        void DeleteTexts();
        bool DeleteTextByName(string name);
    }
}
