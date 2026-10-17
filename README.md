# Sistema de Pedidos

## Descripción

Sistema de pedidos desarrollado como una API REST con ASP.NET Core 8. Permite registrar clientes y productos, administrar pedidos y mantener actualizado el stock disponible. La información se almacena en SQL Server mediante Entity Framework Core.

La API utiliza controladores y DTO para separar los datos recibidos de las entidades de dominio. También aplica validaciones antes de guardar la información y devuelve códigos HTTP según el resultado de cada operación.

## Tecnologías utilizadas

- .NET 8 y C#
- ASP.NET Core Web API
- Entity Framework Core 8 con enfoque Code First
- SQL Server 2022
- Swagger y OpenAPI
- Docker y Docker Compose
- xUnit

## Estructura del proyecto

La solución está organizada en cuatro capas principales:

- **SistemaPedidos.API:** contiene los controladores, la configuración de la aplicación, Swagger y el manejo de errores HTTP.
- **SistemaPedidos.Application:** contiene los DTO, las interfaces de servicios y repositorios, y la lógica de negocio.
- **SistemaPedidos.Domain:** contiene las entidades Cliente, Producto y Pedido, además de las excepciones del dominio.
- **SistemaPedidos.Infrastructure:** contiene el contexto de Entity Framework Core, los repositorios, las configuraciones de las entidades y las migraciones de la base de datos.

Las pruebas unitarias se encuentran en `tests/SistemaPedidos.Tests`.

## Funcionalidades

- Registro, consulta, actualización y eliminación de clientes.
- Registro, consulta, actualización y eliminación de productos.
- Creación y administración de pedidos relacionados con un cliente y un producto.
- Descuento automático del stock al crear un pedido.
- Ajuste del inventario al actualizar o eliminar un pedido.
- Validación de campos obligatorios, correo electrónico, precio, stock y cantidad.
- Rechazo de pedidos cuando el cliente o el producto no existen.
- Rechazo de pedidos cuando el stock disponible es insuficiente.
- Restricción para eliminar clientes o productos que tienen pedidos asociados.
- Control de concurrencia del inventario mediante una columna `RowVersion`.

## Ejecución del proyecto

### Requisitos

- Docker Desktop con contenedores Linux.
- Puertos `8080` y `1433` disponibles.
- .NET 8 SDK para ejecutar las pruebas o trabajar sin Docker.

### Ejecución con Docker

Desde la carpeta raíz del proyecto, cree el archivo local de variables de entorno:

```powershell
Copy-Item .env.example .env
```

Abra `.env` y reemplace el valor de `SQL_SA_PASSWORD` por una contraseña propia que incluya mayúsculas, minúsculas, números y símbolos. Este archivo está excluido de Git.

Inicie SQL Server y la API:

```powershell
docker compose up --build -d
```

Puede comprobar el estado de los servicios con:

```powershell
docker compose ps
```

La API espera que SQL Server esté disponible y aplica las migraciones de Entity Framework Core durante el inicio. Cuando ambos contenedores estén listos, Swagger estará disponible en:

```text
http://localhost:8080/swagger
```

Para detener los contenedores sin eliminar el volumen de la base de datos:

```powershell
docker compose down
```

## Endpoints

| Método | Ruta | Descripción |
|---|---|---|
| GET | `/api/clientes` | Lista todos los clientes. |
| GET | `/api/clientes/{id}` | Consulta un cliente por su identificador. |
| POST | `/api/clientes` | Registra un cliente. |
| PUT | `/api/clientes/{id}` | Actualiza un cliente. |
| DELETE | `/api/clientes/{id}` | Elimina un cliente sin pedidos asociados. |
| GET | `/api/productos` | Lista todos los productos. |
| GET | `/api/productos/{id}` | Consulta un producto por su identificador. |
| POST | `/api/productos` | Registra un producto. |
| PUT | `/api/productos/{id}` | Actualiza un producto. |
| DELETE | `/api/productos/{id}` | Elimina un producto sin pedidos asociados. |
| GET | `/api/pedidos` | Lista todos los pedidos. |
| GET | `/api/pedidos/{id}` | Consulta un pedido por su identificador. |
| POST | `/api/pedidos` | Crea un pedido y descuenta el stock. |
| PUT | `/api/pedidos/{id}` | Actualiza un pedido y ajusta el stock. |
| DELETE | `/api/pedidos/{id}` | Elimina un pedido y devuelve el stock. |

## Pruebas

Las pruebas unitarias validan los servicios de clientes, productos y pedidos, así como las reglas de inventario y las Data Annotations de los DTO. No requieren SQL Server ni contenedores Docker.

Para restaurar las dependencias y ejecutar las pruebas:

```powershell
dotnet restore "MR222768_desafío03.sln"
dotnet test "MR222768_desafío03.sln"
```

## Autor

CIF: MR222768
