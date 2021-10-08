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
    public class LinkService : ILinkService
    {
        private readonly DatabaseContext _context;
        private readonly IMapper _mapper;

        private readonly Dictionary<LinkType, string> icons = new Dictionary<LinkType, string>
        {
            { LinkType.LinkedIn, "https://localhost:5001/Resources/Images/Icons/linked_in_icon.png" },
            { LinkType.Orcid, "https://localhost:5001/Resources/Images/Icons/orcid_icon.png" },
            { LinkType.ResearchGate, "https://localhost:5001/Resources/Images/Icons/research_gate_icon.png" }
        };

        public LinkService(DatabaseContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        private Link CreateLink(LinkForm link)
        {
            if (!Enum.TryParse(link.Type, out LinkType linkType)) return null;

            if (linkType == LinkType.Other)
            {
                if (!Enum.TryParse(link.Color, out LinkColor linkColor)) return null;
                link.IconUrl = null;
            }
            else
            {
                link.Color = null;
                link.IconUrl = icons[linkType];
            }

            Link _link = _mapper.Map<Link>(link);

            return _link;
        }

        public async Task<IEnumerable<LinkForm>> GetLinks()
        {
            IEnumerable<Link> links = await _context.Links.ToListAsync();
            return _mapper.Map<IEnumerable<Link>, IEnumerable<LinkForm>>(links);
        }

        public async Task<LinkForm> GetLinkById(int id)
        {
            Link link = await _context.Links.FirstOrDefaultAsync(link => link.Id == id);
            return _mapper.Map<LinkForm>(link);
        }

        public async Task<LinkForm> AddLink(LinkForm link)
        {
            Link _link = CreateLink(link);
            if (_link == null) return null;

            EntityEntry<Link> __link = await _context.Links.AddAsync(_link);
            await _context.SaveChangesAsync();

            return _mapper.Map<LinkForm>(__link.Entity);
        }

        public async Task<LinkForm> ReplaceLink(LinkForm link)
        {
            Link _link = await _context.Links.FirstOrDefaultAsync(__link => __link.Id == link.Id);
            if (_link == null) return await AddLink(link);

            Link __link = CreateLink(link);
            if (__link == null) return null;
            foreach (PropertyInfo propertyInfo in typeof(Link).GetProperties())
            {
                propertyInfo.SetValue(_link, Functions.ChangeType(propertyInfo.GetValue(__link), propertyInfo.PropertyType), null);
            }
            await _context.SaveChangesAsync();

            return _mapper.Map<LinkForm>(_link);
        }

        public async void DeleteLinks()
        {
            _context.Links.RemoveRange(_context.Links);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteLinkById(int id)
        {
            Link link = await _context.Links.FirstOrDefaultAsync(_link => _link.Id == id);
            if (link == null) return false;

            _context.Links.Remove(link);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
