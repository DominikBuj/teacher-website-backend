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
using TeacherWebsiteBackEnd.DTOs;

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
            // Creates a database context with a postreSQL database.
            services.AddDbContext<DatabaseContext>(options => options.UseNpgsql(_configuration.GetConnectionString("DatabaseConnection")));

            // Creates a custom CORS policy.
            services.AddCors(options => options.AddPolicy("CorsPolicy", builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));

            // Registers the AutoMapper that maps between C# objects.
            services.AddAutoMapper(typeof(Startup));

            services.AddHttpClient();

            // Registers NewtonsoftJson converter that transforms between JSON and C# objects.
            services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });

            // Increases the limit of a request's body for receiving and sending files.
            services.Configure<FormOptions>(options =>
            {
                options.ValueLengthLimit = int.MaxValue;
                options.MultipartBodyLengthLimit = int.MaxValue;
                options.MemoryBufferThreshold = int.MaxValue;
            });

            // Registers the file with app settings.
            IConfigurationSection appSettingsSection = _configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            // Sets up the JWT authentication for the application.
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
                        string? userIdString = context.Principal?.Identity?.Name;
                        if (Int32.TryParse(userIdString, out int userId))
                        {
                            User user = userService.GetUserById(userId).Result;
                            if (user == null) context.Fail("Unauthorized");
                        }
                        else context.Fail("Incorrect user Id");
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

            // Registers dependency injection services.
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITextService, TextService>();
            services.AddScoped<IPostService, PostService>();
            services.AddScoped<IPublicationService, PublicationService>();
            services.AddScoped<ILinkService, LinkService>();
            services.AddScoped<IDissertationService, DissertationService>();
        }

        // Sets up the dummy account of an admin of the site.
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

            // Registers a folder of the application as a place for storing files.
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
