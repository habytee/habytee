FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["habytee.Interconnection/habytee.Interconnection.csproj", "habytee.Interconnection/"]
RUN dotnet restore "habytee.Interconnection/habytee.Interconnection.csproj"

COPY ["habytee.Client/habytee.Client.csproj", "habytee.Client/"]
RUN dotnet restore "habytee.Client/habytee.Client.csproj"

COPY . .

#RUN dotnet workload install wasm-tools

#WORKDIR "/src/habytee.Interconnection"
#RUN dotnet build "habytee.Interconnection.csproj" -c Release -o /app/build

#WORKDIR "/src/habytee.Client"
#RUN dotnet build "habytee.Client.csproj" -c Release -o /app/build

WORKDIR "/src/habytee.Interconnection"
RUN dotnet publish "habytee.Interconnection.csproj" -c Release -o /app/publish

WORKDIR "/src/habytee.Client"
RUN dotnet publish "habytee.Client.csproj" -c Release -o /app/publish

FROM nginx:alpine AS final

COPY --from=build /app/publish/wwwroot /usr/share/nginx/html

CMD ["nginx", "-g", "daemon off;"]
