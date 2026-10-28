namespace MyApi.Data;

public static class DatabaseInitializer
{
    // The database may accept connections a few seconds after the API starts,
    // so retry instead of crashing on the first failed connection.
    public static async Task WaitForDatabaseAsync(WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDb>();

        for (var attempt = 1; ; attempt++)
        {
            try
            {
                await db.Database.EnsureCreatedAsync();
                return;
            }
            catch (Exception ex) when (attempt < 10)
            {
                app.Logger.LogWarning("Database not ready (attempt {Attempt}): {Message}", attempt, ex.Message);
                await Task.Delay(TimeSpan.FromSeconds(2));
            }
        }
    }
}