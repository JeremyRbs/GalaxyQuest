using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace GalaxyQuest.Services;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        // Configurations des services
    }

    [Obsolete("Obsolete")]
    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            // Configurations pour un environnement de production
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthentication(); // Ajoutez cette ligne pour activer le middleware d'authentification

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
