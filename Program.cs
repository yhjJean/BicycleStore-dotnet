using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using BicycleStore.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddRazorPages()
    .AddRazorRuntimeCompilation();

string cs = "Data Source=YONGH\\SQLEXPRESS;Initial Catalog=bicycleStore;Persist Security Info=True;User ID=sa;Password=12345678";

builder.Services.AddDbContext<BicycleStore.Models.bicycleStoreContext>(Options => Options.UseSqlServer(cs));
builder.Services.AddSession(); // Add session support

var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseSession(); // Use session middleware


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Login}/{id?}");

app.Run();
