# Architecture Documentation

This directory contains high-level architectural decisions, roadmaps, and design documents for the Google Maps API .NET library.

## 📋 Documents

### [Modernization Roadmap](./MODERNIZATION_ROADMAP.md)
Comprehensive modernization strategy covering:
- Priority-based implementation plan
- System.Text.Json migration status
- HTTP client modernization
- Nullable reference types implementation
- Future enhancement roadmap

## 🎯 Architecture Principles

### Design Patterns
- **Facade Pattern**: Clean API surface with `GoogleMaps.cs`
- **Generic Constraints**: Type-safe request/response handling
- **Composition over Inheritance**: Modular, reusable components

### Multi-Framework Support
- .NET 8.0, 6.0, Standard 2.0
- .NET Framework 4.81, 4.62
- Consistent API across all targets

### Modern C# Features
- Nullable reference types (in progress)
- System.Text.Json serialization
- Async/await patterns
- Source generators (future)

## 🔄 Decision Records

Major architectural decisions are documented here to maintain context and rationale for future developers.

## 📈 Evolution

Architecture documents should be updated as the system evolves to reflect current state and future direction.
