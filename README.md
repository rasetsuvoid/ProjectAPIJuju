# 📌 Juju API - Refactor y Mejora de Arquitectura

Este proyecto fue refactorizado y modernizado desde .NET Core 2.1 a .NET 9, siguiendo principios de arquitectura limpia y aplicando buenas prácticas de desarrollo.

## 🚀 Cambios principales en el refactor
### 1. Migración tecnológica
* Actualización de **.NET Core 2.1** a **.NET 9**.
* Ajuste de dependencias y paquetes NuGet para compatibilidad con la nueva versión.

### 2. Arquitectura
* Implementación de Arquitectura Limpia (Clean Architecture) con separación en capas:
* API → Controladores, Middlewares, configuración de servicios.
* Application → Lógica de negocio, validaciones, contratos e interfaces.
* Domain → Entidades y reglas del dominio.
* Infrastructure → Acceso a datos y persistencia.

#### Uso de patrones de diseño:

* Repository Pattern → Para encapsular el acceso a datos.
* Unit of Work → Para un manejo eficiente de transacciones.

### 3. Librerías y herramientas
* AutoMapper → Para mapeo automático entre entidades y DTOs.
* FluentValidation → Para validación declarativa y centralizada de modelos.
* Serilog → Mejorado para registrar eventos y errores en base de datos, con configuración flexible desde appsettings.json.

### 4. Manejo de errores
* Creación de un Middleware de Excepciones para capturar y procesar errores controladamente, devolviendo respuestas consistentes a la API.

### 5. Auditoría y control de datos
* Se agregaron campos de auditoría a las entidades (Active, IsDeleted, CreatedDate, UpdatedDate).
* Implementación de soft delete → Los registros no se eliminan físicamente, sino que pasan a un estado Inactivo para mantener historial y trazabilidad.

## 📂 Estructura del proyecto

API/                 # Capa de presentación (controladores, middleware, configuración)

Application/         # Casos de uso, contratos, DTOs, validaciones

Domain/              # Entidades y reglas de negocio

Infrastructure/      # Persistencia, repositorios, Unit of Work



## 🔧 Tecnologías y dependencias principales
* .NET 9
* AutoMapper
* FluentValidation
* Serilog (con integración a SQL Server)
* Entity Framework Core
* Patrón Repository / Unit of Work

## 📈 Beneficios del refactor

* Código más modular, mantenible y escalable.
* Validaciones centralizadas y fáciles de extender.
* Mejor manejo de excepciones y trazabilidad de errores.
* Arquitectura preparada para nuevos casos de uso y fácil adaptación a cambios futuros.
* Registros seguros gracias a la implementación de auditoría y soft delete.
