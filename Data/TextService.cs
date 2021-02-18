using System.Collections.Generic;
using System.Linq;
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

        public IEnumerable<Text> GetTexts()
        {
            return _context.Texts.ToList();
        }

        public Text GetTextByName(string name)
        {
            return _context.Texts.FirstOrDefault(text => text.Name == name);
        }

        public Text AddText(Text text)
        {
            Text _text = GetTextByName(text.Name);

            if (_text != null)
            {
                _text.Value = text.Value;
                _context.SaveChanges();
                return null;
            }

            _context.Texts.Add(text);
            _context.SaveChanges();

            return text;
        }

        public void DeleteTexts()
        {
            _context.Texts.RemoveRange(_context.Texts);
            _context.SaveChanges();
        }

        public bool DeleteTextByName(string name)
        {
            Text text = _context.Texts.FirstOrDefault(text => text.Name == name);

            if (text == null) return false;

            _context.Texts.Remove(text);
            _context.SaveChanges();

            return true;
        }
    }
}
