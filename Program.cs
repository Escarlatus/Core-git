using Asilo.Core.Interfaces;
using Asilo.Core.Repositories;
using Asilo.Core.Services;

var builder = WebApplication.CreateBuilder(args);

// =============================================================================
// 1. CONFIGURACIÓN DE BASE DE DATOS (INTELIGENTE) 🧠
// =============================================================================

// A. Buscamos si Render nos dio una cadena de conexión (Variable de Entorno)
var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection");

// B. Si está vacía (significa que estamos en local), usamos la del archivo de configuración o la hardcodeada
if (string.IsNullOrEmpty(connectionString))
{
    // OJO: Puedes dejar tu cadena local aquí como respaldo para cuando programes en tu PC
    connectionString = "Server=DESKTOP-7FR6MHL\\BD2;Database=AsiloDB;Trusted_Connection=True;TrustServerCertificate=True;";
}

// =============================================================================
// 2. REPOSITORIOS (Capa de Datos)
// =============================================================================
// Aquí pasamos la variable 'connectionString' que ya decidimos arriba
builder.Services.AddScoped<IResidenteRepository>(p => new SqlResidenteRepository(connectionString));
builder.Services.AddScoped<IIngresoRepository>(p => new SqlIngresoRepository(connectionString));
builder.Services.AddScoped<IHabitacionRepository>(p => new SqlHabitacionRepository(connectionString));
builder.Services.AddScoped<IServicioRepository>(p => new SqlServicioRepository(connectionString));
builder.Services.AddScoped<IAnalisisRepository>(p => new SqlAnalisisRepository(connectionString));
builder.Services.AddScoped<IProcedimientoRepository>(p => new SqlProcedimientoRepository(connectionString));
builder.Services.AddScoped<ICuentaRepository>(p => new SqlCuentaRepository(connectionString));
builder.Services.AddScoped<IMovimientoRepository>(p => new SqlMovimientoRepository(connectionString));
builder.Services.AddScoped<IUsuarioRepository>(p => new SqlUsuarioRepository(connectionString));
builder.Services.AddScoped<IAuditRepository>(p => new SqlAuditRepository(connectionString));

// =============================================================================
// 3. SERVICIOS (Capa de Negocio)
// =============================================================================
builder.Services.AddScoped<ResidenteService>();
builder.Services.AddScoped<IngresoService>();
builder.Services.AddScoped<CuentaService>();
builder.Services.AddScoped<UsuarioService>();
builder.Services.AddScoped<AuditService>();

// =============================================================================
// 4. API
// =============================================================================
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// MODIFICACIÓN IMPORTANTE: Sacamos Swagger del "if Development"
// Así podrás ver el Swagger también en Render (Producción) para probar
app.UseSwagger();
app.UseSwaggerUI();

// app.UseHttpsRedirection(); // <--- MANTENER COMENTADO EN RENDER PARA EVITAR ERRORES

app.UseAuthorization();
app.MapControllers();

// Render asigna el puerto dinámicamente, pero .NET 8 con Docker suele manejarlo bien.
app.Run();