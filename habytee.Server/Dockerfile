FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

COPY ["habytee.Interconnection/habytee.Interconnection.csproj", "habytee.Interconnection/"]
RUN dotnet restore "habytee.Interconnection/habytee.Interconnection.csproj"

COPY ["habytee.Server/habytee.Server.csproj", "habytee.Server/"]
RUN dotnet restore "habytee.Server/habytee.Server.csproj"

COPY . .

RUN dotnet publish ./habytee.Server/habytee.Server.csproj -c Release -o /api/publish


FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS final

WORKDIR /api
COPY --from=build /api/publish .

ENTRYPOINT ["dotnet", "habytee.Server.dll"]
