# Nativo Challenge - Sistema de Gestión de Tareas

Este proyecto implementa un sistema de gestión de tareas (tipo "To-Do empresarial") como parte de un desafío técnico. La solución busca aplicar principios de diseño limpio, patrones de arquitectura y buenas prácticas modernas con .NET 8.

---
## Indice
*   [Funcionalidades principales](#funcionalidades-principales)
*   [Arquitectura utilizada](#arquitectura-utilizada)
*   [Principios de diseño aplicados](#principios-de-diseño-aplicados)
*   [Patrones de diseño y arquitectura utilizados](#-patrones-de-diseño-y-arquitectura-utilizados)
*   [Configuración del entorno](#configuración-del-entorno)
*   [DB - Cambiar entre in memory y SQL Server](#-cambiar-entre-in-memory-y-sql-server)
*   [Tests](#Tests)
*   [Swagger y documentación](#Swagger-y-documentación)
*   [Semilla de datos](#Semilla-de-datos)
*   [Funcionalidades opcionales implementadas](#Funcionalidades-opcionales-implementadas)
*   [Mejoras futuras o no implementadas por tiempo](#mejoras-futuras-o-no-implementadas-por-tiempo)
*   [Alternativa simplificada no implementada](#-alternativa-simplificada-no-implementada)
*   [Endpoints principales](#endpoints-principales)
---

## 🎯 Funcionalidades principales

- Crear tarea: Título, descripción, fecha de vencimiento y prioridad (Alta, Media, Baja)
- Marcar tarea como completada
- Listar tareas por estado (pendiente o completada) y ordenadas por prioridad o fecha
- Eliminar una tarea
- Validaciones:
  - Título no vacío
  - No completar tareas vencidas
  - Advertencia si hay más de 10 tareas pendientes de alta prioridad

---

## 🧱 Arquitectura utilizada

Se utilizó una arquitectura inspirada en **Clean Architecture**, lo cual aporta:

- **Separación de responsabilidades** (Separation of Concerns)
- **Mantenibilidad**
- **Testing**
- **Alta cohesión y bajo acoplamiento**
- **Escalabilidad** y modularidad
- **Inversión de dependencias** (Dependency Inversion Principle)
- **Inversión de control** (Inversion of Control Principle)
- **Independencia de frameworks**

### 📚 Estructura de capas

- `Domain`: Entidades, lógica y reglas de negocio.
- `Application`: Casos de uso (CQRS), validaciones, DTOs y lógica de aplicación.
- `Infrastructure`: Acceso a datos (EF Core), configuración de dependencias.
- `WebAPI`: Endpoints HTTP con Minimal API, documentación con Swagger.
- `Tests`: Unit e Integration Tests.

```
src/
├── NativoChallenge.Domain                -> Entidades, enums, reglas de negocio.
├── NativoChallenge.Application           -> Casos de uso (CQRS con MediatR), DTOs, Validaciones.
├── NativoChallenge.Infrastructure        -> EF Core (SQL o In-Memory), repositorios.
├── NativoChallenge.WebAPI                -> Minimal API (.NET 8), validaciones, endpoints.
tests/
├── NativoChallenge.UnitTests             -> Test unitarios de casos de uso, reglas.
├── NativoChallenge.IntegrationTests      -> Test de integración de los endpoints con WebApplicationFactory.
```

---

## ✅ Principios de diseño aplicados

- **SOLID** (especialmente SRP, DIP, OCP)
- **KISS** (Keep It Simple)
- **YAGNI** (You Aren't Gonna Need It)
- **DRY** (Don't Repeat Yourself)
- **Separation of Concerns**
- **Inversion of Control Principle**
- **Explicit Dependencies**

---

## 🧩 Patrones de diseño y arquitectura utilizados

- **CQRS** (Command Query Responsibility Segregation)
- **Strategy** (aplicado a filtros y ordenamiento)
- **Repository Pattern** (incluyendo un repositorio genérico)
- **Dependency Injection** (nativa de .NET)
- **Validation Behavior** (usando MediatR y FluentValidation)
- **Extension Methods** (para configuración)
- **Unit of Work** (a través del contexto de EF Core)
- **Minimal APIs** (.NET 8)

---

## ⚙️ Configuración del entorno

### 🔧 Requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download)
- Visual Studio 2022 o VSCode

---

## ▶️ Cómo ejecutar

### Visual Studio
- Establecé `WebAPI` como proyecto de inicio.
- Presioná `F5` o clic en `IIS Express` para ejecutar.

### Ejecutar desde CLI

```bash
# Restaurar dependencias
dotnet restore

# Compilar
dotnet build

# Ejecutar la WebAPI
dotnet run --project src/NativoChallenge.WebAPI
```

La API estará en `https://localhost:5001` (o el puerto asignado).

---

### Ejecutar con docker

**TO DO: esta parte no esta del todo bien documentada falto probar(bien) al igual que corriendo Docker desde visual**

1. Asegúrate de tener Docker instalado y en ejecución en tu máquina.
2. Desde la raíz del proyecto, ejecuta los siguientes comandos:
```bash
### Construir la imagen Docker
docker build -t nativochallenge-api -f src/NativoChallenge.WebAPI/Dockerfile .
### Ejecutar el contenedor
docker run -d -p 5001:8080 --name nativochallenge-api nativochallenge-api

```
- La API estará disponible en `https://localhost:5001` (o el puerto que hayas mapeado).
- Si necesitas personalizar variables de entorno o la conexión a la base de datos, puedes agregar los parámetros necesarios al comando `docker run`.

> **Nota:** Si deseas usar SQL Server en vez de la base en memoria, asegurate de configurar correctamente la cadena de conexión y exponer el puerto correspondiente.

---

### 🧱 Migraciones y persistencia con EF Core

Por defecto, el sistema utiliza una base de datos **en memoria** (`InMemoryDatabase`) para facilitar las pruebas rápidas y el testing sin necesidad de un servidor externo.  
Si deseás usar una base de datos **persistente** (como SQL Server), seguí los siguientes pasos:

#### 1️⃣ Configurar la base de datos

Editá el archivo `appsettings.json` para desactivar la base en memoria:

```json
"UseInMemoryDb": true // o false si querés usar SQL Server
```

Si `false`(Persistir en SqlServer), agregá tu connection string en:

```json
"UseInMemoryDb": false,
"ConnectionStrings": {
  "NativoConnectionString": "Server=(localdb)\\MSSQLLocalDB;Database=NativoChallengeDb;Trusted_Connection=True;"
}
```
#### 2️⃣ Ejecutar migraciones

Desde la consola, dentro de la carpeta `NativoChallenge.WebAPI` (Startup Project):

```bash
# Update de la migracion del proyecto de infrastructura

dotnet ef migrations add InitialCreate --project ../NativoChallenge.Infrastructure --startup-project .
dotnet ef database update --project ../NativoChallenge.Infrastructure --startup-project .
```
> Asegurate de que el proyecto `NativoChallenge.WebAPI` esté seteado como `Startup Project` y que tenga acceso al archivo `appsettings.json`.

## 🧪 Tests

```bash
# Ejecutar tests unitarios
dotnet test tests/NativoChallenge.UnitTests

# Ejecutar tests de integración
dotnet test tests/NativoChallenge.IntegrationTests
```

> Tambien existen TestCases de endopints en el `TestCases/NativoChallenge.WebAPI.http` file del proyecto NativoChallenge.WebAPI. Ideal para probar todos los endpoints de forma rápida.

### 📚 Endpoints principales

| Método | Endpoint                | Descripción                       |
|--------|------------------------|-----------------------------------|
| GET    | /tasks                 | Listar todas las tareas           |
| GET    | /tasks?State=Pending   | Listar tareas pendientes          |
| GET    | /tasks?OrderBy=Priority| Listar tareas ordenadas por prioridad |
| POST   | /tasks                 | Crear nueva tarea                 |
| PUT    | /tasks/{id}/complete   | Marcar tarea como completada      |
| DELETE | /tasks/{id}            | Eliminar tarea                    |


---

## 🧾 Swagger y documentación

La API tiene Swagger generado automáticamente:

- Accedé a `/swagger` al iniciar la app.
- Documentación y prueba interactiva.

---

## 📦 Semilla de datos

- Se incluyen datos semilla en memoria (`SeedData.cs`) si activás In-Memory en `appsettings.json`.
- Podés comentar la línea `SeedData.InitializeAsync()` si no querés usarlo.

---

##  🔍 Funcionalidades opcionales implementadas
A pesar de que eran opcionales, se implementaron varias funcionalidades adicionales para enriquecer el proyecto y demostrar mayor dominio técnico:
- Persistencia de datos con Entity Framework Core y opción de base de datos en memoria o SQL Server (configurable desde appsettings.json).
- Swagger/OpenAPI para documentación de endpoints.
- Validaciones centralizadas con FluentValidation y comportamiento global con MediatR Pipeline Behaviors.
- Inyección de dependencias extensible con métodos como AddApplication(), AddInfrastructure() y UseTaskEndpoints().
- Semillas de datos (SeedData) para pruebas e integración rápida.
- Separación clara de responsabilidades con DTOs, Results, y uso de proyecciones LINQ.
- Mensajes de advertencia de negocio retornados en los results, listos para usarse en interfaces.

---

## 🛠️️ Mejoras futuras o no implementadas por tiempo
Aunque se entregó una solución robusta, hay mejoras que podrían agregarse si se contara con más tiempo:

- 🔒  Autenticación y autorización de usuarios.
- 🧪  Más tests unitarios e integración completos, especialmente sobre endpoints.
- 📦  Servicios de dominio para encapsular lógica compleja fuera de los Handlers.
- 📨  Integración con servicios externos (envío de emails, logs avanzados).
- 📈  Observabilidad y métricas usando OpenTelemetry.
- 🧠  DDD avanzado: Modelado de Aggregates, ValueObjects, y separación por Bounded Contexts.
- 🐳  Dockerización del entorno (el dockerfile esta creado, faltaría documentar y agregar el docker-compose.yml).
- ☁️  CI/CD e infraestructura cloud (GitHub Actions, Azure, etc).

---

## 🧠 Alternativa simplificada (no implementada)

Si quisiera hacer una solución más simple, hubiera creado:

- Dominio: entidad + lógica + interfaz de repositorio
- Aplicación: implementación de repositorio + servicio
- Infraestructura: EF Core (persistencia) + configuración
- WebAPI: injectar servicios directamente en endpoints
- Tests: unitarios a dominio/servicio y  de integracion a endpoints

Pero opté por mostrar buenas prácticas aplicando CQRS, patrones y Clean Architecture, pensando en escalabilidad y mantenibilidad futura.

---
¡Gracias por leer! 🚀