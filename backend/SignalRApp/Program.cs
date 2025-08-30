
using SignalRApp.Hubs;
using SignalRApp.Services;
using SignalRApp.Services.Interfaces;

namespace SignalRApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddSingleton<IGroupsService, GroupsService>();
            builder.Services.AddSingleton<ITicTacToeService, TicTacToeService>();

            // Add CORS policy for local testing
            builder.Services.AddCors(options => options.AddPolicy("CorsPolicy",
                    builder =>
                    {
                        builder.WithOrigins("http://localhost:3000", "http://127.0.0.1:5500", "null")
                               .AllowAnyHeader()
                               .AllowAnyMethod()
                               .AllowCredentials();
                    }));

            builder.Services.AddSignalR();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            // Enable CORS
            app.UseCors("CorsPolicy");

            app.UseAuthorization();


            app.MapControllers();
            app.MapHub<ChatHub>("/chathub");
            app.MapHub<TicTacToeHub>("/tictactoehub");

            app.Run();
        }
    }
}
