using System.Collections.Generic;
using System.Threading.Tasks;
using TeacherWebsiteBackEnd.Entities;

namespace TeacherWebsiteBackEnd.Data
{
    public interface IPublicationService
    {
        Task<IEnumerable<Publication>> GetPublications();
        Task<Publication> GetPublicationById(int id);
        Task<Publication> AddPublication(Publication publication);
        Task<Publication> ReplacePublication(Publication publication);
        void DeletePublications();
        Task<bool> DeletePublicationById(int id);
    }
}
