# Evidencias reales - MR222768_desafío03

Fecha de verificación: 16 de octubre de 2026 (America/El_Salvador).

## Estado verificado

- Docker Desktop 4.93.0: en ejecución.
- Docker Engine 29.8.1: Linux/amd64 sobre WSL2.
- Contexto activo: `desktop-linux`.
- SQL Server 2022: contenedor saludable.
- API ASP.NET Core 8: contenedor en ejecución en `http://localhost:8080`.
- Swagger UI: `http://localhost:8080/swagger` (HTTP 200).
- Migración aplicada: `20261017042631_InitialCreate`.
- Compilación: 0 errores y 0 advertencias.
- xUnit: 19 aprobadas, 0 fallidas, 0 omitidas.

## Pruebas HTTP reales

| Caso | Resultado |
|---|---:|
| Swagger/OpenAPI | 200 |
| Crear cliente | 201 |
| Consultar cliente | 200 |
| Actualizar cliente | 200 |
| Crear producto | 201 |
| Consultar producto | 200 |
| Actualizar producto | 200 |
| Crear pedido | 201 |
| Consultar pedido | 200 |
| Stock luego de crear pedido de 3 unidades | 12 a 9 |
| Datos de cliente inválidos | 400 |
| Pedido con stock insuficiente | 409 |
| Registro inexistente | 404 |
| Eliminar cliente asociado a pedido | 409 |
| Eliminar producto asociado a pedido | 409 |
| Actualizar pedido de 3 a 5 unidades | 200; stock 9 a 7 |
| Eliminar pedido | 204; stock restaurado a 12 |
| Limpieza del producto de prueba | 204 |
| Limpieza del cliente de prueba | 204 |

Los registros de prueba se eliminaron de forma segura. La base, el volumen y los contenedores permanecen activos.

## Archivos de evidencia

- `01-dotnet-restore.txt`: salida real de `dotnet restore`.
- `02-dotnet-build.txt`: compilación real.
- `03-dotnet-test.txt`: ejecución detallada de las 19 pruebas xUnit.
- `04-docker-compose-up.txt`: construcción e inicio de Compose.
- `05-docker-compose-ps.txt`: estado de los contenedores.
- `06-http-integration.json`: resultados estructurados de la integración HTTP.
- `07-migration-applied.txt`: migración consultada directamente en SQL Server.
- `08-api-logs.txt`: logs reales de la API.
- `09-docker-desktop.png`: captura real de Docker Desktop.
- `10-swagger.png`: captura real de Swagger UI.
- `swagger-v1.json`: especificación OpenAPI servida por la API.

Todos se encuentran en `docs/evidencias/`.

## Pendiente para cerrar la entrega

- URL del repositorio GitHub: ______________________________
- Datos personales o institucionales de portada, si la institución los exige: ______________________________

No se inventaron enlaces, capturas ni información académica personal.
