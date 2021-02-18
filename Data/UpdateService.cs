using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TeacherWebsiteBackEnd.Entities;

namespace TeacherWebsiteBackEnd.Data
{
    public class UpdateService : IUpdateService
    {
        private readonly DatabaseContext _context;

        public UpdateService(DatabaseContext context)
        {
            _context = context;
        }

        public IEnumerable<Update> GetUpdates()
        {
            return _context.Updates.ToList();
        }

        public Update GetUpdateById(int id)
        {
            return _context.Updates.FirstOrDefault(update => update.Id == id);
        }

        public Update AddUpdate(Update update)
        {
            _context.Updates.Add(update);
            _context.SaveChanges();

            return update;
        }

        public Update ReplaceUpdate(Update update)
        {
            Update _update = _context.Updates.FirstOrDefault(__update => __update.Id == update.Id);

            if (_update == null) return null;

            foreach (PropertyInfo propertyInfo in typeof(Update).GetProperties())
            {
                var value = propertyInfo.GetValue(update, null);
                propertyInfo.SetValue(_update, Convert.ChangeType(value, propertyInfo.PropertyType));
            }
            _context.SaveChanges();

            return _update;
        }

        public void DeleteUpdates()
        {
            _context.Updates.RemoveRange(_context.Updates);
            _context.SaveChanges();
        }

        public bool DeleteUpdateById(int id)
        {
            Update update = _context.Updates.FirstOrDefault(update => update.Id == id);

            if (update == null) return false;

            _context.Updates.Remove(update);
            _context.SaveChanges();

            return true;
        }
    }
}
