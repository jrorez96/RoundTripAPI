var builder = WebApplication.CreateBuilder(args);

// Añadir controladores
builder.Services.AddControllers();

// Paso 1: Añadir el servicio de CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder.AllowAnyOrigin()    // Permitir cualquier origen
                   .AllowAnyMethod()    // Permitir cualquier método HTTP (GET, POST, etc.)
                   .AllowAnyHeader();   // Permitir cualquier encabezado
        });
});

// Añadir Swagger para desarrollo
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Paso 2: Configurar la política de CORS en la tubería de solicitudes HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseSwagger();
//app.UseSwaggerUI();

app.UseHttpsRedirection();

// Aplicar CORS antes de autorizar las solicitudes
app.UseCors("AllowAllOrigins");

app.UseAuthorization();

// Mapear los controladores
app.MapControllers();

app.Run();
