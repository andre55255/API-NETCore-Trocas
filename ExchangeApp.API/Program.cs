using ExchangeApp.API.Extensions;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Config Controllers
builder.Services.AddControllers();

// Config contexts db
builder.Services.AddDbContext(builder.Configuration);

// Config identity and jwt
builder.Services.AddAuthJwt(builder.Configuration);
builder.Services.AddIdentity();

// Config cors
builder.Services.AddCors(builder.Configuration);

// Config repos
builder.Services.AddRepositories();

// Config services
builder.Services.AddServices();

// Config swagger
builder.Services.AddSwaggerGenerationApp();

// Config automapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Config endpoints
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
}
// Add Swagger page
app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint($"/swagger/v1/swagger.json", "ExchangeApp"));

// Add http context


// Add static files
app.UseStaticFiles();
app.UseStaticFiles(new StaticFileOptions()
{
    FileProvider = new PhysicalFileProvider(
    Path.Combine(Directory.GetCurrentDirectory(), @"Directory")),
    RequestPath = new PathString("/static")
});
app.UseDirectoryBrowser(new DirectoryBrowserOptions()
{
    FileProvider = new PhysicalFileProvider(
    Path.Combine(Directory.GetCurrentDirectory(), @"Directory")),
    RequestPath = new PathString("/static")
});

app.UseHttpContext();

app.UseCors("ClientPermission");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
