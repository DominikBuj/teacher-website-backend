using System.Collections.Generic;
using System.Threading.Tasks;
using TeacherWebsiteBackEnd.Entities;
using TeacherWebsiteBackEnd.Models;
using TeacherWebsiteBackEnd.DTOs;

namespace TeacherWebsiteBackEnd.Data
{
    public interface IDissertationService
    {
        Task<IEnumerable<DissertationForm>> GetDissertations();
        Task<DissertationForm> GetDissertationById(int id);
        Task<DissertationForm> AddDissertation(DissertationForm dissertation);
        Task<DissertationForm> ReplaceDissertation(DissertationForm dissertation);
        void DeleteDissertations();
        Task<bool> DeleteDissertationById(int id);
    }
}
