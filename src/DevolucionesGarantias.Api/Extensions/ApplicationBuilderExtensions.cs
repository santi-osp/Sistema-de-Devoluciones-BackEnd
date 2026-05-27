namespace DevolucionesGarantias.Api.Extensions;

public static class ApplicationBuilderExtensions
{
    public static WebApplication UseApiPipeline(this WebApplication app)
    {
        app.UseApiSwagger();
        app.UseHttpsRedirection();
        app.UseCors(CorsExtensions.DefaultCorsPolicyName);
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapGet("/", () => Results.Redirect("/swagger"));
        app.MapControllers();

        return app;
    }
}
