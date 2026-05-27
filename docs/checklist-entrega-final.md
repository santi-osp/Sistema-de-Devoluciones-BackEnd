# Checklist entrega final

## Backend

- [x] Backend .NET 8 Web API por capas.
- [x] Swagger disponible en `/swagger`.
- [x] Health check disponible en `/api/health`.
- [x] JWT Bearer configurado.
- [x] Autorizacion por roles: Cliente, Administrador, Analista y Proveedor.
- [x] EF Core con PostgreSQL y migracion inicial aplicada en entorno local.
- [x] Seeders de Development conectados al arranque.
- [x] Supabase Storage integrado para evidencias.
- [x] Exportacion CSV/PDF de reportes.
- [x] Middleware global de errores.

## Frontend Angular

- [x] Angular creado en `frontend-angular/`.
- [x] Login conectado a la API real.
- [x] Interceptor Bearer.
- [x] Manejo 401 con limpieza de sesion.
- [x] Guards por autenticacion y rol.
- [x] Menu lateral por rol.
- [x] Flujo Cliente conectado a API real.
- [x] Flujo Administrador/Analista conectado a API real.
- [x] Flujo Reportes conectado a API real.
- [x] Flujo Proveedor conectado a API real.
- [x] Build Angular correcto.

## Roles y rutas

- [x] Publico: `/login`.
- [x] Cliente: pedidos, productos, solicitudes, detalle y evidencias.
- [x] Administrador/Analista: dashboard, bandeja, detalle operativo y reportes.
- [x] Proveedor: casos asignados y detalle.
- [x] Wildcard redirige a login.
- [x] Entrada `/` redirige segun rol autenticado.

## Evidencias y storage

- [x] Upload multipart desde Angular.
- [x] Validacion de archivo en backend.
- [x] Subida real a Supabase Storage.
- [x] Metadata persistida en PostgreSQL.
- [x] Listado de evidencias sin exponer secretos.

## Reportes

- [x] Metricas desde API.
- [x] Generacion de reportes.
- [x] Descarga CSV como archivo.
- [x] Descarga PDF como archivo.
- [x] No se imprime contenido binario en consola.

## Seguridad

- [x] No se agregaron tokens reales al frontend.
- [x] Angular no usa `service_role`.
- [x] README usa placeholders para secretos.
- [x] `appsettings*.json` esta ignorado por Git en el backend local.
- [x] No se ejecutaron comandos destructivos.
- [x] No se generaron migraciones nuevas en el cierre.

## Auditoria de mocks

- [x] No se encontraron mocks como fuente principal en `frontend-angular/src`.
- [x] No se encontraron arrays locales de pedidos, solicitudes, reportes o casos reemplazando la API.
- [x] Los usuarios demo quedan exclusivamente como seed del backend Development.

## Pendientes conocidos

- [ ] Ejecutar una pasada visual manual en navegador con resoluciones movil y escritorio.
- [ ] Si se requiere comparacion exacta con Blazor, incorporar capturas o archivos reales de la UI Blazor; en este workspace `Sistema-de-Devoluciones-FrontEnd` no contiene pantallas fuente, solo README y `.git`.
- [ ] Revisar archivos de prueba subidos a Supabase Storage y decidir manualmente si se conservan o se eliminan.
- [ ] Agregar pruebas automatizadas e2e para login y flujos principales.
