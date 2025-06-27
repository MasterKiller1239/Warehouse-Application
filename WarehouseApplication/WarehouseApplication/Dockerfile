# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
WORKDIR /app
EXPOSE 5000
EXPOSE 5001
COPY ["WarehouseApplication.csproj", "."]
RUN dotnet restore "WarehouseApplication.csproj"
COPY . .
RUN dotnet build "WarehouseApplication.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "WarehouseApplication.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
RUN apt-get update && apt-get install -y net-tools
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WarehouseApplication.dll"]