using Microsoft.Extensions.FileProviders;
using Serilog;
using VideoCatalogue.CustomMiddleware;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.Configure<RequestSizeLimitOptions>(options =>
{
    options.MaxRequestBodySize = 209715200; // 200 MB
    options.PathToLimit = "/api/Fileupload";
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseStaticFiles(new StaticFileOptions
{

    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "MediaFiles")),
    RequestPath = "/media"
});

app.UseMiddleware<RequestSizeLimitMiddleware>(); // 200 MB
app.UseRouting();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
