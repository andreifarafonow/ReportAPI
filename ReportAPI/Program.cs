using FluentValidation;
using Microsoft.EntityFrameworkCore;
using ReportAPI.Models;
using ReportAPI.Models.Validation;

namespace ReportAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(x => x.EnableAnnotations());
            builder.Services.AddDbContext<ReportApiDbContext>(options => 
                options.UseNpgsql(builder.Configuration.GetConnectionString("PgConnection")));
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            builder.Services.AddValidatorsFromAssemblyContaining<UserDtoValidator>();


            var app = builder.Build();


            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}