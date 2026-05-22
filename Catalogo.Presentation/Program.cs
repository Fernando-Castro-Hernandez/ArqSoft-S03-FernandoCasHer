using CatalogoApp.Application.Services;
using CatalogoApp.Domain.Interfaces;
using CatalogoApp.Infrastructure.Repositories;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// === SESIÓN ===
// Necesario para recordar al usuario logueado.
builder.Services.AddDistributedMemoryCache();   // almacén en memoria para la sesión
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);  // la sesión expira tras 30 min de inactividad
    options.Cookie.HttpOnly = true;                  // la cookie no es accesible desde JavaScript
    options.Cookie.IsEssential = true;               // la cookie funciona aunque no se acepten cookies opcionales
});

// Necesario para leer la sesión desde las vistas (_Layout)
builder.Services.AddHttpContextAccessor();

// === REPOSITORIOS ===
// Registro del repositorio de items (ya lo tenías)
builder.Services.AddScoped<IItemRepository>(sp =>
    new JsonItemRepository(
        Path.Combine(builder.Environment.ContentRootPath, "Data", "items.json")));

// Registro del repositorio de usuarios (NUEVO)
builder.Services.AddScoped<IUserRepository>(sp =>
    new JsonUserRepository(
        Path.Combine(builder.Environment.ContentRootPath, "Data", "users.json")));

// Registro del repositorio de reviews (NUEVO)
builder.Services.AddScoped<IReviewRepository>(sp =>
    new JsonReviewRepository(
        Path.Combine(builder.Environment.ContentRootPath, "Data", "reviews.json")));

// === SERVICIOS ===
builder.Services.AddScoped<ItemService>();
builder.Services.AddScoped<AuthService>();   // NUEVO
builder.Services.AddScoped<ReviewService>();   // NUEVO

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseSession();        // === NUEVO: activa la sesión en el pipeline ===

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();