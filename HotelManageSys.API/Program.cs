
using HotelManageSys.API.Features.Rooms.Providers;
using HotelManageSys.API.Features.Rooms.Services;
using HotelManageSys.API.Features.Amenities.Providers;
using HotelManageSys.API.Features.Amenities.Services;
using HotelManageSys.API.Features.AdditionalOffers.Providers;
using HotelManageSys.API.Features.AdditionalOffers.Services;
using HotelManageSys.API.Features.RoomTypes.Providers;
using HotelManageSys.API.Features.RoomTypes.Services;
using HotelManageSys.API.Features.Guests.Providers;
using HotelManageSys.API.Features.Guests.Services;
using HotelManageSys.API.Features.Workers.Providers;
using HotelManageSys.API.Features.Workers.Services;
using HotelManageSys.API.Features.Payments.Providers;
using HotelManageSys.API.Features.Payments.Services;
using HotelManageSys.API.Features.Reservations.Providers;
using HotelManageSys.API.Features.Reservations.Services;
using HotelManageSys.API.Models.Data;
using Mapster;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using MediatR;

using HotelManageSys.API.Behaviors;
using HotelManageSys.API.Features.Auth.Services;
using HotelManageSys.API.Middleware;
using Microsoft.IdentityModel.Tokens;

namespace HotelManageSys.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
               options.UseSqlServer(builder.Configuration.GetConnectionString("ApplicationDbContext")));

            builder.Services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());

                cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
                cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
                
            });


            builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            builder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });


            builder.Services.AddAuthorization();
            
            builder.Services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ClockSkew = TimeSpan.Zero,
                        ValidIssuer = builder.Configuration["JwtConfig:Issuer"],
                        ValidAudience = builder.Configuration["JwtConfig:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtConfig:SecretKey"])),
                        
                    };

                });

            TypeAdapterConfig.GlobalSettings.Scan(Assembly.GetExecutingAssembly());

            RegisterProviders(builder);

            RegisterServices(builder);

            SetUpCORS(builder);

            builder.Services.AddControllers().AddJsonOptions(x =>
            {
                x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

            builder.Services.AddOpenApi();

            var app = builder.Build();

            app.UseMiddleware<ExceptionMiddleware>();
            
            ConfigureDevelopment(app);
            
            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();
            
            app.Run();
        }

        private static void RegisterProviders(WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IRoomProvider, RoomProvider>();
            builder.Services.AddScoped<IRoomTypeProvider, RoomTypeProvider>();
            builder.Services.AddScoped<IAmenityProvider, AmenityProvider>();
            builder.Services.AddScoped<IAdditionalOfferProvider, AdditionalOfferProvider>();
            builder.Services.AddScoped<IGuestProvider, GuestProvider>();
            builder.Services.AddScoped<IWorkerProvider, WorkerProvider>();
            builder.Services.AddScoped<IPaymentProvider, PaymentProvider>();
            builder.Services.AddScoped<IReservationProvider, ReservationProvider>();

        }


        private static void RegisterServices(WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IRoomService, RoomService>();
            builder.Services.AddScoped<IRoomTypeService, RoomTypeService>();
            builder.Services.AddScoped<IAmenityService, AmenityService>();
            builder.Services.AddScoped<IAdditionalOfferService, AdditionalOfferService>();
            builder.Services.AddScoped<IGuestService, GuestService>();
            builder.Services.AddScoped<IWorkerService, WorkerService>();
            builder.Services.AddScoped<IPaymentService, PaymentService>();
            builder.Services.AddScoped<IReservationService, ReservationService>();
            builder.Services.AddScoped<IJwtProvider, JwtProvider>();
            
        }

        private static void SetUpCORS(WebApplicationBuilder builder)
        {
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    policy => policy
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
            });
        }

        private static void ConfigureDevelopment(WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {

                using (var scope = app.Services.CreateScope())
                {
                    try
                    {
                        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                        dbContext.Database.Migrate();
                    }
                    catch (Exception ex)
                    {
                        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
                        logger.LogError(ex, "Błąd podczas migracji bazy danych");
                    }
                }

                app.UseCors("AllowAll");

                app.MapOpenApi();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/openapi/v1.json", "v1");
                });

               
            }
        }
    }
}
