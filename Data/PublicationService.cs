using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using TeacherWebsiteBackEnd.Entities;
using TeacherWebsiteBackEnd.Helpers;
using TeacherWebsiteBackEnd.Models;
using TeacherWebsiteBackEnd.DTOs;

namespace TeacherWebsiteBackEnd.Data
{
    public class PublicationService : IPublicationService
    {
        private readonly DatabaseContext _context;
        private readonly IMapper _mapper;

        public PublicationService(DatabaseContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        private Publication CreatePublication(PublicationForm publication)
        {
            if (!Enum.TryParse(publication.Type, out PublicationType publicationType)) return null;

            Publication _publication = _mapper.Map<Publication>(publication);

            return _publication;
        }

        public async Task<IEnumerable<PublicationForm>> GetPublications()
        {
            IEnumerable<Publication> publications = await _context.Publications.ToListAsync();
            return _mapper.Map<IEnumerable<Publication>, IEnumerable<PublicationForm>>(publications);
        }

        public async Task<PublicationForm> GetPublicationById(int id)
        {
            Publication publication = await _context.Publications.FirstOrDefaultAsync(publication => publication.Id == id);
            return _mapper.Map<PublicationForm>(publication);
        }

        public async Task<PublicationForm> AddPublication(PublicationForm publication)
        {
            Publication _publication = CreatePublication(publication);
            if (_publication == null) return null;

            EntityEntry<Publication> __publication = await _context.Publications.AddAsync(_publication);
            await _context.SaveChangesAsync();

            return _mapper.Map<PublicationForm>(__publication.Entity);
        }

        public async Task<PublicationForm> ReplacePublication(PublicationForm publication)
        {
            Publication _publication = await _context.Publications.FirstOrDefaultAsync(__publication => __publication.Id == publication.Id);
            if (_publication == null) return await AddPublication(publication);

            Publication __publication = CreatePublication(publication);
            if (__publication == null) return null;
            foreach (PropertyInfo propertyInfo in typeof(Publication).GetProperties())
            {
                propertyInfo.SetValue(_publication, Functions.ChangeType(propertyInfo.GetValue(__publication), propertyInfo.PropertyType), null);
            }
            await _context.SaveChangesAsync();

            return _mapper.Map<PublicationForm>(_publication);
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
