using WebWeatherApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Register WeatherService with HttpClient
builder.Services.AddHttpClient<WeatherService>();

// Add session services
builder.Services.AddSession();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Enable session middleware
app.UseSession();

app.UseAuthorization();

// Set default route to WeatherController and Landing action
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Weather}/{action=Landing}/{id?}");

app.Run();
