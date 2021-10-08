using System.Collections.Generic;
using System.Threading.Tasks;
using TeacherWebsiteBackEnd.Models;
using TeacherWebsiteBackEnd.DTOs;

namespace TeacherWebsiteBackEnd.Data
{
    public interface IPublicationService
    {
        Task<IEnumerable<PublicationForm>> GetPublications();
        Task<PublicationForm> GetPublicationById(int id);
        Task<PublicationForm> AddPublication(PublicationForm publication);
        Task<PublicationForm> ReplacePublication(PublicationForm publication);
        void DeletePublications();
        Task<bool> DeletePublicationById(int id);
    }
}
