<<<<<<< HEAD:FastxWebApi-backend/Program.cs
using AutoMapper;
using FastxWebApi.Context;
using FastxWebApi.Interfaces;
using FastxWebApi.Mappings;
using FastxWebApi.Models;
using FastxWebApi.Repository;
using FastxWebApi.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using Route = FastxWebApi.Models.Route;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using FastxWebApi.Filter;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]
namespace FastxWebApi
{

    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            


            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme"
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
            });

            builder.Services.AddCors(options => options.AddPolicy("DefaultCORS", opts =>
            {
                opts.AllowAnyMethod()
                .AllowAnyHeader()
                .AllowAnyOrigin();
            }));


            builder.Logging.AddLog4Net();
            builder.Services.AddAutoMapper(typeof(MappingProfile));
            builder.Services.AddDbContext<ApplicationDbContext>(opts =>
            {
                opts.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            Console.WriteLine($"Using connection string: {connectionString}");

            builder.Services.AddControllers(options =>
            {
                options.Filters.Add(typeof(CustomExceptionFilter));
            });

            // Also, you need to register the filter itself as a service
            builder.Services.AddScoped<CustomExceptionFilter>();

            // Configure JWT Authentication
            #region Authentication
            var jwtKey = builder.Configuration["Tokens:JWT"];
            Console.WriteLine($"JWT key from configuration: {jwtKey}");

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Tokens:JWT"]))
                    };
                });
            #endregion


            #region 
            builder.Services.AddScoped<IAuthenticate, AuthenticationService>();
            builder.Services.AddScoped<IBusRepository, BusRepository>();

            builder.Services.AddScoped<IRouteRepository, RouteRepository>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();

            builder.Services.AddScoped<IBookingRepository, BookingRepository>();

            builder.Services.AddScoped<ISeatRepository, SeatRepository>();
            builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
            builder.Services.AddScoped<IRefundRepository, RefundRepository>();

            
            builder.Services.AddScoped<IRepository<int, Bus>, BusRepository>();

            builder.Services.AddScoped<IRepository<int, Route>, RouteRepository>();
            builder.Services.AddScoped<IRepository<int, User>, UserRepository>();
            builder.Services.AddScoped<IRepository<int, Booking>, BookingRepository>();

            builder.Services.AddScoped<IRepository<int, Seat>, SeatRepository>();

            builder.Services.AddScoped<IRepository<int, Payment>, PaymentRepository>();
            builder.Services.AddScoped<IRepository<int, Refund>, RefundRepository>();
            builder.Services.AddScoped<IRepository<int, Role>, RoleRepository>();
            builder.Services.AddScoped<IRepository<int, BookingSeat>, BookingSeatRepository>();
            #endregion

            #region 
            
            builder.Services.AddScoped<IBusService, BusService>();

            builder.Services.AddScoped<IRouteService, RouteService>();
            builder.Services.AddScoped<IUserService, UserService>();

            builder.Services.AddScoped<IBookingService, BookingService>();

            builder.Services.AddScoped<ISeatService, SeatService>();
            builder.Services.AddScoped<IPaymentService, PaymentService>();
            builder.Services.AddScoped<IRefundService, RefundService>();
            builder.Services.AddScoped<ITokenService, TokenService>();
            #endregion

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseRouting();
            app.UseCors("DefaultCORS");
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
     
    }
=======
using AutoMapper;
using FastxWebApi.Context;
using FastxWebApi.Interfaces;
using FastxWebApi.Mappings;
using FastxWebApi.Models;
using FastxWebApi.Repository;
using FastxWebApi.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using Route = FastxWebApi.Models.Route;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using FastxWebApi.Filter;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]
namespace FastxWebApi
{

    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            


            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme"
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
            });

            builder.Services.AddCors(options => options.AddPolicy("DefaultCORS", opts =>
            {
                opts.AllowAnyMethod()
                .AllowAnyHeader()
                .AllowAnyOrigin();
            }));


            builder.Logging.AddLog4Net();
            builder.Services.AddAutoMapper(typeof(MappingProfile));
            builder.Services.AddDbContext<ApplicationDbContext>(opts =>
            {
                opts.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            Console.WriteLine($"Using connection string: {connectionString}");

            builder.Services.AddControllers(options =>
            {
                options.Filters.Add(typeof(CustomExceptionFilter));
            });

            // Also, you need to register the filter itself as a service
            builder.Services.AddScoped<CustomExceptionFilter>();

            // Configure JWT Authentication
            #region Authentication
            var jwtKey = builder.Configuration["Tokens:JWT"];
            Console.WriteLine($"JWT key from configuration: {jwtKey}");

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Tokens:JWT"]))
                    };
                });
            #endregion


            #region 
            builder.Services.AddScoped<IAuthenticate, AuthenticationService>();
            builder.Services.AddScoped<IBusRepository, BusRepository>();

            builder.Services.AddScoped<IRouteRepository, RouteRepository>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();

            builder.Services.AddScoped<IBookingRepository, BookingRepository>();

            builder.Services.AddScoped<ISeatRepository, SeatRepository>();
            builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
            builder.Services.AddScoped<IRefundRepository, RefundRepository>();

            
            builder.Services.AddScoped<IRepository<int, Bus>, BusRepository>();

            builder.Services.AddScoped<IRepository<int, Route>, RouteRepository>();
            builder.Services.AddScoped<IRepository<int, User>, UserRepository>();
            builder.Services.AddScoped<IRepository<int, Booking>, BookingRepository>();

            builder.Services.AddScoped<IRepository<int, Seat>, SeatRepository>();

            builder.Services.AddScoped<IRepository<int, Payment>, PaymentRepository>();
            builder.Services.AddScoped<IRepository<int, Refund>, RefundRepository>();
            builder.Services.AddScoped<IRepository<int, Role>, RoleRepository>();
            builder.Services.AddScoped<IRepository<int, BookingSeat>, BookingSeatRepository>();
            #endregion

            #region 
            
            builder.Services.AddScoped<IBusService, BusService>();

            builder.Services.AddScoped<IRouteService, RouteService>();
            builder.Services.AddScoped<IUserService, UserService>();

            builder.Services.AddScoped<IBookingService, BookingService>();

            builder.Services.AddScoped<ISeatService, SeatService>();
            builder.Services.AddScoped<IPaymentService, PaymentService>();
            builder.Services.AddScoped<IRefundService, RefundService>();
            builder.Services.AddScoped<ITokenService, TokenService>();
            #endregion

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseRouting();
            app.UseCors("DefaultCORS");
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
     
    }
>>>>>>> e40ecec (initial commit - backend fastx):Program.cs
}