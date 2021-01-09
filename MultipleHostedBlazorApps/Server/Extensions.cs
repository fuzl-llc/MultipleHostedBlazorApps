using Microsoft.AspNetCore.Builder;

namespace MultipleHostedBlazorApps.Server
{
    public static class Extensions
    {
        public static IApplicationBuilder MapBlazorApp(this IApplicationBuilder app, string path)
        {
            app.MapWhen(context => context.Request.Path.Value != null && (context.Request.Path.Value.StartsWith($"/{path}/") || context.Request.Path.Value == $"/{path}"), first =>
            {
                first.UseStaticFiles();
                first.UseBlazorFrameworkFiles($"/{path}");
                first.UseStaticFiles($"/{path}");
                first.UseRouting();

                first.UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                    endpoints.MapFallbackToFile($"/{path}/{{*path:nonfile}}", $"{path}/index.html");
                    // TODO: Brent: Pri1: this uses *path but also worked in testing when using **slug - which is better?
                });
            });

            return app;
        }
    }
}
