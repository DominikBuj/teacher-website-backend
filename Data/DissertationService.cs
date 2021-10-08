using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using TeacherWebsiteBackEnd.Entities;
using TeacherWebsiteBackEnd.Models;
using TeacherWebsiteBackEnd.Helpers;
using TeacherWebsiteBackEnd.DTOs;

namespace TeacherWebsiteBackEnd.Data
{
    public class DissertationService : IDissertationService
    {
        private readonly DatabaseContext _context;
        private readonly IMapper _mapper;

        public DissertationService(DatabaseContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        private Dissertation CreateDissertation(DissertationForm dissertation)
        {
            if (!Enum.TryParse(dissertation.Status, out DissertationStatus dissertationStatus)) return null;

            if (dissertationStatus != DissertationStatus.Completed) dissertation.Date = null;

            Dissertation _dissertation = _mapper.Map<Dissertation>(dissertation);

            return _dissertation;
        }

        public async Task<IEnumerable<DissertationForm>> GetDissertations()
        {
            IEnumerable<Dissertation> dissertations = await _context.Dissertations.ToListAsync();
            return _mapper.Map<IEnumerable<Dissertation>, IEnumerable<DissertationForm>>(dissertations);
        }

        public async Task<DissertationForm> GetDissertationById(int id)
        {
            Dissertation dissertation = await _context.Dissertations.FirstOrDefaultAsync(dissertation => dissertation.Id == id);
            return _mapper.Map<DissertationForm>(dissertation);
        }

        public async Task<DissertationForm> AddDissertation(DissertationForm dissertation)
        {
            Dissertation _dissertation = CreateDissertation(dissertation);
            if (_dissertation == null) return null;

            EntityEntry<Dissertation> __dissertation = await _context.Dissertations.AddAsync(_dissertation);
            await _context.SaveChangesAsync();

            return _mapper.Map<DissertationForm>(__dissertation.Entity);
        }

        public async Task<DissertationForm> ReplaceDissertation(DissertationForm dissertation)
        {
            Dissertation _dissertation = await _context.Dissertations.FirstOrDefaultAsync(__dissertation => __dissertation.Id == dissertation.Id);
            if (_dissertation == null) return await AddDissertation(dissertation);

            Dissertation __dissertation = CreateDissertation(dissertation);
            if (__dissertation == null) return null;
            foreach (PropertyInfo propertyInfo in typeof(Dissertation).GetProperties())
            {
                propertyInfo.SetValue(_dissertation, Functions.ChangeType(propertyInfo.GetValue(__dissertation), propertyInfo.PropertyType), null);
            }
            await _context.SaveChangesAsync();

            return _mapper.Map<DissertationForm>(_dissertation);
        }

        public async void DeleteDissertations()
        {
            _context.Dissertations.RemoveRange(_context.Dissertations);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteDissertationById(int id)
        {
            Dissertation dissertation = await _context.Dissertations.FirstOrDefaultAsync(dissertation => dissertation.Id == id);
            if (dissertation == null) return false;

            _context.Dissertations.Remove(dissertation);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
