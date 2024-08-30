using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using PaymentGateway.Service.IService;
using PaymentGateway.Service.Service;
using System.Text;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.Cookies;
using PaymentGateway.Repository.IRepository;
using PaymentGateway.Repository.Repository;
using Microsoft.EntityFrameworkCore;
using NLog.Web;
using NLog;
using PaymentGateway.Models;
using Stripe;

namespace PaymentGateway
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
            logger.Debug("init main");

            try
            {
                var builder = WebApplication.CreateBuilder(args);
                builder.Logging.ClearProviders();
                builder.Host.UseNLog();
                // Add services to the container.
                builder.Services.AddControllers();
                builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
                builder.Services.AddScoped<IUserService, UserService>();
                builder.Services.AddScoped<IUserRepository, UserRepository>();
                builder.Services.AddScoped<ITransactionService, TransactionService>();
                builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();    
                builder.Services.AddScoped<IPayPalService>(provider =>
                {
                    var configuration = provider.GetRequiredService<IConfiguration>();
                    var clientId = configuration["PayPal:ClientId"];
                    var clientSecret = configuration["PayPal:ClientSecret"];
                    var isSandbox = configuration.GetValue<bool>("PayPal:IsSandbox");

                    return new PayPalService(clientId, clientSecret, isSandbox);
                });
                builder.Services.AddScoped<IStripeService>(sp => new StripeService(builder.Configuration["Stripe:SecretKey"]));

                // Configure Authentication (JWT and Google)
                builder.Services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
                        ValidAudience = builder.Configuration["JwtSettings:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"]))
                    };
                })
                .AddGoogle(options =>
                {
                    options.ClientId = builder.Configuration["GoogleAuthSettings:ClientId"];
                    options.ClientSecret = builder.Configuration["GoogleAuthSettings:ClientSecret"];
                });

                builder.Services.AddAuthentication().AddFacebook(options =>
                {
                    options.AppId = builder.Configuration["FacebookAuthSettings:AppID"];
                    options.AppSecret = builder.Configuration["FacebookAuthSettings:AppSecret"];
                });

                builder.Services.AddDbContext<SDirectContext>(options =>

               options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));




                // Add CORS policy
                builder.Services.AddCors(options =>
                {
                    options.AddPolicy("AllowAll", builder =>
                    {
                        builder.AllowAnyOrigin()
                               .AllowAnyMethod()
                               .AllowAnyHeader();
                    });
                });

                // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
                builder.Services.AddEndpointsApiExplorer();
                builder.Services.AddSwaggerGen();

                var app = builder.Build();

                // Configure the HTTP request pipeline.
                if (app.Environment.IsDevelopment())
                {
                    app.UseSwagger();
                    app.UseSwaggerUI();
                }

                app.UseHttpsRedirection();

                // Use CORS policy
                app.UseCors("AllowAll");

                app.UseAuthentication(); // Ensure this comes before UseAuthorization

                app.UseAuthorization();

                // Global error handling
                //.UseExceptionHandler("/error");

                app.MapControllers();

                app.Run();
            }
            catch (Exception ex) {
                logger.Error(ex, "Stopped program because of exception");
                throw;
                   }
            finally
            {
                NLog.LogManager.Shutdown();
            }
        }
    }
}
