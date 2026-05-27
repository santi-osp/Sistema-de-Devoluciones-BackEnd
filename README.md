# DevolucionesGarantias Backend

Backend .NET 8 Web API para la App de Gestion de Devoluciones y Garantias de un e-commerce.

## Requisitos

- .NET SDK 8.x.
- PostgreSQL compatible con Npgsql. En desarrollo se usa Supabase Postgres manteniendo la clave de configuracion `ConnectionStrings:NeonPostgres`.
- Buckets de Supabase Storage:
  - `evidence-files`
  - `report-files`

## Arquitectura

- `DevolucionesGarantias.Api`: controllers, Swagger, CORS, JWT y middleware de errores.
- `DevolucionesGarantias.Application`: casos de uso, DTOs, validadores, mappers e interfaces.
- `DevolucionesGarantias.Domain`: entidades, value objects, reglas de negocio y patrones Strategy, Builder, Observer, State y Facade.
- `DevolucionesGarantias.Infrastructure`: repositorios, JWT, hashing, storage Supabase, auditoria, notificaciones y exportadores.
- `DevolucionesGarantias.Persistence`: EF Core, configuraciones, migraciones y seeders de desarrollo.
- `DevolucionesGarantias.Shared`: respuestas y utilidades transversales.

## Configuracion local

No guardar secretos reales en archivos versionados. Usa variables de entorno, User Secrets o `appsettings*.json` local ignorado por Git.

Variables requeridas:

- `ConnectionStrings__NeonPostgres`
- `Jwt__Issuer`
- `Jwt__Audience`
- `Jwt__Secret`
- `Jwt__AccessTokenExpirationMinutes`
- `Supabase__Url`
- `Supabase__ServiceRoleKey`
- `Supabase__EvidenceBucket`
- `Supabase__ReportsBucket`

Ejemplo PowerShell con placeholders:

```powershell
$env:ConnectionStrings__NeonPostgres='Host=<HOST>;Port=5432;Database=<DATABASE>;Username=<USER>;Password=<PASSWORD>;SSL Mode=Require;Trust Server Certificate=true'
$env:Jwt__Issuer='DevolucionesGarantias'
$env:Jwt__Audience='DevolucionesGarantias.Angular'
$env:Jwt__Secret='<JWT_SECRET_MIN_32_CHARS>'
$env:Jwt__AccessTokenExpirationMinutes='60'
$env:Supabase__Url='https://<PROJECT_REF>.supabase.co'
$env:Supabase__ServiceRoleKey='<SUPABASE_SERVICE_ROLE_KEY>'
$env:Supabase__EvidenceBucket='evidence-files'
$env:Supabase__ReportsBucket='report-files'
```

## Comandos

Desde esta carpeta:

```powershell
dotnet restore
dotnet build
$env:ASPNETCORE_ENVIRONMENT='Development'
dotnet run --project src/DevolucionesGarantias.Api
```

Migracion inicial, solo cuando la base este configurada y nunca para resetear datos:

```powershell
dotnet ef database update --project src/DevolucionesGarantias.Persistence --startup-project src/DevolucionesGarantias.Api
```

## Swagger

Con la API levantada:

- `http://localhost:5000/swagger`
- `GET http://localhost:5000/api/health`

Swagger incluye Bearer JWT. Usa el boton `Authorize` con `Bearer <token>`.

## Usuarios demo de desarrollo

Los usuarios demo existen solo como seed de Development:

- `cliente.demo@ecommerce.com`
- `admin.demo@ecommerce.com`
- `proveedor.demo@ecommerce.com`
- `analista.demo@ecommerce.com`

La contrasena demo debe tratarse como dato local de desarrollo, nunca productivo.

## Flujos validados

- Login JWT y roles.
- Cliente: pedidos, productos, elegibilidad, solicitudes, evidencias.
- Administrador/Analista: dashboard, bandeja, detalle, comentarios, informacion adicional, decisiones y reportes.
- Proveedor: casos asignados, validacion de garantia, dictamen, autorizaciones y recepcion.
- Supabase Storage para evidencias.
- Exportacion CSV/PDF de reportes.

## Seguridad

- No exponer connection strings, `Jwt:Secret` ni `Supabase:ServiceRoleKey`.
- Angular nunca debe usar `service_role`.
- No ejecutar `database update` contra produccion sin revision.
- No borrar tablas ni resetear esquemas durante validaciones.
