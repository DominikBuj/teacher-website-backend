using System.Collections.Generic;
using System.Threading.Tasks;
using TeacherWebsiteBackEnd.Entities;
using TeacherWebsiteBackEnd.Models;
using TeacherWebsiteBackEnd.DTOs;

namespace TeacherWebsiteBackEnd.Data
{
    public interface ILinkService
    {
        Task<IEnumerable<LinkForm>> GetLinks();
        Task<LinkForm> GetLinkById(int id);
        Task<LinkForm> AddLink(LinkForm link);
        Task<LinkForm> ReplaceLink(LinkForm link);
        void DeleteLinks();
        Task<bool> DeleteLinkById(int id);
    }
}
