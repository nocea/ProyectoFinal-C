using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using ProyectoFinal_C.Utils;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(
    options =>
    {
        options.LoginPath = "/Home/Login";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
    }
    );
//Para que si he cerrado sesiión y le doy a volver a la página anterior no me pueda acceder y que se borre la caché
builder.Services.AddControllersWithViews(
    options =>
    {
        options.Filters.Add(
            new ResponseCacheAttribute
            {
                NoStore = true,
                Location=ResponseCacheLocation.None,
            }
            );
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

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Controlador_Login}/{action=Login}/{id?}");
Utils.Log("Se ha iniciado el proyecto");
app.Run();
