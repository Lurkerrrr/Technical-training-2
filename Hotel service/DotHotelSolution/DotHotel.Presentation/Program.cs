using DotHotel.Presentation.Middlewares;
using DotHotel.Presentation.StartupExtensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.ConfigureApplicationIdentity(builder.Configuration);
builder.Services.ConfigureServices();
builder.Services.ConfigureRepositories();
builder.Services.ConfigureDomainEvents();
builder.Services.ConfigureBackgroundServices();

var app = builder.Build();

app.UseExceptionHandlingMiddleware();

app.UseHsts();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
