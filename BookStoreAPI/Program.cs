using Microsoft.EntityFrameworkCore;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

        // ApplicationDbContext'i DI konteynerine ekle
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString)); // SQL Server kullanıyoruz

        builder.Services.AddControllersWithViews();

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowLocalhost3000",
                builder => builder
                    .WithOrigins("http://localhost:3000") // Sadece bu URL'ye izin veriyoruz
                    .AllowAnyMethod()
                    .AllowAnyHeader());
        });


        // Add services to the container.

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

        app.UseHttpsRedirection();

        app.UseCors("AllowAllOrigins");
        app.UseCors("AllowLocalhost3000"); // Belirttiğimiz CORS politikasını uyguluyoruz

        app.UseAuthorization();

        app.UseRouting();

        app.MapControllers();

        app.Run();
    }
    

}