# Arquitectura Técnica — App de Gestión de Devoluciones y Garantías

## Qué hacer ahora

1. Guarda este documento en el repositorio como `docs/arquitectura-tecnica-devoluciones-garantias.md`.
2. Guarda el UML como `docs/uml/diagrama_clases_unificado_organizado.puml`.
3. No le pidas a Codex que implemente todo todavía.
4. Primero pídele a Codex un análisis del repositorio sin modificar archivos.
5. Después de revisar ese análisis, implementa por fases.

---

## Contexto técnico confirmado

| Decisión | Valor |
|---|---|
| Backend | .NET 8 Web API |
| Frontend final | Angular |
| Frontend actual | Integración Figma + GitHub en Vite |
| Base de datos | Neon PostgreSQL |
| ORM | Entity Framework Core con Npgsql |
| Storage | Supabase Storage |
| Auth | JWT propio en .NET |
| Arquitectura | Por capas |
| Swagger | Funcional |
| Login | No existe, debe crearse |

---

## Módulos obligatorios

| Código | Módulo |
|---|---|
| RU-01 | Gestión de solicitudes |
| RU-02 | Gestión operativa |
| RU-03 | Gestión de proveedores |
| RU-04 | Reportes y métricas |
| RU-05 | Autenticación y gestión de sesión |

---

## Patrones obligatorios

| Patrón | Uso |
|---|---|
| Strategy | Reglas de elegibilidad de devolución y garantía |
| Builder | Creación controlada de solicitudes |
| Observer | Notificaciones y eventos por cambios de estado |
| State | Ciclo de vida de solicitudes |
| Facade | Coordinación de módulos principales |

---

## Arquitectura general

```text
Frontend Angular
        │
        ▼
API Layer (.NET 8 Web API)
        │
        ▼
Application Layer
        │
        ▼
Domain Layer
        │
        ▼
Infrastructure Layer
        │
        ├── Persistence Layer → Neon PostgreSQL
        └── Storage Layer → Supabase Storage
```

---

## Backend recomendado

```text
DevolucionesGarantias/
├── DevolucionesGarantias.sln
├── README.md
├── docker-compose.yml
├── .env.example
├── src/
│   ├── DevolucionesGarantias.Api/
│   │   ├── Controllers/
│   │   │   ├── AuthController.cs
│   │   │   ├── RequestsController.cs
│   │   │   ├── OrdersController.cs
│   │   │   ├── OperationController.cs
│   │   │   ├── ProvidersController.cs
│   │   │   └── ReportsController.cs
│   │   ├── Middlewares/
│   │   ├── Filters/
│   │   ├── Extensions/
│   │   ├── Swagger/
│   │   ├── Program.cs
│   │   └── appsettings.json
│   ├── DevolucionesGarantias.Application/
│   │   ├── Auth/
│   │   ├── Requests/
│   │   ├── Operation/
│   │   ├── Providers/
│   │   ├── Reports/
│   │   ├── Common/
│   │   └── DependencyInjection.cs
│   ├── DevolucionesGarantias.Domain/
│   │   ├── Entities/
│   │   ├── Enums/
│   │   ├── ValueObjects/
│   │   ├── Strategies/
│   │   ├── Builders/
│   │   ├── Observers/
│   │   ├── States/
│   │   ├── Facades/
│   │   ├── Interfaces/
│   │   └── Exceptions/
│   ├── DevolucionesGarantias.Infrastructure/
│   │   ├── Auth/
│   │   ├── Repositories/
│   │   ├── Storage/
│   │   ├── Notifications/
│   │   ├── Reports/
│   │   └── DependencyInjection.cs
│   ├── DevolucionesGarantias.Persistence/
│   │   ├── Context/
│   │   ├── Configurations/
│   │   ├── Migrations/
│   │   ├── Seeders/
│   │   └── DependencyInjection.cs
│   └── DevolucionesGarantias.Shared/
└── tests/
```

---

## Frontend Angular recomendado

```text
src/app/
├── core/
│   ├── config/
│   ├── guards/
│   ├── interceptors/
│   ├── services/
│   └── errors/
├── shared/
│   ├── components/
│   ├── models/
│   ├── pipes/
│   ├── directives/
│   └── utils/
├── layouts/
│   ├── public-layout/
│   └── private-layout/
├── features/
│   ├── auth/
│   ├── requests/
│   ├── operation/
│   ├── providers/
│   └── reports/
├── models/
├── app.routes.ts
├── app.config.ts
└── app.component.ts
```

---

## Base de datos principal

Tablas principales:

- `roles`
- `users`
- `user_roles`
- `sessions`
- `login_attempts`
- `orders`
- `products`
- `order_products`
- `request_types`
- `solution_preferences`
- `request_statuses`
- `requests`
- `evidence`
- `request_timeline`
- `internal_comments`
- `operational_decisions`
- `additional_information_requests`
- `provider_assignments`
- `warranty_validations`
- `technical_reports`
- `product_receptions`
- `reports`
- `report_metrics`
- `exported_files`
- `audit_logs`

---

## Relaciones principales

```text
users 1 ── * sessions
users * ── * roles
users 1 ── * orders
orders 1 ── * order_products
products 1 ── * order_products
users 1 ── * requests
orders 1 ── * requests
products 1 ── * requests
requests 1 ── * evidence
requests 1 ── * internal_comments
requests 1 ── * operational_decisions
requests 1 ── * request_timeline
requests 1 ── 0..1 technical_reports
requests 1 ── 0..1 warranty_validations
requests 1 ── 0..1 product_receptions
reports 1 ── * report_metrics
reports 1 ── * exported_files
users 1 ── * audit_logs
```

---

## Seeders requeridos

- Roles: Cliente, Administrador, Proveedor, Analista.
- Usuarios demo por rol.
- Estados: Creada, EnRevision, PendienteInformacion, Aprobada, Rechazada, Cerrada.
- Tipos: Devolucion, Garantia.
- Preferencias: Reembolso, Cambio, Reparacion.
- Pedidos y productos de prueba.
- Solicitudes de devolución y garantía.
- Evidencias simuladas.
- Comentarios internos.
- Decisiones operativas.
- Dictámenes técnicos.
- Reportes y métricas iniciales.

---

## Endpoints mínimos

### Auth

- `POST /api/auth/login`
- `POST /api/auth/logout`
- `GET /api/auth/me`
- `GET /api/auth/validate-access`

### Solicitudes

- `GET /api/orders`
- `GET /api/orders/{orderId}`
- `GET /api/orders/{orderId}/products`
- `GET /api/products/{productId}/eligibility`
- `POST /api/requests`
- `GET /api/requests`
- `GET /api/requests/{id}`
- `GET /api/requests/{id}/timeline`
- `POST /api/requests/{id}/evidence`
- `GET /api/requests/{id}/evidence`

### Operación

- `GET /api/operation/dashboard`
- `GET /api/operation/requests`
- `GET /api/operation/requests/{id}`
- `POST /api/operation/requests/{id}/comments`
- `POST /api/operation/requests/{id}/request-information`
- `POST /api/operation/requests/{id}/approve`
- `POST /api/operation/requests/{id}/reject`

### Proveedores

- `GET /api/providers/cases`
- `GET /api/providers/cases/{id}`
- `POST /api/providers/cases/{id}/warranty-validation`
- `POST /api/providers/cases/{id}/technical-report`
- `POST /api/providers/cases/{id}/authorize-repair`
- `POST /api/providers/cases/{id}/authorize-replacement`
- `POST /api/providers/cases/{id}/reception`

### Reportes

- `POST /api/reports/generate`
- `GET /api/reports`
- `GET /api/reports/{id}`
- `GET /api/reports/metrics`
- `GET /api/reports/{id}/export?format=csv`
- `GET /api/reports/{id}/export?format=pdf`

---

## Supabase Storage

Buckets:

```text
evidence-files
report-files
```

Rutas:

```text
evidence/{requestId}/{guid}-{filename}
reports/{reportId}/{format}/{guid}-{filename}
```

Flujo:

```text
Angular → .NET 8 API → Validación → Supabase Storage → Neon guarda metadata
```

---

## Trazabilidad

La tabla `audit_logs` debe registrar:

- Usuario.
- Rol.
- Acción.
- Entidad afectada.
- Id de entidad.
- Valores anteriores.
- Valores nuevos.
- Fecha y hora.
- IP.
- User-Agent.
- TraceId.

Acciones auditables:

- Login exitoso.
- Login fallido.
- Logout.
- Creación de solicitud.
- Carga de evidencia.
- Cambio de estado.
- Aprobación.
- Rechazo.
- Solicitud de información adicional.
- Asignación a proveedor.
- Validación de garantía.
- Registro de dictamen técnico.
- Exportación de reporte.

---

## Prompt para Codex — usar ahora

```text
Analiza el repositorio completo antes de modificar archivos.

Contexto del proyecto:
- Sistema: App de Gestión de Devoluciones y Garantías para un E-commerce.
- Backend requerido: .NET 8 Web API.
- Frontend requerido: Angular.
- Frontend actual: integración Figma + GitHub en Vite.
- Base de datos: Neon PostgreSQL.
- ORM recomendado: Entity Framework Core con Npgsql.
- Storage: Supabase Storage para evidencias y reportes exportados.
- Login: no existe, debe implementarse.
- Arquitectura requerida: arquitectura por capas.
- Swagger/OpenAPI debe quedar funcional.
- Debe respetarse el Entregable 2, el UML unificado organizado, SOLID y los patrones Strategy, Builder, Observer, State y Facade.

Documentos de referencia:
- docs/arquitectura-tecnica-devoluciones-garantias.md
- docs/uml/diagrama_clases_unificado_organizado.puml
- Entregable 2, si está disponible en el repositorio.

Reglas obligatorias:
- No modifiques archivos todavía.
- No ejecutes builds, tests, lint, typecheck ni comandos largos.
- No instales paquetes.
- No generes migraciones todavía.
- No crees carpetas todavía.
- Valida estáticamente leyendo la estructura y el código existente.
- Identifica archivos impactados antes de proponer cambios.
- Reutiliza componentes existentes siempre que sean coherentes.
- Respeta la arquitectura y estilo actual del proyecto cuando no contradiga el UML.
- No agregues cambios fuera del alcance.
- No hardcodees secretos ni valores configurables.
- Usa configuración por appsettings, variables de entorno o el patrón existente del proyecto.
- Si hay dudas, pregúntame antes de actuar.
- No asumas comportamiento no especificado.
- Maneja correctamente booleanos; un valor false no debe omitirse.
- Revisa el diff esperado antes de proponer implementación.

Entrega un informe con:

1. Estado actual del repositorio:
   - Estructura de carpetas.
   - Framework frontend detectado.
   - Framework backend detectado.
   - Si existe o no backend .NET.
   - Si existe o no Angular.
   - Si el frontend actual está en Vite, React, HTML u otra estructura.
   - Archivos de configuración importantes.

2. Diferencias contra la arquitectura objetivo:
   - Qué falta para tener .NET 8 Web API.
   - Qué falta para tener Angular.
   - Qué debe migrarse desde Vite/Figma hacia Angular.
   - Qué debe mantenerse para conservar el diseño visual.
   - Qué debe eliminarse o refactorizarse.

3. Comparación contra el UML:
   - Clases, interfaces y servicios que ya existen.
   - Clases, interfaces y servicios que faltan.
   - Patrones no implementados.
   - Entidades duplicadas o mal ubicadas.

4. Comparación contra el Entregable 2:
   - RF cubiertos.
   - RF faltantes.
   - Reglas de negocio faltantes.
   - RNF que requieren implementación técnica.

5. Datos quemados/mocks:
   - Arrays locales.
   - JSON simulados.
   - Usuarios quemados.
   - Solicitudes quemadas.
   - Reportes quemados.
   - Servicios fake.

6. Plan de implementación:
   - Orden recomendado para implementar.
   - Carpetas que deben crearse.
   - Archivos que deben crearse.
   - Archivos que deben modificarse.
   - Riesgos de romper el frontend.
   - Riesgos de romper el backend.
   - Dependencias NuGet o npm que podrían requerirse, pero sin instalarlas todavía.

7. Resultado final:
   - Lista clara de brechas.
   - Lista de archivos impactados.
   - Plan por fases.
   - Preguntas pendientes antes de implementar.

No implementes nada todavía. Solo analiza y propone el plan.
```

---

## Prompt para Codex — después de aprobar el análisis

```text
Con base en el análisis aprobado del repositorio, implementa únicamente la Fase 1: backend base .NET 8 con arquitectura por capas.

Reglas:
- Implementa solo la fase indicada.
- No avances a otras fases sin aprobación.
- No ejecutes builds, tests, lint, typecheck ni comandos largos.
- Valida estáticamente la coherencia del código.
- Reutiliza componentes existentes cuando sean coherentes.
- Respeta el estilo del repositorio.
- No hardcodees secretos.
- No agregues cambios fuera del alcance.
- Identifica archivos impactados antes de modificar.
- Revisa el diff antes de finalizar.
- Entrega resumen claro de archivos creados y modificados.

Fase 1:
- Crear o ajustar la solución .NET 8.
- Crear estructura de proyectos por capas si no existe:
  - Api.
  - Application.
  - Domain.
  - Infrastructure.
  - Persistence.
  - Shared.
  - Tests.
- Configurar referencias entre proyectos.
- Configurar Program.cs base.
- Configurar Swagger base.
- Configurar CORS base.
- Configurar appsettings con placeholders seguros para Neon, JWT y Supabase.
- Crear DependencyInjection.cs por capa.
- No implementar todavía lógica completa de módulos.

Al finalizar:
- Resume archivos creados/modificados.
- Indica qué quedó pendiente para Fase 2.
- Indica cualquier duda o riesgo detectado.
```

---

## Checklist de validación

| Elemento | Estado |
|---|---|
| Backend .NET 8 incluido | Sí |
| Angular incluido | Sí |
| Migración Vite/Figma incluida | Sí |
| Neon PostgreSQL incluido | Sí |
| EF Core + Npgsql incluido | Sí |
| Supabase Storage incluido | Sí |
| JWT propio incluido | Sí |
| Arquitectura por capas definida | Sí |
| Swagger/OpenAPI contemplado | Sí |
| RU-01 a RU-05 cubiertos | Sí |
| Strategy incluido | Sí |
| Builder incluido | Sí |
| Observer incluido | Sí |
| State incluido | Sí |
| Facade incluido | Sí |
| Base de datos definida | Sí |
| Seeders definidos | Sí |
| API REST definida | Sí |
| Integración Angular + API definida | Sí |
| Trazabilidad definida | Sí |
| Prompt para Codex incluido | Sí |
| Implementación por fases definida | Sí |

