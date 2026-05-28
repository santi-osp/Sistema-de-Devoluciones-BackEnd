# Flujo de revision y aprobacion con proveedor

## Flujo funcional

1. El cliente crea la solicitud desde el frontend. La solicitud queda en `Creada` y no se asigna a ningun proveedor.
2. El administrador revisa la solicitud. Desde `Creada` puede solicitar informacion al cliente, rechazarla o enviarla a revision del proveedor.
3. La accion `Enviar a revision` cambia la solicitud a `EnRevisionProveedor` y crea o reactiva un `CasoAsignado` para un proveedor real registrado en base de datos. Si el request incluye `providerId`, se usa ese proveedor; si no, se toma el primer proveedor disponible.
4. El proveedor ve el caso en su bandeja, registra la validacion de garantia y emite un dictamen tecnico:
   - `Procede`: da visto bueno.
   - `NoProcede`: no da visto bueno y exige motivo tecnico.
   - `RequiereRevisionAdicional`: deja el caso con conflicto para analisis del administrador.
5. Al registrar el dictamen, el caso pasa a `Dictaminado` y la solicitud queda en `PendienteDecisionFinalAdmin`.
6. El administrador ve el dictamen, observaciones, disponibilidad, conflictos y resolucion de conflicto. Luego aprueba o rechaza definitivamente.
7. Al aprobar o rechazar definitivamente, la solicitud queda en `Aprobada` o `Rechazada` y el caso asignado pasa a `Cerrado`.

## Estados de solicitud

- `Creada`: solicitud registrada por el cliente, pendiente de revision inicial del administrador.
- `EnRevision`: estado legado de revision operativa. Se mantiene para compatibilidad.
- `PendienteInformacion`: el administrador solicito correccion o informacion adicional al cliente.
- `EnRevisionProveedor`: solicitud asignada y visible para el proveedor.
- `PendienteDecisionFinalAdmin`: el proveedor ya emitio dictamen y el administrador debe decidir.
- `Aprobada`: decision final aprobada por administrador.
- `Rechazada`: decision final rechazada por administrador.
- `Cerrada`: solicitud cerrada operativamente.

## Permisos por rol

- Cliente:
  - Crear solicitud.
  - Ver sus solicitudes.
  - Adjuntar evidencias.
- Administrador/Analista operativo:
  - Ver bandeja y detalle operativo.
  - Solicitar informacion desde la revision inicial.
  - Enviar a revision del proveedor.
  - Ver respuesta del proveedor, disponibilidad y conflictos.
  - Aprobar o rechazar definitivamente.
- Proveedor:
  - Ver solo casos asignados a su usuario.
  - Registrar validacion de garantia.
  - Emitir dictamen tecnico con visto bueno, rechazo o revision adicional.
  - No aprobar ni rechazar la decision final.

## Endpoints involucrados

- `POST /api/requests`: crea la solicitud del cliente en `Creada`.
- `GET /api/operation/requests`: lista solicitudes para backoffice.
- `GET /api/operation/requests/{id}`: detalle operativo con `providerReview`.
- `POST /api/operation/requests/{id}/request-information`: devuelve a `PendienteInformacion`.
- `POST /api/operation/requests/{id}/send-to-review`: asigna proveedor y cambia a `EnRevisionProveedor`.
- `POST /api/providers/cases/{id}/warranty-validation`: registra validacion de garantia.
- `POST /api/providers/cases/{id}/technical-report`: registra dictamen y cambia a `PendienteDecisionFinalAdmin`.
- `GET /api/providers/cases`: bandeja de casos asignados al proveedor autenticado.
- `GET /api/providers/cases/{id}`: detalle de caso asignado con `review`.
- `POST /api/operation/requests/{id}/approve`: decision final aprobada.
- `POST /api/operation/requests/{id}/reject`: decision final rechazada.

## Cambios backend

- Se agregaron los estados `EnRevisionProveedor` y `PendienteDecisionFinalAdmin`.
- `Enviar a revision` ahora crea o reutiliza `CasoAsignado`; ya no es solo cambio de estado.
- El proveedor solo puede trabajar casos en `EnRevisionProveedor` o en el estado legado `EnRevision` cuando ya estan asignados.
- El dictamen tecnico mueve la solicitud a `PendienteDecisionFinalAdmin`.
- La decision final cierra el `CasoAsignado`.
- El detalle operativo y el detalle proveedor incluyen `ProviderReviewDto`.
- Se corrigio la validacion de disponibilidad para usar `ProductoId` y `Cantidad` de la solicitud, no el `RequestId`.
- Se mantiene compatibilidad con datos existentes en `EnRevision`.

## Cambios frontend

- La bandeja admin filtra `EnRevisionProveedor` y `PendienteDecisionFinalAdmin`.
- El detalle admin muestra asignacion, validacion de garantia, dictamen, disponibilidad, conflicto y resolucion.
- El boton `Aprobar` solo aparece para decision final o estado legado compatible.
- `Enviar a revision` queda disponible desde `Creada` y `PendienteInformacion`.
- El proveedor ve el resultado ya registrado y no duplica validacion o dictamen desde la UI.
- La resolucion de conflicto se muestra cuando la API la entrega, incluso si la solicitud ya esta `Aprobada`.

## Base de datos, migraciones y seeders

- No se requirio migracion nueva porque `requests.status` y `assigned_cases.status` se almacenan como texto sin restriccion por lista de enum.
- El seeder de solicitudes sigue creando un caso demo asignado; con la nueva transicion queda en `EnRevisionProveedor`.
- `CasoAsignado`, `ValidacionGarantia` y `DictamenTecnico` son las tablas que guardan la trazabilidad del proveedor.

## Disponibilidad, conflicto y resolucion

- La disponibilidad se evalua desde el dictamen tecnico y la preferencia de solucion.
- Para `Cambio` y `Reparacion`, la API consulta inventario con `ProductoId` y `Cantidad`.
- Si la garantia no esta vigente, no hay stock, el proveedor no da visto bueno o pide revision adicional, `hasConflict` queda activo y se expone `conflictReason`.
- `conflictResolution` siempre se calcula cuando existe dictamen o conflicto; el frontend no lo oculta por estar `Aprobada`.

## Casos de prueba manual

1. Cliente crea solicitud y confirmar que queda en `Creada`.
2. Proveedor abre su bandeja y confirmar que la solicitud creada no aparece.
3. Admin abre el detalle y ejecuta `Enviar a revision`.
4. Proveedor confirma que el caso aparece en `GET /api/providers/cases`.
5. Proveedor registra garantia vigente y dictamen `Procede`.
6. Admin confirma que el detalle queda en `PendienteDecisionFinalAdmin` y muestra visto bueno, disponibilidad y resolucion.
7. Admin aprueba definitivamente y confirma estado `Aprobada`.
8. Confirmar que la resolucion sigue visible en el detalle aprobado.
9. Repetir con garantia no vigente o dictamen `NoProcede` y confirmar que se muestra conflicto y resolucion para decision final.
