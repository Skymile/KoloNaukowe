var app = WebApplication.Create(args);
app.UseExceptionHandler("/error");
app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast"        , () => GetWeatherForecast(5)             );
app.MapGet("/weatherforecast/{count}", (int count) => GetWeatherForecast(count));
app.MapGet("/error"                  , () => "B³¹d!"                           );
app.MapGet("/"                       , () => "Strona g³ówna!"                  );

app.Run();

IEnumerable<WeatherForecast> GetWeatherForecast(int count) =>
    Enumerable
        .Range(1, count)
        .Select(index =>
           new WeatherForecast
           (
               DateTime.Now.AddDays(index),
               Random.Shared.Next(-20, 55),
               summaries?[Random.Shared.Next(summaries.Length)]
           ));

internal record WeatherForecast(DateTime Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
