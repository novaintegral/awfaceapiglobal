using System.Net;
using AwFace.Api.Certiface;
using AwFace.Api.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddOptions<CertifaceOptions>()
    .Bind(builder.Configuration.GetSection(CertifaceOptions.SectionName))
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services
    .AddHttpClient<ICertifaceProxy, CertifaceProxy>((serviceProvider, client) =>
    {
        var options = serviceProvider
            .GetRequiredService<Microsoft.Extensions.Options.IOptions<CertifaceOptions>>()
            .Value;

        client.BaseAddress = options.BaseUrl;
        client.Timeout = TimeSpan.FromSeconds(options.TimeoutSeconds);
    })
    .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
    {
        AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate | DecompressionMethods.Brotli
    });

builder.Services
    .AddHttpClient<ICertifaceClient, CertifaceClient>((serviceProvider, client) =>
    {
        var options = serviceProvider
            .GetRequiredService<Microsoft.Extensions.Options.IOptions<CertifaceOptions>>()
            .Value;

        client.BaseAddress = options.BaseUrl;
        client.Timeout = TimeSpan.FromSeconds(options.TimeoutSeconds);
    })
    .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
    {
        AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate | DecompressionMethods.Brotli
    });

builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AngularDev", policy =>
    {
        policy
            .WithOrigins("http://localhost:4200", "https://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseHttpsRedirection();
app.UseCors("AngularDev");

app.MapGet("/health", () => Results.Ok(new
{
    status = "Healthy",
    service = "AwFace.Api",
    utc = DateTimeOffset.UtcNow
}));

app.MapControllers();

app.Run();
