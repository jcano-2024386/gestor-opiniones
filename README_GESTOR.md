GestorOpiniones API (Minimal)

Run locally:

1. Build

```powershell
dotnet build src\GestorOpiniones.Api\GestorOpiniones.Api.csproj
```

2. Run

```powershell
dotnet run --project src\GestorOpiniones.Api\GestorOpiniones.Api.csproj
```

Defaults:
- MongoDB: mongodb://localhost:27017
- DB name: gestor_opiniones
- Update `appsettings.json` to set a strong `JwtSettings:SecretKey` before production.
