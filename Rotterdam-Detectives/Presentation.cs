using Microsoft.Extensions.FileProviders;
using RotterdamDetectives_LogicInterface;
using System.Diagnostics;

namespace RotterdamDetectives_Presentation
{
    public class Presentation
    {
        private IProcessedDataSource _processedDataSource;

        public Presentation(IProcessedDataSource processedDataSource)
        {
            _processedDataSource = processedDataSource;
        }

        public void Start() {
            Directory.SetCurrentDirectory(AppContext.BaseDirectory);

            var builder = WebApplication.CreateBuilder();

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddSingleton(_processedDataSource);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}