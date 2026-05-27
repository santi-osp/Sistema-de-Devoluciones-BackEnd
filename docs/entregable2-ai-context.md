# Entregable 2 — AI Context Compacto

> Archivo recomendado: `docs/entregable2-ai-context.md`  
> Objetivo: resumir el Entregable 2 en un formato compacto, fácil de leer por IA y útil para Codex sin pegar 70 páginas completas.

---

## 0. Instrucciones para IA

```yaml
ai_usage:
  purpose: "Contexto funcional y arquitectónico compacto para implementar la App de Gestión de Devoluciones y Garantías."
  source: "Entregable 2 - Diseño de Sistemas de Información"
  project_name: "App de Gestión de Devoluciones y Garantías para un E-commerce"
  implementation_target:
    backend: ".NET 8 Web API"
    frontend: "Angular"
    current_frontend: "Figma + GitHub en Vite"
    database: "Neon PostgreSQL"
    orm: "Entity Framework Core + Npgsql"
    file_storage: "Supabase Storage"
    auth: "JWT propio en .NET"
    architecture: "Arquitectura por capas"
  rule:
    - "No asumir funcionalidades fuera del Entregable 2."
    - "Respetar UML unificado organizado."
    - "Respetar SOLID."
    - "Usar Strategy, Builder, Observer, State y Facade."
    - "Mantener trazabilidad de acciones críticas."
    - "No dejar mocks como fuente final de datos."
```

---

## 1. Metadatos del entregable

```yaml
document:
  name: "Entregable 2"
  course: "Diseño de Sistemas de Información"
  institution: "Instituto Tecnológico Metropolitano"
  date: "2026-04-16"
  project: "App de Gestión de Devoluciones y Garantías para un E-commerce"
  authors:
    - "Andrés Felipe Méndez Cano"
    - "Santiago Ospina Arango"
    - "Juan Pablo Restrepo Muñoz"
    - "Daniel Bedoya Naranjo"
  teacher: "Alexandra Guerrero Bocanegra"
```

---

## 2. Actores y roles

```yaml
roles:
  Cliente:
    description: "Usuario que crea solicitudes de devolución o garantía y consulta sus casos."
    permissions:
      - "Ver pedidos propios"
      - "Seleccionar productos elegibles"
      - "Crear solicitudes"
      - "Adjuntar evidencias"
      - "Consultar historial y detalle de sus solicitudes"
  Administrador:
    description: "Usuario interno que gestiona operación, revisa casos, solicita información y toma decisiones."
    permissions:
      - "Ver dashboard operativo"
      - "Filtrar solicitudes"
      - "Revisar evidencias"
      - "Registrar comentarios internos"
      - "Solicitar información adicional"
      - "Aprobar o rechazar solicitudes"
      - "Consultar y exportar reportes"
  Proveedor:
    description: "Usuario que evalúa casos asignados, valida garantía y registra dictámenes técnicos."
    permissions:
      - "Ver casos asignados"
      - "Evaluar evidencias y producto"
      - "Validar vigencia de garantía"
      - "Registrar dictamen técnico"
      - "Autorizar reparación o reemplazo cuando proceda"
  Analista:
    description: "Usuario interno orientado a métricas y reportes."
    permissions:
      - "Consultar métricas"
      - "Generar reportes"
      - "Exportar reportes"
```

---

## 3. Requisitos de usuario

```yaml
user_requirements:
  RU-01:
    name: "Gestión de solicitudes"
    description: "Permitir a clientes crear, consultar y gestionar solicitudes de devolución o garantía."
  RU-02:
    name: "Gestión operativa"
    description: "Permitir al administrador supervisar pedidos, solicitudes y operación general."
  RU-03:
    name: "Gestión proveedores"
    description: "Permitir interacción con proveedores para evaluación, procedencia, reparación o reemplazo."
  RU-04:
    name: "Reportes y métricas"
    description: "Permitir generar, consultar y exportar reportes con métricas de gestión y desempeño."
  RU-05:
    name: "Autenticación"
    description: "Permitir iniciar y cerrar sesión de forma segura según rol."
```

---

## 4. Requisitos funcionales

```yaml
functional_requirements:
  RF-01:
    ru: "RU-01"
    name: "Seleccionar pedido/producto elegible"
    actor: "Cliente"
    description: "El cliente selecciona un pedido y productos elegibles."
  RF-02:
    ru: "RU-01"
    name: "Formulario de solicitud"
    actor: "Cliente"
    description: "El cliente diligencia motivo y tipo de solicitud."
  RF-03:
    ru: "RU-01"
    name: "Adjuntar evidencias"
    actor: "Cliente"
    description: "El cliente carga fotos, videos y comprobantes."
  RF-04:
    ru: "RU-01"
    name: "Ver historial"
    actor: "Cliente"
    description: "El cliente consulta historial y detalle de solicitudes."
  RF-05:
    ru: "RU-02"
    name: "Dashboard solicitudes"
    actor: "Administrador"
    description: "El administrador visualiza solicitudes pendientes."
  RF-06:
    ru: "RU-02"
    name: "Revisión de evidencias"
    actor: "Administrador"
    description: "El administrador visualiza evidencias y comentarios."
  RF-07:
    ru: "RU-02"
    name: "Solicitar información adicional"
    actor: "Administrador"
    description: "El administrador solicita información al cliente."
  RF-08:
    ru: "RU-02"
    name: "Aprobar/Rechazar solicitud"
    actor: "Administrador"
    description: "El administrador toma decisión sobre solicitudes."
  RF-09:
    ru: "RU-03"
    name: "Evaluación de evidencia"
    actor: "Proveedor"
    description: "El proveedor registra evaluación del producto/evidencia."
  RF-10:
    ru: "RU-03"
    name: "Validar vigencia de garantía"
    actor: "Proveedor"
    description: "El proveedor verifica si el producto está dentro del periodo de garantía."
  RF-11:
    ru: "RU-03"
    name: "Registrar dictamen técnico"
    actor: "Proveedor"
    description: "El proveedor registra conclusión técnica y solución aplicable."
  RF-12:
    ru: "RU-04"
    name: "Consultar métricas y reportes"
    actor: "Administrador"
    description: "El administrador consulta panel de indicadores operativos."
  RF-13:
    ru: "RU-04"
    name: "Filtrar reportes"
    actor: "Administrador"
    description: "El administrador aplica filtros por fechas, estados y tipos de caso."
  RF-14:
    ru: "RU-04"
    name: "Exportación de reportes"
    actor: "Administrador"
    description: "El administrador exporta reportes en CSV y PDF."
  RF-15:
    ru: "RU-05"
    name: "Autenticación y gestión de sesión"
    actor: "Usuario"
    description: "El sistema permite iniciar sesión, validar credenciales y gestionar sesiones."
```

---

## 5. Requisitos no funcionales

```yaml
non_functional_requirements:
  RNF-01:
    type: "Desempeño"
    description: "La autenticación debe completarse en menos de 2 segundos."
  RNF-02:
    type: "Desempeño"
    description: "La carga de pedidos debe responder en menos de 2 segundos en el 95% de los casos."
  RNF-03:
    type: "Seguridad"
    description: "Solo usuarios autenticados pueden acceder a la información del sistema."
  RNF-04:
    type: "Seguridad"
    description: "El sistema debe manejar permisos diferenciados por rol."
  RNF-05:
    type: "Accesibilidad"
    description: "La plataforma debe ser navegable con teclado y compatible con lectores de pantalla."
  RNF-06:
    type: "Compatibilidad"
    description: "La plataforma debe funcionar en desktop y dispositivos móviles."
  RNF-07:
    type: "Privacidad"
    description: "El sistema solo debe mostrar información asociada al usuario autenticado."
  RNF-08:
    type: "Disponibilidad"
    description: "El sistema debe mantener disponibilidad de pedidos y almacenamiento."
  RNF-09:
    type: "Trazabilidad"
    description: "Todas las acciones críticas deben registrarse con fecha, hora y usuario."
  RNF-10:
    type: "Exportación"
    description: "Los archivos exportados deben ser compatibles con herramientas estándar."
```

---

## 6. Reglas de negocio

```yaml
business_rules:
  RN-01: "Solo se pueden seleccionar productos asociados al cliente autenticado."
  RN-02: "Solo se muestran productos elegibles dentro de la ventana permitida."
  RN-03: "No se permiten solicitudes duplicadas sobre el mismo pedido y motivo."
  RN-04: "Toda decisión administrativa debe tener un motivo obligatorio."
  RN-05: "Los comentarios internos no son visibles para el cliente."
  RN-06: "Si el daño corresponde a mal uso, el caso no procede."
  RN-07: "La autorización de reparación o reemplazo solo puede ocurrir si el caso procede."
  RN-08: "Las sesiones deben expirar después del tiempo definido por seguridad."
```

---

## 7. Historias de usuario compactas

```yaml
user_stories:
  HU-GS-01:
    module: "Gestión de solicitudes"
    actor: "Cliente"
    goal: "Seleccionar pedido y productos elegibles."
    includes: ["ver pedidos propios", "ver productos del pedido", "validar elegibilidad", "seleccionar producto"]
    excludes: ["crear solicitud", "cargar evidencias", "evaluar solicitud"]
    rules: ["solo pedidos propios", "producto dentro de política", "sin solicitud previa", "mostrar motivo de no elegibilidad"]
    acceptance:
      - "muestra pedidos del cliente autenticado"
      - "muestra productos elegibles y no elegibles"
      - "permite seleccionar producto elegible"
      - "bloquea producto no elegible y muestra motivo"
    nfr: ["desempeño", "seguridad", "accesibilidad", "compatibilidad", "privacidad"]

  HU-GS-02:
    module: "Gestión de solicitudes"
    actor: "Cliente"
    goal: "Completar formulario de solicitud."
    includes: ["datos precargados", "motivo", "descripción", "cantidad", "preferencia de solución", "crear solicitud"]
    excludes: ["revisión administrativa", "aprobación", "reembolso/cambio/reparación"]
    rules: ["campos obligatorios", "no duplicados", "cantidad <= comprada", "preferencia válida", "producto aún elegible"]
    acceptance:
      - "muestra pedido/producto precargado"
      - "crea solicitud con identificador único"
      - "bloquea campos obligatorios vacíos"
      - "bloquea cantidad mayor a comprada"
      - "bloquea si deja de ser elegible"

  HU-GS-03:
    module: "Gestión de solicitudes"
    actor: "Cliente"
    goal: "Consultar historial y detalle de solicitudes."
    includes: ["listado", "detalle", "estado", "historial", "evidencias", "filtros por estado/fecha"]
    excludes: ["editar", "cancelar", "operar caso"]
    rules: ["solo solicitudes propias", "historial cronológico descendente", "detalle restringido al propietario"]
    acceptance:
      - "muestra solicitudes propias"
      - "muestra detalle con estado, eventos y evidencias"
      - "muestra empty state si no hay solicitudes"
      - "bloquea acceso a solicitudes ajenas"

  HU-GS-04:
    module: "Gestión de solicitudes"
    actor: "Cliente"
    goal: "Adjuntar evidencias."
    includes: ["selección de archivos", "validación formato/tamaño/cantidad", "carga", "almacenamiento", "asociación a solicitud"]
    excludes: ["edición de archivos", "análisis automático", "eliminación avanzada"]
    rules: ["formatos permitidos", "tamaño máximo", "cantidad máxima", "asociación a solicitud", "validación de seguridad"]
    acceptance:
      - "almacena archivos válidos"
      - "rechaza tamaño excedido"
      - "rechaza formato no permitido"
      - "permite reintento ante interrupción"
      - "bloquea archivo riesgoso y registra evento"

  HU-GO-01:
    module: "Gestión operativa"
    actor: "Administrador"
    goal: "Ver y filtrar solicitudes pendientes."
    includes: ["dashboard", "filtros por estado/fecha/prioridad", "identificación de prioritarias", "acceso a detalle"]
    excludes: ["toma de decisión", "evaluación", "gestión proveedor"]
    rules: ["solo administrador", "solo pendientes", "estado actualizado", "prioridad por tiempo", "evitar edición concurrente"]
    acceptance:
      - "muestra pendientes"
      - "destaca prioridad alta"
      - "filtra listado"
      - "muestra empty state"
      - "permite reintentar error de carga"

  HU-GO-02:
    module: "Gestión operativa"
    actor: "Administrador"
    goal: "Revisar evidencias y comentarios del caso."
    includes: ["visualizar evidencias", "visualizar comentarios", "registrar comentario interno"]
    excludes: ["decisión final", "comunicación directa", "modificar evidencias"]
    rules: ["solo administrador", "evidencias asociadas a solicitud", "comentarios con autor/fecha", "comentarios no modificables ni eliminables"]
    acceptance:
      - "muestra evidencias y comentarios"
      - "visualiza evidencia según tipo"
      - "registra comentario con trazabilidad"
      - "maneja evidencia corrupta/no disponible"
      - "permite pasar a solicitar información adicional"

  HU-GO-03:
    module: "Gestión operativa"
    actor: "Administrador"
    goal: "Solicitar información adicional al cliente."
    includes: ["registrar solicitud", "mensaje", "plazo", "notificación", "cambio de estado"]
    excludes: ["respuesta cliente", "reevaluación automática", "decisión final"]
    rules: ["motivo obligatorio", "plazo obligatorio", "trazabilidad", "estado en espera de información", "solo administrador"]
    acceptance:
      - "notifica al cliente y cambia estado"
      - "registra fecha límite"
      - "registra errores de notificación y permite reintento"
      - "mantiene o gestiona estado si no hay respuesta"

  HU-GO-04:
    module: "Gestión operativa"
    actor: "Administrador"
    goal: "Registrar decisión final aprobar/rechazar."
    includes: ["seleccionar decisión", "motivo obligatorio", "actualizar estado", "registrar trazabilidad"]
    excludes: ["procesar reembolso", "evaluación física", "modificar decisiones cerradas"]
    rules: ["motivo obligatorio", "solo autorizado", "cerrada no modificable sin autorización", "registrar usuario/fecha/motivo", "comentarios internos no visibles"]
    acceptance:
      - "aprueba y registra motivo"
      - "rechaza y registra motivo"
      - "bloquea sin motivo"
      - "bloquea conflicto de edición/estado"

  HU-GP-01:
    module: "Gestión proveedor"
    actor: "Proveedor/Inspector"
    goal: "Registrar evaluación física del producto devuelto."
    includes: ["ver evidencias", "ver datos producto", "registrar evaluación", "observaciones", "resultado preliminar"]
    excludes: ["decisión final", "reembolso/cambio", "comunicación cliente"]
    rules: ["solo proveedor asignado", "evaluación con usuario/fecha/observaciones", "observación obligatoria si evidencia no coincide", "criterios de negocio", "acceso solo a casos asignados"]
    acceptance:
      - "muestra evidencia y datos del producto"
      - "guarda evaluación con trazabilidad"
      - "permite registrar discrepancias"
      - "deja caso disponible para aclaración si falta soporte"
      - "permite reintento ante error"

  HU-GP-02:
    module: "Gestión proveedor"
    actor: "Proveedor"
    goal: "Validar vigencia de garantía."
    includes: ["consultar datos del producto", "verificar periodo", "comparar fecha base", "mostrar resultado", "registrar validación"]
    excludes: ["dictamen final", "autorizar reparación/reemplazo", "modificar pedido/producto", "comunicación cliente"]
    rules: ["solo proveedor asignado", "garantía configurada", "comparar contra fecha base", "informar no vigente", "registrar usuario/fecha/hora", "no alterar pedido"]
    acceptance:
      - "muestra periodo de garantía y fecha base"
      - "indica garantía vigente"
      - "indica garantía no vigente"
      - "informa datos insuficientes"
      - "guarda resultado con trazabilidad"

  HU-GP-03:
    module: "Gestión proveedor"
    actor: "Proveedor/Administrador autorizado"
    goal: "Registrar dictamen técnico."
    includes: ["resultado procede/no procede/revisión adicional", "observaciones", "motivo técnico", "actualizar estado", "trazabilidad"]
    excludes: ["revisar evidencias", "autorizar reembolso", "comunicación cliente", "modificar evaluaciones previas"]
    rules: ["requiere revisión previa", "resultado válido", "si no procede motivo obligatorio", "si revisión adicional estado correspondiente", "usuario/fecha/resultado/observaciones", "solo asignado/autorizado"]
    acceptance:
      - "registra reparación si procede"
      - "registra reemplazo si procede y hay inventario"
      - "bloquea acción sin stock"
      - "permite reintento si inventario falla"
      - "bloquea acción no compatible"

  HU-RM-01:
    module: "Reportes y métricas"
    actor: "Administrador/Analista"
    goal: "Generar y exportar reportes por fechas y filtros."
    includes: ["rango de fechas", "filtros", "vista previa", "exportación"]
    excludes: ["analítica predictiva", "BI externo", "modificar datos históricos"]
    rules: ["respetar filtros", "informar sin resultados", "respetar límites", "solo autorizados"]
    acceptance:
      - "muestra vista previa filtrada"
      - "muestra mensaje sin resultados"
      - "genera archivo con datos filtrados"
      - "permite continuar/notifica si tarda"
      - "permite reintentar error"

  HU-RM-02:
    module: "Reportes y métricas"
    actor: "Administrador de operaciones"
    goal: "Visualizar SLA y detectar incumplimientos."
    includes: ["tiempos de resolución", "casos fuera de SLA", "filtros", "métricas agregadas"]
    excludes: ["escalamiento automático", "modificar reglas SLA", "intervenir casos desde panel"]
    rules: ["SLA por tipo de solicitud", "calcular con eventos", "marcar incumplidos", "marcar eventos incompletos", "solo autorizados"]
    acceptance:
      - "muestra tiempos con eventos completos"
      - "marca casos fuera de SLA"
      - "marca datos incompletos"
      - "actualiza métricas con filtros"
      - "permite reintento ante error"

  HU-RM-03:
    module: "Reportes y métricas"
    actor: "Administrador"
    goal: "Exportar resultados en CSV o PDF."
    includes: ["seleccionar formato", "generar archivo", "descargar archivo"]
    excludes: ["envío automático por correo", "integraciones externas", "formatos adicionales"]
    rules: ["exportar solo datos visibles", "requiere permisos", "no exportar sin datos", "archivo refleja datos mostrados"]
    acceptance:
      - "genera CSV"
      - "genera PDF"
      - "bloquea exportación sin datos"
      - "maneja error de generación"
      - "soporta volumen alto"

  HU-SEC-01:
    module: "Seguridad y acceso"
    actor: "Cliente/Administrador/Proveedor"
    goal: "Autenticarse y acceder según rol."
    includes: ["login", "validación identidad", "sesión", "control por roles"]
    excludes: ["recuperación de contraseña", "proveedores externos de identidad", "gestión avanzada de usuarios"]
    rules: ["acceso por rol", "sesión expira por inactividad", "registrar intentos fallidos", "bloqueo temporal tras múltiples fallos", "validar credenciales"]
    acceptance:
      - "credenciales válidas crean sesión activa"
      - "credenciales inválidas rechazan acceso"
      - "sesión expirada pide autenticación"
      - "usuario sin permisos no accede a módulo restringido"
      - "múltiples fallos generan bloqueo temporal"
```

---

## 8. Patrones de diseño esperados

```yaml
design_patterns:
  Strategy:
    purpose: "Variar reglas de elegibilidad según tipo de solicitud."
    target_classes: ["ReglaElegibilidadStrategy", "ReglaElegibilidadDevolucion", "ReglaElegibilidadGarantia", "ContextoElegibilidad", "ResultadoElegibilidad"]
  Builder:
    purpose: "Construir solicitudes con datos requeridos y validaciones de creación."
    target_classes: ["SolicitudBuilder", "Solicitud"]
  Observer:
    purpose: "Notificar eventos y cambios de estado de solicitud."
    target_classes: ["ISolicitudObserver", "ISolicitudSubject", "SolicitudNotifier", "INotificacionService"]
  State:
    purpose: "Controlar transiciones de estado de una solicitud."
    states: ["Creada", "EnRevision", "PendienteInformacion", "Aprobada", "Rechazada", "Cerrada"]
  Facade:
    purpose: "Coordinar servicios principales del MVP desde una interfaz transversal."
    target_classes: ["ServicioGestionDevoluciones", "AuthService", "SolicitudService", "BandejaOperativaService", "GestionProveedorService", "ReporteService"]
```

---

## 9. C4 y arquitectura esperada

```yaml
c4_architecture:
  context:
    external_users: ["Cliente", "Administrador", "Proveedor", "Analista"]
    system: "Sistema de Gestión de Devoluciones y Garantías"
    external_dependencies:
      - "Servicio de pedidos"
      - "Servicio de almacenamiento de archivos"
      - "Servicio de notificaciones"
      - "Servicio de inventario"
  containers:
    frontend:
      type: "Angular Web App"
      responsibilities: ["UI", "rutas", "guards", "interceptors", "servicios API", "formularios", "dashboards"]
    backend:
      type: ".NET 8 Web API"
      responsibilities: ["API REST", "auth", "reglas de negocio", "orquestación", "Swagger", "auditoría"]
    database:
      type: "Neon PostgreSQL"
      responsibilities: ["usuarios", "roles", "sesiones", "pedidos", "productos", "solicitudes", "reportes", "auditoría"]
    storage:
      type: "Supabase Storage"
      responsibilities: ["evidencias", "comprobantes", "videos", "imágenes", "reportes exportados"]
  backend_layers:
    - "API Layer"
    - "Application Layer"
    - "Domain Layer"
    - "Infrastructure Layer"
    - "Persistence Layer"
    - "Shared/Common Layer"
    - "Tests"
```

---

## 10. Prototipo funcional y anexos

```yaml
prototype:
  repository: "https://github.com/DanielBN09/Sistema-De-Devoluciones.git"
  figma: "https://www.figma.com/make/1hprf0zlHLytmn9b09mHDS/Sistema-de-Devoluciones"
  screens:
    - "Pantalla Login"
    - "Panel cliente"
    - "Panel administrador"
    - "Panel proveedor"
  diagrams:
    editable_diagrams: "draw.io en Google Drive"
    activity_diagram: "Diagrama de actividades UML.docx"
    gof_patterns_template: "Plantilla de análisis y selección de patrones GoF.docx"
    solid_template: "Plantilla de Análisis Principios SOLID.docx"
```

---

## 11. Entidades base sugeridas por el Entregable + UML

```yaml
domain_entities:
  auth: ["Usuario", "Cliente", "Administrador", "Proveedor", "Rol", "Sesion", "LoginAttempt"]
  requests: ["Solicitud", "Pedido", "Producto", "Evidencia", "RequestTimeline"]
  operation: ["ComentarioInterno", "DecisionOperativa", "SolicitudInformacionAdicional"]
  providers: ["CasoAsignado", "ValidacionGarantia", "DictamenTecnico", "RecepcionProducto"]
  reports: ["Reporte", "IndicadorMetrica", "FiltroReporte", "ArchivoExportado"]
  audit: ["AuditLog"]
```

---

## 12. Endpoints mínimos derivados

```yaml
api_endpoints:
  auth:
    - "POST /api/auth/login"
    - "POST /api/auth/logout"
    - "GET /api/auth/me"
    - "GET /api/auth/validate-access"
  requests:
    - "GET /api/orders"
    - "GET /api/orders/{orderId}"
    - "GET /api/orders/{orderId}/products"
    - "GET /api/products/{productId}/eligibility"
    - "POST /api/requests"
    - "GET /api/requests"
    - "GET /api/requests/{id}"
    - "GET /api/requests/{id}/timeline"
    - "POST /api/requests/{id}/evidence"
    - "GET /api/requests/{id}/evidence"
  operation:
    - "GET /api/operation/dashboard"
    - "GET /api/operation/requests"
    - "GET /api/operation/requests/{id}"
    - "POST /api/operation/requests/{id}/comments"
    - "POST /api/operation/requests/{id}/request-information"
    - "POST /api/operation/requests/{id}/approve"
    - "POST /api/operation/requests/{id}/reject"
  providers:
    - "GET /api/providers/cases"
    - "GET /api/providers/cases/{id}"
    - "POST /api/providers/cases/{id}/warranty-validation"
    - "POST /api/providers/cases/{id}/technical-report"
    - "POST /api/providers/cases/{id}/authorize-repair"
    - "POST /api/providers/cases/{id}/authorize-replacement"
    - "POST /api/providers/cases/{id}/reception"
  reports:
    - "POST /api/reports/generate"
    - "GET /api/reports"
    - "GET /api/reports/{id}"
    - "GET /api/reports/metrics"
    - "GET /api/reports/{id}/export?format=csv"
    - "GET /api/reports/{id}/export?format=pdf"
```

---

## 13. Archivos que deben estar en `docs/`

```yaml
docs_expected_files:
  - path: "docs/arquitectura-tecnica-devoluciones-garantias.md"
    purpose: "Arquitectura técnica completa definida para implementación."
  - path: "docs/entregable2-ai-context.md"
    purpose: "Versión compacta del Entregable 2 para IA/Codex."
  - path: "docs/uml/diagrama_clases_unificado_organizado.puml"
    purpose: "UML organizado y refinado del sistema."
```

---

## 14. Instrucciones después de colocar los 3 archivos en `docs/`

### Paso 1 — Confirmar estructura de documentos

Antes de abrir Codex, confirma que el repositorio tenga:

```text
docs/
├── arquitectura-tecnica-devoluciones-garantias.md
├── entregable2-ai-context.md
└── uml/
    └── diagrama_clases_unificado_organizado.puml
```

### Paso 2 — Primer prompt a Codex: análisis sin modificar

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

Documentos de referencia obligatorios:
- docs/arquitectura-tecnica-devoluciones-garantias.md
- docs/entregable2-ai-context.md
- docs/uml/diagrama_clases_unificado_organizado.puml

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

3. Comparación contra el UML y Entregable 2:
   - Clases, interfaces y servicios que ya existen.
   - Clases, interfaces y servicios que faltan.
   - Patrones no implementados.
   - RF cubiertos y faltantes.
   - RNF que requieren implementación.
   - Reglas de negocio faltantes.

4. Datos quemados/mocks:
   - Arrays locales.
   - JSON simulados.
   - Usuarios quemados.
   - Solicitudes quemadas.
   - Reportes quemados.
   - Servicios fake.

5. Plan de implementación:
   - Orden recomendado para implementar.
   - Carpetas que deben crearse.
   - Archivos que deben crearse.
   - Archivos que deben modificarse.
   - Riesgos de romper frontend o backend.
   - Dependencias NuGet o npm que podrían requerirse, sin instalarlas todavía.

No implementes nada todavía. Solo analiza y propone el plan.
```

### Paso 3 — Revisar respuesta de Codex

Valida que Codex responda con:

```yaml
codex_analysis_validation:
  must_include:
    - "estructura actual del repo"
    - "detección exacta del frontend Vite/Figma"
    - "detección de si hay backend .NET o no"
    - "brechas contra arquitectura"
    - "brechas contra UML"
    - "mocks y datos quemados"
    - "lista de archivos impactados"
    - "plan por fases"
    - "preguntas antes de implementar"
  reject_if:
    - "empieza a modificar archivos"
    - "quiere implementar todo en un solo cambio"
    - "ignora Angular"
    - "ignora Neon/PostgreSQL"
    - "ignora Supabase Storage"
    - "no lista archivos impactados"
    - "no separa frontend y backend"
```

### Paso 4 — Segundo prompt a Codex: Fase 1 solamente

Usa este prompt solo después de aprobar el análisis:

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

### Paso 5 — Orden de fases posteriores

```yaml
implementation_phases:
  1: "Backend base .NET 8 por capas"
  2: "Domain: entidades, enums, value objects y patrones UML"
  3: "Persistence: EF Core, Neon, configuraciones, migraciones y seeders"
  4: "Application: DTOs, interfaces, services, validators, mappers"
  5: "API: controllers, Swagger, JWT, autorización por rol"
  6: "Supabase Storage: evidencias y reportes exportados"
  7: "Frontend Angular: estructura core/shared/features/layouts"
  8: "Migración visual desde Vite/Figma a Angular"
  9: "Integración Angular con API real y eliminación de mocks"
  10: "Revisión final estática y documentación de ejecución"
```

---

## 15. Checklist final para IA/Codex

```yaml
final_checklist:
  business_scope:
    RU-01: true
    RU-02: true
    RU-03: true
    RU-04: true
    RU-05: true
  rf_coverage:
    RF-01_to_RF-15: true
  nfr_coverage:
    RNF-01_to_RNF-10: true
  business_rules:
    RN-01_to