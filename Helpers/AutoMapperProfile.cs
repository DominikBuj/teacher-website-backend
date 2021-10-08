using AutoMapper;
using TeacherWebsiteBackEnd.Entities;
using TeacherWebsiteBackEnd.Models;
using TeacherWebsiteBackEnd.DTOs;

namespace TeacherWebsiteBackEnd.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<RegisterForm, User>();
            CreateMap<RegisterForm, LoginForm>();
            CreateMap<DissertationForm, Dissertation>();
            CreateMap<Dissertation, DissertationForm>();
            CreateMap<LinkForm, Link>();
            CreateMap<Link, LinkForm>();
            CreateMap<PublicationForm, Publication>();
            CreateMap<Publication, PublicationForm>();
        }
    }
}
