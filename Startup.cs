using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using TeacherWebsiteBackEnd.Data;
using TeacherWebsiteBackEnd.Entities;
using TeacherWebsiteBackEnd.Models;

namespace TeacherWebsiteBackEnd
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IConfiguration _configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<DatabaseContext>(options => options.UseNpgsql(_configuration.GetConnectionString("DatabaseConnection")));

            // CREATES A CUSTOM CORS POLICY
            services.AddCors(options => options.AddPolicy("CorsPolicy", builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));

            // REGISTERS AUTO MAPPER
            services.AddAutoMapper(typeof(Startup));

            services.AddHttpClient();

            services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });

            services.Configure<FormOptions>(options =>
            {
                options.ValueLengthLimit = int.MaxValue;
                options.MultipartBodyLengthLimit = int.MaxValue;
                options.MemoryBufferThreshold = int.MaxValue;
            });

            IConfigurationSection appSettingsSection = _configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            AppSettings appSettings = appSettingsSection.Get<AppSettings>();
            byte[] key = Encoding.ASCII.GetBytes(appSettings.Secret);
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.Events = new JwtBearerEvents
                {
                    OnTokenValidated = context =>
                    {
                        IUserService userService = context.HttpContext.RequestServices.GetRequiredService<IUserService>();
                        int userId = int.Parse(context.Principal.Identity.Name);
                        User user = userService.GetUserById(userId).Result;
                        if (user == null) context.Fail("Unauthorized");
                        return Task.CompletedTask;
                    }
                };

                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            // REGISTERS DEPENDENCY INJECTION SERVICES
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITextService, TextService>();
            services.AddScoped<IPostService, PostService>();
            services.AddScoped<IPublicationService, PublicationService>();
            services.AddScoped<ILinkService, LinkService>();
            services.AddScoped<IDissertationService, DissertationService>();
        }

        private void SetUpAdmin(IUserService userService, IOptions<AppSettings> settings)
        {
            bool adminExists = userService.UserExistsByUsername(settings.Value.AdminUsername).Result;
            if (adminExists) return;

            RegisterForm adminRegisterForm = new RegisterForm
            {
                Username = settings.Value.AdminUsername,
                Password = settings.Value.AdminPassword,
                Role = Enum.GetName(typeof(UserRole), UserRole.Admin)
            };

            User registeredAdmin = userService.Register(adminRegisterForm).Result;
            if (registeredAdmin == null) Console.WriteLine("Failed to register the admin");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IUserService userService, IMapper mapper, IOptions<AppSettings> settings)
        {
            SetUpAdmin(userService, settings);

            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            app.UseHttpsRedirection();

            app.UseCors("CorsPolicy");

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"Resources")),
                RequestPath = new PathString("/Resources")
            });

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}
