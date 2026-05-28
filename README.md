# Sistema de Gestion de Devoluciones y Garantias - Backend

Backend .NET 8 Web API para la app de gestion de devoluciones y garantias de un e-commerce. Expone los servicios REST consumidos por el frontend Angular y concentra la logica de negocio, validaciones, persistencia, autenticacion, reportes y almacenamiento de evidencias.

## Integrantes

1. Andrés Felipe Méndez Cano
2. Santiago Ospina Arango
3. Juan Pablo Restrepo Muñoz
4. Daniel Bedoya Naranjo

## Tecnologias utilizadas

- .NET 8 Web API.
- ASP.NET Core.
- Entity Framework Core.
- PostgreSQL compatible con Npgsql, usando la clave `ConnectionStrings:NeonPostgres`.
- Supabase Storage para evidencias y reportes exportados.
- JWT Bearer Authentication.
- Swagger / OpenAPI.
- Arquitectura por capas.
- Patrones GoF: Strategy, Builder, Observer, State y Facade.
- Repositorios y Unit of Work.
- Exportacion CSV/PDF.

## Funcionalidades implementadas

- Login JWT y cierre de sesion.
- Consulta del usuario autenticado.
- Autorizacion por roles: Cliente, Administrador y Proveedor.
- Cliente: pedidos, productos, elegibilidad, solicitudes, detalle, timeline y evidencias.
- Administrador: dashboard, bandeja operativa, detalle, comentarios, informacion adicional, asignacion a proveedor, aprobacion, rechazo y reportes.
- Proveedor: casos asignados, validacion de garantia, dictamen tecnico, autorizaciones y recepcion.
- Persistencia de datos con EF Core y PostgreSQL.
- Carga de evidencias a Supabase Storage.
- Exportacion de reportes en CSV/PDF.
- Middleware global de errores.
- Auditoria de acciones relevantes.

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

- Cliente: `cliente.demo@ecommerce.com`
- Administrador: `admin.demo@ecommerce.com`
- Proveedor: `proveedor.demo@ecommerce.com`

Contrasena para los usuarios demo:

```text
DevDemo123!
```

La contrasena demo debe tratarse como dato local de desarrollo, nunca productivo.

## Flujos validados

- Login JWT y roles.
- Cliente: pedidos, productos, elegibilidad, solicitudes, evidencias.
- Administrador: dashboard, bandeja, detalle, comentarios, informacion adicional, decisiones y reportes.
- Proveedor: casos asignados, validacion de garantia, dictamen, autorizaciones y recepcion.
- Supabase Storage para evidencias.
- Exportacion CSV/PDF de reportes.

## Seguridad

- No exponer connection strings, `Jwt:Secret` ni `Supabase:ServiceRoleKey`.
- Angular nunca debe usar `service_role`.
- No ejecutar `database update` contra produccion sin revision.
- No borrar tablas ni resetear esquemas durante validaciones.
