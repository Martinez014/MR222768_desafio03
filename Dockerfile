FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["src/SistemaPedidos.Domain/SistemaPedidos.Domain.csproj", "src/SistemaPedidos.Domain/"]
COPY ["src/SistemaPedidos.Application/SistemaPedidos.Application.csproj", "src/SistemaPedidos.Application/"]
COPY ["src/SistemaPedidos.Infrastructure/SistemaPedidos.Infrastructure.csproj", "src/SistemaPedidos.Infrastructure/"]
COPY ["src/SistemaPedidos.API/SistemaPedidos.API.csproj", "src/SistemaPedidos.API/"]
RUN dotnet restore "src/SistemaPedidos.API/SistemaPedidos.API.csproj"

COPY . .
RUN dotnet publish "src/SistemaPedidos.API/SistemaPedidos.API.csproj" \
    --configuration Release \
    --no-restore \
    --output /app/publish \
    /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "SistemaPedidos.API.dll"]
