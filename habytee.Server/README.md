# V. Server
The landing page after searching on google.

## 1. Build

```bash
docker build -f Dockerfile -t habytee/server:latest ../
```

## 2. Push

```bash
docker push habytee/server:latest
```


## Initial Migration
```bash
dotnet ef migrations add InitialCreate --context WriteDbContext
```
