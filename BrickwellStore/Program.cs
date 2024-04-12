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

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<ILegoRepository, EFLegoRepository>();

builder.Services.AddRazorPages();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

builder.Services.AddScoped<Cart>(sp => SessionCart.GetCart(sp));
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

var services = builder.Services;
var configuration = builder.Configuration;

services.AddAuthentication().AddGoogle(googleOptions =>
{
    googleOptions.ClientId = "246075900303-55r8n0gt13g82j59h4nu9i0qvnlk0m0a.apps.googleusercontent.com";
    googleOptions.ClientSecret = "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";
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

app.Use(async (ctx, next) =>
{
    var csp = "default-src 'self'; " +
         "script-src 'self' https://kit.fontawesome.com 'unsafe-inline' https://apis.google.com; " +
         "connect-src 'self' http://localhost:5255/ https://ka-f.fontawesome.com; " +
         "style-src 'self' https://cdnjs.cloudflare.com/ https://fonts.gstatic.com https://fonts.googleapis.com/ 'unsafe-inline'; " +
         "font-src 'self' https://fonts.gstatic.com https://fonts.googleapis.com/ https://ka-f.fontawesome.com; " +
         "img-src 'self' https://m.media-amazon.com https://www.lego.com/ https://images.brickset.com/ https://www.brickeconomy.com/ https://live.staticflickr.com  data:; ";
    ctx.Response.Headers.Append("Content-Security-Policy", csp);
    ctx.Response.Headers.Append("X-Content-Type-Options", "nosniff");
    await next();
});

app.UseAuthentication();
app.UseAuthorization();

//app.MapControllerRoute("pagenumandcolor", "{productColor}/{pageNum}", new { Controller = "Home", action = "Product" });
app.MapControllerRoute("productPagination", "Product/{pageNum}", new { Controller = "Home", action = "Product", pageNum = 1 });
app.MapControllerRoute("productPagination", "Product/{pageNum}", new { Controller = "Home", action = "Product", pageNum = 1 });

//app.MapControllerRoute("productColor", "{productColor}", new { Controller = "Home", action = "Product", pageNum = 1 });

app.MapDefaultControllerRoute();
app.MapRazorPages();

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var roles = new[] { "Admin", "Member" };

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
            await roleManager.CreateAsync(new IdentityRole(role));
    }
}

using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

    string email = "naumannaadn@gmail.com";
    string password = "Test1234!";

    if (await userManager.FindByEmailAsync(email) == null)
    {
        var user = new IdentityUser();
        user.UserName = email;
        user.Email = email;

        await userManager.CreateAsync(user, password);

        await userManager.AddToRoleAsync(user, "Admin");
    }
}

app.Run();
