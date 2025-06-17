# 🧩 Nativo Challenge - Gestión de Tareas (.NET 8 + Clean Architecture)

Este proyecto es una solución de ejemplo para la gestión de tareas empresariales (tipo "To-Do empresarial"), desarrollado como parte de un challenge técnico.  
Fue implementado en **.NET 8**, con un enfoque profesional basado en buenas prácticas de arquitectura y diseño de software moderno.

---

## 📐 Enfoque y arquitectura utilizada

La aplicación fue diseñada siguiendo **Clean Architecture**, lo cual aporta:

- 🔁 **Separación de responsabilidades** (_Separation of Concerns_)
- 🔧 **Mantenibilidad** y facilidad de testeo
- ⚙️ **Escalabilidad** y modularidad
- ⛓️ **Inversión de dependencias** (_Dependency Inversion Principle_)
- 🔄 **Inversión de control** (_Inversion of Control Principle_)
- 🔌 **Independencia de frameworks**
- 🧩 **Alta cohesión y bajo acoplamiento**

---

### 📚 Estructura de capas

- `Domain`: Entidades, lógica y reglas de negocio.
- `Application`: Casos de uso (CQRS), validaciones, DTOs y lógica de aplicación.
- `Infrastructure`: Acceso a datos (EF Core), configuración de dependencias.
- `WebAPI`: Endpoints HTTP con Minimal API, autenticación/autorización, Swagger, logs, trazas.
- `Tests`: Unit e Integration Tests.

---

## ✅ Funcionalidades implementadas

- Crear tarea: Título, descripción, fecha de vencimiento y prioridad (Alta, Media, Baja)
- Marcar tarea como completada
- Listar tareas por estado (pendiente o completada) y ordenadas por prioridad o fecha
- Eliminar una tarea
- Validaciones:
  - Título no vacío
  - No completar tareas vencidas
  - Advertencia si hay más de 10 tareas pendientes de alta prioridad

- Autenticación con JWT y roles
- Logs con Serilog (en consola y archivo)
- Trazas distribuidas con OpenTelemetry
- Domain Events usando MediatR (ej: al crear una tarea se simula envío de notificación por mail)

---

## 🧠 Principios y patrones aplicados

### ✔️ Principios de diseño

- **SOLID**
- **KISS** (Keep it simple, stupid)
- **DRY** (Don't repeat yourself)
- **YAGNI** (You aren’t gonna need it)
- **Separation of Concerns**
- **Inversion of Control (IoC)**
- **Dependency Inversion Principle**
- **Explicit Dependencies Principle**

### 🔁 Patrones de diseño

- **CQRS** (Command Query Responsibility Segregation)
- **Mediator** (MediatR)
- **Builder** (construcción controlada de entidades)
- **Strategy** (aplicado a filtros y ordenamiento)
- **Repository Pattern** (incluyendo un repositorio genérico)
- **Decorator** (DbContextWithEvents)
- **Factory Method / Abstract Factory** (en enfoque opcional)
- **Singleton** (Serilog global)
- **Dependency Injection** (nativa de .NET)
- **Validation Behavior** (MediatR + FluentValidation)
- **Unit of Work** (a través del contexto de EF Core)

### 🧱 Patrones de arquitectura

- **Clean Architecture**
- **Vertical Slice Architecture** (estructuración por feature dentro de `Application`)
- **Minimal API** para endpoints HTTP
- **RESTful API** (estandarización)

---

## 🔧 Configuración del entorno

### 🔸 Requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- (Opcional) SQL Server (solo si se desea persistencia)

### 🔸 Variables de configuración

Editar `appsettings.json`:

```json
"UseInMemoryDb": true,
"ConnectionStrings": {
  "NativoConnectionString": "Server=(localdb)\\MSSQLLocalDB;Database=NativoChallengeDb;Trusted_Connection=True;"
},
"Jwt": {
  "Key": "TU_SECRET_AQUI_SUPERSEGURO_DE_MAS_DE_32CHARS",
  "Issuer": "NativoAPI",
  "Audience": "NativoClient"
}
```

---

## 🚀 Levantar la aplicación

### Opción 1 - CLI
```bash
cd NativoChallenge.WebAPI
# Si usás SQL Server
DOTNET_ENVIRONMENT=Development dotnet ef database update
# Correr app
dotnet run
```

### Opción 2 - Visual Studio
- Establecer `NativoChallenge.WebAPI` como **Startup Project**
- Cambiar perfil de IIS Express a `Docker` o `Project`
- Presionar **F5**

### Opción 3 - Docker (opcional)
_TODO_

---

## 🧪 Testing

- `NativoChallenge.Tests.Unit`: Test de Application Layer + Dominio
- `NativoChallenge.Tests.Integration`: Test de endpoints + Seed DB + JWT

```bash
# Correr todos los tests
cd NativoChallenge.Tests.Unit
 dotnet test

cd ../NativoChallenge.Tests.Integration
 dotnet test
```

---

## ⚙️ Extras implementados

- Serilog: logging estructurado
- OpenTelemetry: trazas distribuidas (con `AddConsoleExporter`, extensible a Jaeger, Zipkin...)
- Swagger: documentación interactiva (`/swagger`)
- Authorization por roles y policies
- Decoradores (DbContextWithEvents)
- Domain Events (`TaskCreatedEvent`) + handler con EmailSender simulado

---

## 💡 Mejoras opcionales no implementadas por falta de tiempo

- Integración de ElasticSearch o Jaeger para trazabilidad y visualización
- Implementación de ICurrentUserService
- Registro de usuarios (`/auth/register`) y endpoint `/me`
- Uso real de un EmailSender (con SendGrid u otro)
- Auditoría: agregar `CreatedBy`, `CreatedAt`, etc.
- Integration Events + Message Broker (RabbitMQ, MassTransit...)
- Feature toggles, rate limiting y cache (ej: Redis)

---

## 💬 Notas finales

Este proyecto fue desarrollado como un ejercicio técnico, pero con un enfoque de calidad profesional, modular y extensible.

También se podría haber entregado una solución mucho más simple en 1-2 horas, basada en:

- `Domain`: clase `Task` con lógica básica
- `Application`: Servicio con inyección del repositorio
- `Infraestructura`: EF DbContext y repo
- `WebAPI`: Endpoints directos al servicio

Pero se eligió una implementación más completa como ejercicio de repaso e integración de conocimientos.

Gracias por leer 🙌
