
using StoreManagement.Middleware;
using StoreManagement.Services;

namespace StoreManagement
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            // Add services to the container.
            builder.Services.AddScoped<TimeService>();
            builder.Services.AddControllers();
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
            //app.Use((context, next) =>
            //{
            //    DateTime requestTime = DateTime.Now;
            //    var result = next(context);
            //    DateTime responseTime = DateTime.Now;
            //    TimeSpan processTime = requestTime - requestTime;
            //    Console.WriteLine("Process Duration=" + processTime.TotalMicroseconds + "ms" )

            //    return result;
            //});
            app.UseMiddleware<StatusMiddleware>();
            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}