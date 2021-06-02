using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TeacherWebsiteBackEnd.Data;
using TeacherWebsiteBackEnd.Entities;
using TeacherWebsiteBackEnd.Models;

namespace TeacherWebsiteBackEnd.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly IOptions<AppSettings> _settings;

        public AuthController(IUserService userService, IMapper mapper, IOptions<AppSettings> settings)
        {
            _userService = userService;
            _mapper = mapper;
            _settings = settings;
        }

        [HttpPost("login")]
        public async Task<ActionResult<User>> Login([FromBody] LoginForm loginForm)
        {
            User user = await _userService.Login(loginForm);
            if (user == null) return BadRequest();

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            byte[] key = Encoding.ASCII.GetBytes(_settings.Value.Secret);
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.Role.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);

            return Ok(user);
        }

        [HttpPost("register")]
        public async Task<ActionResult<User>> Register([FromBody] RegisterForm registerForm)
        {
            User registeredUser = await _userService.Register(registerForm);
            if (registeredUser == null) return BadRequest();

            LoginForm loginForm = _mapper.Map<LoginForm>(registerForm);

            return await Login(loginForm);
        }
    }
}
