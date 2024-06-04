using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RecordManagementPortalDev.Data;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.Name = "AspNetCore.Identity.Application";
    options.ExpireTimeSpan = TimeSpan.FromMinutes(45);
    options.SlidingExpiration = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
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

//updated by chaw (29/02/2024)
app.Use(async (context, next) =>
{
    context.Response.Headers.Add("X-Frame-Options", "SAMEORIGIN; https://www.google.com");
    await next();
});
//DENY: It completely prevents the page from being called in an iframe.
//SAMEORIGIN: It prevents the page from being called in an iframe outside of its domain.
//ALLOW-FROM uri : Allows calling from a specific url in an iframe.

app.Use(async (context, next) =>
{
    context.Response.Headers.Add("X-Permitted-Cross-Domain-Policies", "none");
    await next();
});
//prevent clients from making cross-site requests by using the X-Permission-Cross-Domain-Policies header.

app.Use(async (context, next) =>
{
    context.Response.Headers.Add("X-Xss-Protection", "1; mode=block");
    await next();
});
//The X-XSS-Protection header causes browsers to stop loading the web page when they detect a cross-site scripting attack.

app.Use(async (context, next) =>
{
    context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
    await next();
});
//It is used to prevent browsers from determining the MIME type sent with the Content Type header in requests sent from the client.

//app.Use(async (ctx, next) =>
//{
//    //ctx.Response.Headers.Add("Content-Security-Policy",
//    //    "default-src 'self' data:* https://fonts.googleapis.com/ https://cdnjs.cloudflare.com/ https://cdn.jsdelivr.net https://fonts.gstatic.com/; script-src 'self' https://localhost:5001/ https://cdnjs.cloudflare.com; style-src 'self' https://cdnjs.cloudflare.com https://cdn.jsdelivr.net https://fonts.googleapis.com/; image-src 'self' data:*"
//    //    );
//    ctx.Response.Headers.Add("Content-Security-Policy",
//        "default-src *; script-src 'self' https://cdnjs.cloudflare.com;"
//        );
//    await next();
//});
//Content-Security-Policy is a security policy used to control data injection attacks that may occur due to a web page’s style and script files.

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
