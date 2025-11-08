using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using BookLendingApplication.Repositories;
using BookLendingApplication.Services;
using System.Diagnostics.CodeAnalysis;

namespace BookLendingApplication;

[ExcludeFromCodeCoverage]
public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
}

[ExcludeFromCodeCoverage]
public class Startup
{
    public IConfiguration Configuration { get; }

    private IWebHostEnvironment CurrentEnvironment { get; set; }

    public Startup(IConfiguration configuration, IWebHostEnvironment env)
    {
        Configuration = configuration;
        CurrentEnvironment = env ?? throw new ArgumentNullException(nameof(env));
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
            {
                Title = "Book Lending Application",
                Version = "v1",
                Description = "Api to perform book lending operations",
                Contact = new Microsoft.OpenApi.Models.OpenApiContact
                {
                    Name = "Book Lending Application",
                }
            });
        });

        services.AddMemoryCache();

        var builder = WebApplication.CreateBuilder();

        // Use CurrentEnvironment instead of env
        if (CurrentEnvironment.IsDevelopment())
        {
            services.AddScoped<IBookRepository, InMemoryBookRepository>(); // Use this line to use In-Memory repository
        }
        else
        {
            var awsOptions = builder.Configuration.GetAWSOptions();
            services.AddDefaultAWSOptions(awsOptions);
            services.AddAWSService<IAmazonDynamoDB>();
            services.AddScoped<IDynamoDBContext, DynamoDBContext>();
            services.AddScoped<IBookRepository, BookRepository>(); // Use this line to use DynamoDB repository
        }
        services.AddScoped<IBookService, BookService>();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Book Lending Application v1");
                c.RoutePrefix = "swagger";
            });
        }
        else
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Book Lending Application v1");
                c.RoutePrefix = "swagger";
            });
        }

        app.UseRouting();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapGet("/", async context =>
            {
                await context.Response.WriteAsync("Welcome to Book Lending Application");
            });
        });
    }
}
