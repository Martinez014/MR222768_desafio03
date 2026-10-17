# MR222768_desafío03 — Sistema de pedidos

API REST académica para administrar clientes, productos e inventario, y pedidos. Está construida con ASP.NET Core 8, arquitectura de cuatro capas, Entity Framework Core Code First, SQL Server, Docker y pruebas unitarias xUnit.

## Tecnologías

- .NET 8 / ASP.NET Core Web API con controladores
- Entity Framework Core 8 y proveedor SQL Server
- Data Annotations
- Swagger/OpenAPI (Swashbuckle)
- xUnit
- SQL Server 2022 y Docker Compose

## Arquitectura y estructura

Las dependencias apuntan hacia adentro: `API -> Application <- Infrastructure -> Domain`; Application también depende de Domain. Los controladores solo coordinan HTTP y delegan a servicios.

```text
MR222768_desafio03/
├── src/
│   ├── SistemaPedidos.API/             Controladores, Swagger y middleware
│   ├── SistemaPedidos.Application/     DTOs, interfaces y casos de uso
│   ├── SistemaPedidos.Domain/          Entidades y excepciones de dominio
│   └── SistemaPedidos.Infrastructure/  EF Core, repositorios y migraciones
├── tests/SistemaPedidos.Tests/         Pruebas unitarias y dobles de prueba
├── docs/EVIDENCIAS.md                  Guía para completar el PDF
├── SistemaPedidos.http                 Solicitudes HTTP de ejemplo
├── Dockerfile
├── docker-compose.yml
└── MR222768_desafío03.sln
```

## Requisitos previos

- SDK .NET 8 o posterior capaz de compilar `net8.0`.
- SQL Server accesible, o Docker Desktop con contenedores Linux.
- Git, solo para publicar el proyecto.

La herramienta `dotnet-ef` 8.0.30 está declarada localmente en `.config/dotnet-tools.json`; no se necesita instalarla globalmente.

## Ejecución local en Windows

Desde PowerShell, en la raíz del proyecto:

```powershell
dotnet restore "MR222768_desafío03.sln"
dotnet tool restore
$env:ConnectionStrings__DefaultConnection="Server=localhost,1433;Database=SistemaPedidosDb;User Id=sa;Password=SU_CLAVE;TrustServerCertificate=True;Encrypt=False"
dotnet tool run dotnet-ef database update --project src/SistemaPedidos.Infrastructure --startup-project src/SistemaPedidos.API
dotnet run --project src/SistemaPedidos.API --urls http://localhost:8080
```

Si se desea que la API aplique las migraciones con reintentos al iniciar:

```powershell
$env:ApplyMigrations="true"
dotnet run --project src/SistemaPedidos.API --urls http://localhost:8080
```

No se usa `EnsureCreated`; la base se crea y evoluciona exclusivamente con migraciones.

## Ejecución con Docker

1. Cree el archivo local de variables y cambie la contraseña:

```powershell
Copy-Item .env.example .env
notepad .env
```

2. Valide, construya e inicie:

```powershell
docker compose config
docker compose build
docker compose up -d
docker compose ps
docker compose logs -f api
```

Compose espera a que SQL Server responda al healthcheck. Luego la API reintenta la conexión y ejecuta `Database.Migrate()`; por tanto, aplica la migración inicial antes de aceptar tráfico. La API se conecta a `sqlserver`, el nombre interno del servicio, y escucha por HTTP en el puerto 8080.

Para detener los servicios conservando los datos:

```powershell
docker compose down
```

Para eliminar también el volumen de base de datos (acción destructiva):

```powershell
docker compose down -v
```

## Documentación de API

Con la aplicación iniciada:

- Swagger UI: <http://localhost:8080/swagger>
- Especificación OpenAPI: <http://localhost:8080/swagger/v1/swagger.json>

## Endpoints

| Método | Ruta | Resultado principal |
|---|---|---|
| GET | `/api/clientes` | Lista clientes |
| GET | `/api/clientes/{id}` | Consulta un cliente |
| POST | `/api/clientes` | Crea un cliente (201) |
| PUT | `/api/clientes/{id}` | Actualiza un cliente (200) |
| DELETE | `/api/clientes/{id}` | Elimina un cliente (204) |
| GET | `/api/productos` | Lista productos |
| GET | `/api/productos/{id}` | Consulta un producto |
| POST | `/api/productos` | Crea un producto (201) |
| PUT | `/api/productos/{id}` | Actualiza un producto (200) |
| DELETE | `/api/productos/{id}` | Elimina un producto (204) |
| GET | `/api/pedidos` | Lista pedidos con resumen de cliente y producto |
| GET | `/api/pedidos/{id}` | Consulta un pedido |
| POST | `/api/pedidos` | Crea pedido y descuenta stock (201) |
| PUT | `/api/pedidos/{id}` | Actualiza pedido y ajusta stock (200) |
| DELETE | `/api/pedidos/{id}` | Elimina pedido y restaura stock (204) |

Los datos inválidos devuelven 400; registros inexistentes, 404; reglas de negocio o concurrencia, 409; y errores inesperados, 500 sin detalles internos.

## Ejemplos JSON

Cliente:

```json
{
  "nombre": "Ana",
  "apellido": "López",
  "email": "ana@example.com"
}
```

Producto:

```json
{
  "nombre": "Teclado",
  "descripcion": "Teclado mecánico",
  "precio": 49.99,
  "stock": 10
}
```

Pedido:

```json
{
  "fechaPedido": "2026-10-16T12:00:00Z",
  "cantidad": 2,
  "clienteId": 1,
  "productoId": 1
}
```

Hay más peticiones listas para ejecutar en `SistemaPedidos.http`.

## Migraciones EF Core

Restaurar la herramienta y listar migraciones:

```powershell
dotnet tool restore
dotnet tool run dotnet-ef migrations list --project src/SistemaPedidos.Infrastructure --startup-project src/SistemaPedidos.API
```

Aplicar migraciones:

```powershell
dotnet tool run dotnet-ef database update --project src/SistemaPedidos.Infrastructure --startup-project src/SistemaPedidos.API
```

Crear una migración futura:

```powershell
dotnet tool run dotnet-ef migrations add NombreMigracion --project src/SistemaPedidos.Infrastructure --startup-project src/SistemaPedidos.API --output-dir Persistence/Migrations
```

## Pruebas

Las pruebas no requieren SQL Server ni Docker: los servicios se aíslan mediante repositorios falsos dedicados al proyecto de pruebas.

```powershell
dotnet test "MR222768_desafío03.sln" --configuration Release
```

También se puede generar cobertura:

```powershell
dotnet test "MR222768_desafío03.sln" --collect:"XPlat Code Coverage"
```

## Consistencia y concurrencia

- Crear, actualizar y eliminar pedidos ocurre dentro de una transacción SQL `Serializable`.
- `Productos.Version` se configura como `rowversion`; EF Core detecta actualizaciones concurrentes.
- Si otro proceso modifica el inventario, la operación devuelve 409 y el cliente debe volver a consultar.
- Las restricciones `CHECK` de SQL Server impiden precio/cantidad no positivos y stock negativo aun si se omite la API.
- Las claves foráneas usan borrado restringido. Además, los servicios rechazan expresamente eliminar clientes o productos con pedidos.

## Variables de entorno

| Variable | Uso |
|---|---|
| `SQL_SA_PASSWORD` | Compose: contraseña de `sa`; solo debe estar en `.env` local |
| `ConnectionStrings__DefaultConnection` | Sobrescribe la conexión configurada |
| `ApplyMigrations` | `true` aplica migraciones al inicio con reintentos |
| `ASPNETCORE_URLS` | Dirección de escucha, p. ej. `http://+:8080` |

Nunca confirme `.env` ni contraseñas reales. `.env.example` contiene solo un valor de muestra.

## Problemas frecuentes

- **Error de contraseña de SQL Server:** use al menos 8 caracteres y combine mayúscula, minúscula, número y símbolo; después recree el contenedor.
- **Puerto 1433 u 8080 ocupado:** detenga el servicio conflictivo o cambie el lado izquierdo del mapeo en `docker-compose.yml`.
- **SQL Server aún no responde:** revise `docker compose logs sqlserver`; el arranque inicial puede tardar.
- **Migración pendiente:** ejecute `dotnet tool restore` y luego `dotnet tool run dotnet-ef database update ...`.
- **Certificado SQL local:** la conexión de desarrollo ya usa `TrustServerCertificate=True;Encrypt=False`; no utilice esa política sin evaluación en producción.
- **Docker Desktop apagado:** inícielo y espere hasta que el motor indique que está listo.

## Publicación en GitHub

El repositorio privado del proyecto se encuentra en:

**[github.com/Martinez014/MR222768_desafio03](https://github.com/Martinez014/MR222768_desafio03)**

Para clonar el proyecto mediante HTTPS:

```powershell
git clone https://github.com/Martinez014/MR222768_desafio03.git
```

El acceso requiere una cuenta autorizada porque el repositorio es privado. El archivo `.env`, las carpetas `bin` y `obj`, y los archivos temporales están excluidos mediante `.gitignore`.

## Evidencias y documento de entrega

- Enlace del repositorio: [https://github.com/Martinez014/MR222768_desafio03](https://github.com/Martinez014/MR222768_desafio03)
- Capturas reales: `docs/evidencias/`
- Documento Word editable: `docs/MR222768_Desafio03_Entrega.docx`

Consulte `docs/EVIDENCIAS.md`. El PDF definitivo debe generarse después de revisar el documento Word y sustituir las capturas si se desea.
