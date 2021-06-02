using System.Collections.Generic;
using System.Threading.Tasks;
using TeacherWebsiteBackEnd.Entities;

namespace TeacherWebsiteBackEnd.Data
{
    public interface ITextService
    {
        Task<IEnumerable<Text>> GetTexts();
        Task<Text> GetTextByName(string name);
        Task<Text> AddText(Text text);
        Task<Text> ReplaceText(Text text);
        void DeleteTexts();
        Task<bool> DeleteTextByName(string name);
    }
}
