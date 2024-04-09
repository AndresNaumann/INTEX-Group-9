using BrickwellStore.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDbContext<BrickwellContext>(options =>
    options.UseSqlite(connectionString));

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<ILegoRepository, EFLegoRepository>();

builder.Services.AddRazorPages();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

var services = builder.Services;
var configuration = builder.Configuration;

services.AddAuthentication().AddGoogle(googleOptions =>
{
    //googleOptions.ClientId = configuration["Authentication:Google:ClientId"];
    //googleOptions.ClientSecret = configuration["Authentication:Google:ClientSecret"];
    googleOptions.ClientId = "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";
    googleOptions.ClientSecret = "246075900303-55r8n0gt13g82j59h4nu9i0qvnlk0m0a.apps.googleusercontent.com";
}).AddFacebook(facebookOptions =>
{
    //facebookOptions.AppId = configuration["Authentication:Facebook:AppId"];
    //facebookOptions.AppSecret = configuration["Authentication:Facebook:AppSecret"];
    facebookOptions.AppId = "946477026888276";
    facebookOptions.AppSecret = "6dcddd58604753eea3c5e24080ffee0d";
});

var app = builder.Build();

//Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();


app.UseSession();

app.UseRouting();

app.UseAuthorization();

//app.MapControllerRoute("pagenumandcolor", "{productColor}/{pageNum}", new { Controller = "Home", action = "Product" });
//app.MapControllerRoute("pagination", "{pageNum}", new { Controller = "Home", action = "Product", pageNum = 1 });
//app.MapControllerRoute("productColor", "{productColor}", new { Controller = "Home", action = "Product", pageNum = 1 });

app.MapDefaultControllerRoute();
app.MapRazorPages();

app.Run();
