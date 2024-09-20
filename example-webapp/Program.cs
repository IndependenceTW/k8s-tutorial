var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

var app = builder.Build();

app.Use(async (context, next) =>
{
    var logger = app.Services.GetRequiredService<ILogger<Program>>();
    logger.LogInformation("Handling request: {Method} {Url}", context.Request.Method, context.Request.Path);
    await next.Invoke();
    logger.LogInformation("Finished handling request.");
});

app.MapGet("/", () => "Hello World!");

app.MapPost("/write", (string content) =>
{
    return WriteToFile(content);
});

app.MapGet("/write/{content}", (string content) =>
{
    return WriteToFile(content);
});

app.MapGet("/read", () =>
{
    var filePath = Path.Combine("data", "output.txt");
    if (File.Exists(filePath))
    {
        var content = File.ReadAllText(filePath);
        return Results.Ok(content);
    }
    return Results.NotFound("File not found.");
});

app.MapControllers();

IResult WriteToFile(string content)
{
    var filePath = Path.Combine("data", "output.txt");
    File.WriteAllText(filePath, content);
    return Results.Ok("File written successfully.");
}

app.Run();
