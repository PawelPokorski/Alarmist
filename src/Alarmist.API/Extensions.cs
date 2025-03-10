namespace Alarmist.API;

public static class Extensions
{
    public static IServiceCollection AddApi(this IServiceCollection services)
    {
        services.AddControllersWithViews();

        services.AddSwaggerGen(swagger =>
        {
            swagger.EnableAnnotations();
        });

        return services;
    }

    public static IApplicationBuilder UseApi(this WebApplication app)
    {
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.UseSwagger();
        app.UseSwaggerUI();

        return app;
    }
}