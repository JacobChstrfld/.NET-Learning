using Microsoft.AspNetCore.Builder;

namespace MyStore
{
    public class Startup
    {
       
        public void ConfigureServices(IServiceCollection services)
        {
            // Add session state to the application
            services.AddSession();

            // Other service configurations
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
            app.UseSession();
        }

    }
}
