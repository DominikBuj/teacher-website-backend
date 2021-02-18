using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeacherWebsiteBackEnd.Entities;
using TeacherWebsiteBackEnd.Models;
using TeacherWebsiteBackEnd.Models.Users;

namespace TeacherWebsiteBackEnd.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<RegisterForm, User>();
            CreateMap<RegisterForm, LoginForm>();
        }
    }
}
