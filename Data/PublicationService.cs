using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TeacherWebsiteBackEnd.Entities;

namespace TeacherWebsiteBackEnd.Data
{
    public class PublicationService : IPublicationService
    {
        private readonly DatabaseContext _context;

        public PublicationService(DatabaseContext context)
        {
            _context = context;
        }

        public IEnumerable<Publication> GetPublications()
        {
            return _context.Publications.ToList();
        }

        public Publication GetPublicationById(int id)
        {
            return _context.Publications.FirstOrDefault(publication => publication.Id == id);
        }

        public Publication AddPublication(Publication publication)
        {
            _context.Publications.Add(publication);
            _context.SaveChanges();

            return publication;
        }

        public Publication ReplacePublication(Publication publication)
        {
            Publication _publication = _context.Publications.FirstOrDefault(__publication => __publication.Id == publication.Id);

            if (_publication == null) return null;

            foreach (PropertyInfo propertyInfo in typeof(Publication).GetProperties())
            {
                var value = propertyInfo.GetValue(publication, null);
                propertyInfo.SetValue(_publication, Convert.ChangeType(value, propertyInfo.PropertyType));
            }
            _context.SaveChanges();

            return _publication;
        }

        public void DeletePublications()
        {
            _context.Publications.RemoveRange(_context.Publications);
            _context.SaveChanges();
        }

        public bool DeletePublicationById(int id)
        {
            Publication publication = _context.Publications.FirstOrDefault(publication => publication.Id == id);

            if (publication == null) return false;

            _context.Publications.Remove(publication);
            _context.SaveChanges();

            return true;
        }
    }
}
