using System.Collections.Generic;
using TeacherWebsiteBackEnd.Entities;

namespace TeacherWebsiteBackEnd.Data
{
    public interface IPublicationService
    {
        IEnumerable<Publication> GetPublications();
        Publication GetPublicationById(int id);
        Publication AddPublication(Publication publication);
        Publication ReplacePublication(Publication publication);
        void DeletePublications();
        bool DeletePublicationById(int id);
    }
}
