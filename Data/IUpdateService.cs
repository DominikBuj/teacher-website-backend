using System.Collections.Generic;
using TeacherWebsiteBackEnd.Entities;

namespace TeacherWebsiteBackEnd.Data
{
    public interface IUpdateService
    {
        IEnumerable<Update> GetUpdates();
        Update GetUpdateById(int id);
        Update AddUpdate(Update update);
        Update ReplaceUpdate(Update update);
        void DeleteUpdates();
        bool DeleteUpdateById(int id);
    }
}
