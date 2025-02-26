# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

# Create and set permissions for the uploads directory
RUN mkdir -p /app/wwwroot/uploads && chmod -R 777 /app/wwwroot/uploads

EXPOSE 8080
EXPOSE 8081

# Install required packages and vsdbg in base stage
RUN apt-get update && apt-get install -y --no-install-recommends unzip curl ca-certificates \
    && rm -rf /var/lib/apt/lists/* \
    && curl -sSL https://aka.ms/getvsdbgsh | bash /dev/stdin -v latest -l /vsdbg \
    && chmod +x /vsdbg/vsdbg

# Create the uploads directory with proper permissions
RUN mkdir -p /app/wwwroot/uploads && chmod -R 777 /app/wwwroot/uploads

# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Debug
WORKDIR /src
COPY ["CTFApp/CTFApp.csproj", "CTFApp/"]
COPY ["CTFApp.DataAccess/CTFApp.DataAccess.csproj", "CTFApp.DataAccess/"]
COPY ["CTFApp.Models/CTFApp.Models.csproj", "CTFApp.Models/"]
RUN dotnet restore "CTFApp/CTFApp.csproj"
COPY . .
WORKDIR "/src/CTFApp"
RUN dotnet build "CTFApp.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Debug
RUN dotnet publish "CTFApp.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
# Copy the published app
COPY --from=publish /app/publish .
# Copy the vsdbg folder from the base stage
COPY --from=base /vsdbg /vsdbg
ENTRYPOINT ["dotnet", "CTFApp.dll"]