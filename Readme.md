# Proyecto ECommerce

Este es un sistema de E-Commerce desarrollado en .NET 8, que incluye una API RESTful, una aplicación web frontend (Blazor WebAssembly) y un simulador de compras. El proyecto está diseñado para gestionar productos, clientes, proveedores, compras, pedidos, inventario y reportes.

## Estructura del Proyecto

- **ApiECommerce/**  
  API principal desarrollada en ASP.NET Core, expone endpoints para la gestión de entidades y lógica de negocio.
- **ECommerceWebAppFrontend/**  
  Aplicación web frontend desarrollada en Blazor WebAssembly.
- **ApiECommerce.Shared/**  
  Biblioteca compartida de modelos y DTOs.
- **ECommerce.Tests/**  
  Proyecto para pruebas unitarias.
- **SimuladorCompras/**  
  Simulador para pruebas de compras automáticas.
- **Script/**  
  Scripts de bases de datos 
- **modelo de datos.drawio/pdf**  
  Diagramas del modelo de datos.

## Tecnologías Utilizadas

- .NET 8
- ASP.NET Core Web API
- Entity Framework Core (MySQL)
- Blazor WebAssembly
- NPOI (para generación de reportes Excel)
- Confluent.Kafka (integración con Kafka)
- Docker (opcional)
- Swashbuckle (Swagger para documentación de la API)

## Configuración Inicial

1. **Clonar el repositorio**
   ```sh
   git clone https://github.com/keilylopez22/ProyectoFinalPrograIII
   ```

2. **Configurar la base de datos**
   - Edita `ApiECommerce/appsettings.json` y `appsettings.Development.json` con la cadena de conexión de tu base de datos MySQL.

3. **Aplicar migraciones**
   ```sh
   cd ApiECommerce
   dotnet ef database update
   ```

4. **Ejecutar la API**
   ```sh
   dotnet run --project ApiECommerce/ApiECommerce.csproj
   ```

5. **Ejecutar el Frontend**
   ```sh
   cd ECommerceWebAppFrontend
   dotnet run
   ```

6. **(Opcional) Usar Docker**
   ```sh
   docker-compose up --build
   ```

## Endpoints y Documentación

- La documentación Swagger está disponible en:  
  `http://localhost:5172/swagger/index.html`

## Funcionalidades Principales

- Gestión de productos, clientes, proveedores y categorías.
- Registro y consulta de compras y pedidos.
- Movimientos de inventario.
- Reportes descargables en Excel.
- Integración con Kafka para procesamiento de pedidos.
- Autenticación y login de usuarios.

## Contribución

1. Haz un fork del repositorio.
2. Crea una rama para tu feature o fix.
3. Haz tus cambios y realiza un commit.
4. Abre un Pull Request.


---

**Desarrollado por:**  
- Keily Lopez
- Delmy Fajardo
- Cristia Chamo
- Cristian Lopez