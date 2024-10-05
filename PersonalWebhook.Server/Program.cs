
namespace PersonalWebhook.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            ConfigureServices(builder.Services);

            var app = builder.Build();

            app.UseDefaultFiles();
            app.UseStaticFiles();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();
            app
                //.UseCors("CorsPolicy")
                .UseCors("signalr")
                ;
            app.MapHub<SignalRHub>("/webhookHub");
            app.MapControllers();
            app.MapFallbackToFile("/index.html");

            app.Run();
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddMemoryCache();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IWebhookReceiver, WebhookReceiver>();
            services.AddCors(options =>
                {
                    options.AddPolicy("CorsPolicy",
                        builder => builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        );

                    options.AddPolicy("signalr",
                        builder => builder
                        .AllowAnyMethod()
                        .AllowAnyHeader()

                        .AllowCredentials()
                        .SetIsOriginAllowed(hostName => true));
                });
            services.AddControllers();
            services.AddSignalR();
  
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();

            services.AddSwaggerGen();

            services.AddLogging(options =>
            {
                options.AddSimpleConsole(c => c.TimestampFormat = "[HH:mm:ss]");
            });
        }
    }
}
