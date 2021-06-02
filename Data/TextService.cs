using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using TeacherWebsiteBackEnd.Entities;

namespace TeacherWebsiteBackEnd.Data
{
    public class TextService : ITextService
    {
        private readonly DatabaseContext _context;

        public TextService(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Text>> GetTexts()
        {
            return await _context.Texts.ToListAsync();
        }

        public async Task<Text> GetTextByName(string name)
        {
            return await _context.Texts.FirstOrDefaultAsync(text => text.Name == name);
        }

        public async Task<Text> AddText(Text text)
        {
            EntityEntry<Text> _text = await _context.Texts.AddAsync(text);
            await _context.SaveChangesAsync();

            return _text.Entity;
        }

        public async Task<Text> ReplaceText(Text text)
        {
            Text _text = await GetTextByName(text.Name);
            if (_text == null) return await AddText(text);

            _text.Value = text.Value;
            await _context.SaveChangesAsync();

            return _text;
        }

        public async void DeleteTexts()
        {
            _context.Texts.RemoveRange(_context.Texts);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteTextByName(string name)
        {
            Text text = await GetTextByName(name);
            if (text == null) return false;

            _context.Texts.Remove(text);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
