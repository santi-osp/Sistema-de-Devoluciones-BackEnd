using DevolucionesGarantias.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DevolucionesGarantias.Persistence.Context;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<Cliente> Clientes => Set<Cliente>();
    public DbSet<Administrador> Administradores => Set<Administrador>();
    public DbSet<Proveedor> Proveedores => Set<Proveedor>();
    public DbSet<Rol> Roles => Set<Rol>();
    public DbSet<Sesion> Sesiones => Set<Sesion>();
    public DbSet<LoginAttempt> LoginAttempts => Set<LoginAttempt>();
    public DbSet<Pedido> Pedidos => Set<Pedido>();
    public DbSet<Producto> Productos => Set<Producto>();
    public DbSet<Solicitud> Solicitudes => Set<Solicitud>();
    public DbSet<Evidencia> Evidencias => Set<Evidencia>();
    public DbSet<RequestTimeline> RequestTimeline => Set<RequestTimeline>();
    public DbSet<ComentarioInterno> ComentariosInternos => Set<ComentarioInterno>();
    public DbSet<DecisionOperativa> DecisionesOperativas => Set<DecisionOperativa>();
    public DbSet<SolicitudInformacionAdicional> SolicitudesInformacionAdicional => Set<SolicitudInformacionAdicional>();
    public DbSet<CasoAsignado> CasosAsignados => Set<CasoAsignado>();
    public DbSet<ValidacionGarantia> ValidacionesGarantia => Set<ValidacionGarantia>();
    public DbSet<DictamenTecnico> DictamenesTecnicos => Set<DictamenTecnico>();
    public DbSet<RecepcionProducto> RecepcionesProducto => Set<RecepcionProducto>();
    public DbSet<Reporte> Reportes => Set<Reporte>();
    public DbSet<IndicadorMetrica> IndicadoresMetrica => Set<IndicadorMetrica>();
    public DbSet<ArchivoExportado> ArchivosExportados => Set<ArchivoExportado>();
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
