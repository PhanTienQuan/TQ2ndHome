using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TQuanHome.Areas.Identity.Data;
using TQuanHome.Data;
var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("TQuanHomeContextConnection") ?? throw new InvalidOperationException("Connection string 'TQuanHomeContextConnection' not found.");

builder.Services.AddDbContext<TQuanHomeContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<TQuanHomeUser>(options => options.SignIn.RequireConfirmedAccount = false).AddEntityFrameworkStores<TQuanHomeContext>();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
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

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();
app.Run();
