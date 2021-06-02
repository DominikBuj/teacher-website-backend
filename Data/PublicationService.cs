using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using TeacherWebsiteBackEnd.Entities;
using TeacherWebsiteBackEnd.Helpers;

namespace TeacherWebsiteBackEnd.Data
{
    public class PublicationService : IPublicationService
    {
        private readonly DatabaseContext _context;

        public PublicationService(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Publication>> GetPublications()
        {
            return await _context.Publications.ToListAsync();
        }

        public async Task<Publication> GetPublicationById(int id)
        {
            return await _context.Publications.FirstOrDefaultAsync(publication => publication.Id == id);
        }

        public async Task<Publication> AddPublication(Publication publication)
        {
            EntityEntry<Publication> _publication = await _context.Publications.AddAsync(publication);
            await _context.SaveChangesAsync();

            return _publication.Entity;
        }

        public async Task<Publication> ReplacePublication(Publication publication)
        {
            Publication _publication = await GetPublicationById((int)publication.Id);
            if (_publication == null) return await AddPublication(publication);

            foreach (PropertyInfo propertyInfo in typeof(Publication).GetProperties())
            {
                propertyInfo.SetValue(_publication, Functions.ChangeType(propertyInfo.GetValue(publication), propertyInfo.PropertyType), null);
            }
            await _context.SaveChangesAsync();

            return _publication;
        }

        public async void DeletePublications()
        {
            _context.Publications.RemoveRange(_context.Publications);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeletePublicationById(int id)
        {
            Publication publication = await _context.Publications.FirstOrDefaultAsync(publication => publication.Id == id);
            if (publication == null) return false;

            _context.Publications.Remove(publication);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
