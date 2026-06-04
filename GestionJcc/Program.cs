/*using GestionJcc.Datos;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
//area de servicios
builder.Services.AddControllers();

builder.Services.AddDbContext<ApplicationDbContext>
    (opciones => opciones.UseSqlServer("name=DefaultConnection"));

builder.Services.AddSwaggerGen();

var app = builder.Build();

//area de middlewares
app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();
app.Run();*/
using GestionJcc.Datos;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// área de servicios
builder.Services.AddControllers();
builder.Services.AddDbContext<ApplicationDbContext>(
    opciones => opciones.UseSqlServer("name=DefaultConnection"));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
});
builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirReact", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// área de middlewares
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "GestionJcc v1");
    });
}

app.UseStaticFiles();  
app.UseCors("PermitirReact");  
app.UseSession();
app.UseAuthorization();
app.MapControllers();
app.Run();