# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and project files
COPY BookingSystemm.sln .
COPY BookingSystem.Domain/BookingSystem.Domain.csproj BookingSystem.Domain/
COPY BookingSystem.Application/BookingSystem.Application.csproj BookingSystem.Application/
COPY BookingSystem.Infrastructure/BookingSystem.Infrastructure.csproj BookingSystem.Infrastructure/
COPY BookingSystem.Web/BookingSystem.Web.csproj BookingSystem.Web/

# Copy nuget config if exists
COPY nuget.config* ./

# Restore dependencies with retries
RUN dotnet restore BookingSystemm.sln --disable-parallel || \
    (sleep 5 && dotnet restore BookingSystemm.sln --disable-parallel) || \
    (sleep 10 && dotnet restore BookingSystemm.sln --disable-parallel)

# Copy all source code
COPY . .

# Build the application
WORKDIR /src/BookingSystem.Web
RUN dotnet build -c Release -o /app/build --no-restore

# Stage 2: Publish
FROM build AS publish
RUN dotnet publish -c Release -o /app/publish --no-restore /p:UseAppHost=false

# Stage 3: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Copy published app
COPY --from=publish /app/publish .

# Set environment variables
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

ENTRYPOINT ["dotnet", "BookingSystem.Web.dll"]