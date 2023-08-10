namespace Photography
{
    public class RouteConfig
    {
        public static void ConfigureRoutes(IEndpointRouteBuilder app)
        {
            app.MapControllerRoute(
                name: "about",
                pattern: "about",
                defaults: new { controller = "Home", action = "About" });

            app.MapControllerRoute(
                name: "privacy",
                pattern: "privacy",
                defaults: new { controller = "Home", action = "Privacy" });

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
        }
    }
}
