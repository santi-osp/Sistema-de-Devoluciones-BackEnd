# Backend legacy reference

Este documento registra la informacion util del backend legacy antes de su eliminacion controlada. El backend legacy se considera descartable y no debe usarse como base de implementacion. El nuevo backend debe crearse desde cero en .NET 8 Web API con arquitectura por capas.

## Tecnologia detectada

- .NET 8 Web API.
- Minimal APIs.
- Entity Framework Core.
- Npgsql para PostgreSQL/Neon.
- User Secrets configurado en el proyecto legacy.
- CORS configurable mediante `Cors:AllowedOrigins`.
- Persistencia configurada mediante `ConnectionStrings:DefaultConnection`.
- No se detecto Swagger/OpenAPI funcional en `Program.cs`.
- No se detecto JWT Bearer real; existia un `TokenService` propio simplificado.

## Endpoints legacy detectados

Raiz:

- `GET /`

Auth:

- `POST /api/auth/login`
- `POST /api/auth/logout`

Catalogo:

- `GET /api/catalogo/clientes/{clienteId}/pedidos`
- `GET /api/catalogo/pedidos/{pedidoId}/productos`
- `GET /api/catalogo/pedidos/{pedidoId}/productos/{sku}`

Solicitudes:

- `GET /api/solicitudes/`
- `GET /api/solicitudes/{id}`
- `GET /api/solicitudes/cliente/{clienteId}`
- `POST /api/solicitudes/`
- `POST /api/solicitudes/{id}/evidencias`
- `PATCH /api/solicitudes/{id}/estado`

Operacion:

- `GET /api/operacion/bandeja`
- `GET /api/operacion/solicitudes/{id}`
- `POST /api/operacion/solicitudes/{id}/comentarios`
- `POST /api/operacion/solicitudes/{id}/decisiones`

Proveedor:

- `GET /api/proveedor/casos?proveedorId={proveedorId}`
- `POST /api/proveedor/solicitudes/{id}/dictamenes`
- `POST /api/proveedor/solicitudes/{id}/autorizar-reparacion`
- `POST /api/proveedor/solicitudes/{id}/autorizar-reemplazo`

Reportes:

- `GET /api/reportes/metricas`

## DTOs y contratos utiles como referencia

- `CreateSolicitudRequest`
- `ChangeEstadoRequest`
- `AddEvidenceRequest`
- `AddComentarioRequest`
- `AddDecisionRequest`
- `AddDictamenRequest`
- `LoginRequest`
- `RequestModelDto`
- `PedidoDto`
- `ProductoDto`
- `InternalCommentDto`
- `MetricDto`

Notas:

- Algunos contratos mezclaban nombres en ingles y espanol (`OrderNumber`, `Product`, `Sku`, `Reason`, `ClientId`).
- `AddComentarioRequest.VisibleCliente` y `AddDecisionRequest.Aprobada` eran booleanos relevantes; en la nueva implementacion un valor `false` debe persistirse y serializarse correctamente.
- `CreateSolicitudRequest` tenia valores por defecto demo y no debe conservarlos como comportamiento productivo.

## Modelos conceptuales utiles

Auth y usuarios:

- `Usuario`
- `Cliente`
- `Administrador`
- `Proveedor`
- `Sesion`

Solicitudes y catalogo:

- `Pedido`
- `Producto`
- `Solicitud`
- `Evidencia`
- `TipoSolicitud`
- `PreferenciaSolucion`

Operacion:

- `ComentarioInterno`
- `DecisionOperativa`

Proveedor:

- `DictamenTecnico`

Reportes:

- `FiltroReporte`
- `IndicadorMetrica`
- `Reporte`
- `Archivo`

## Configuraciones utiles detectadas

- `ConnectionStrings:DefaultConnection`: cadena PostgreSQL/Neon. Debe venir de appsettings seguro, variables de entorno o User Secrets en desarrollo.
- `Cors:AllowedOrigins`: lista de origenes permitidos para frontend local.
- Recomendacion legacy para Neon: usar SSL y evitar placeholders o secretos reales en archivos versionados.
- `UserSecretsId`: existia en el `.csproj` legacy, pero no debe reutilizarse automaticamente para el backend nuevo.

## Seeds y usuarios demo

El backend legacy contenia seeds de desarrollo con usuarios demo:

- Cliente: `cliente@demo.com / demo123`
- Administrador: `admin@demo.com / admin123`
- Proveedor: `proveedor@demo.com / prov123`

Tambien se detectaron ids y datos demo como:

- `cliente-demo`
- `admin-demo`
- `proveedor-techpro`
- `TechPro Solutions`
- Solicitudes `SOL-*`
- Pedidos `ORD-*`
- Reporte `rep-demo-1`

Estos datos solo pueden existir como seed de desarrollo. No deben quedar como credenciales productivas, secretos, autorizacion real ni fuente final de datos.

## Partes que no deben reutilizarse

- Estructura de proyecto legacy como base principal.
- Minimal APIs legacy como implementacion final.
- Migraciones legacy.
- `DbContext` legacy.
- Repositorios legacy acoplados al modelo anterior.
- `TokenService` legacy sin JWT Bearer real.
- Seeds demo con credenciales o hashes fijos como comportamiento productivo.
- `Program.cs` legacy.
- `appsettings` legacy salvo como referencia de claves de configuracion.
- Diagrama duplicado dentro del backend legacy.

## Riesgos de eliminacion

- Perder nombres de rutas consumibles por el frontend futuro si no se documentan.
- Perder shape de DTOs que ya estaban alineados parcialmente con pantallas React/Vite.
- Perder datos demo utiles para seeders de desarrollo.
- Romper integraciones locales si alguien esperaba ejecutar el backend legacy.
- Confundir el estado Git del backend legacy, porque puede conservar historia aunque el codigo fisico sea eliminado.

## Recomendacion

Crear el nuevo backend desde cero en .NET 8 Web API con arquitectura por capas:

- `Api`
- `Application`
- `Domain`
- `Infrastructure`
- `Persistence`
- `Shared`
- `Tests`

El backend nuevo debe implementar Swagger/OpenAPI, JWT, autorizacion por rol, EF Core con Npgsql para Neon PostgreSQL, Supabase Storage, trazabilidad/auditoria y los patrones requeridos por el Entregable 2 y el UML: Strategy, Builder, Observer, State y Facade.
