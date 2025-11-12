
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Laboratorio152
{
    public class Startup
    {
        
        public void ConfigureServices(IServiceCollection services)
        {
            
            services.AddRouting();
        }

        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello World!");
                });
            });
        }
    }
}