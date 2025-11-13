# Use the official .NET 8 runtime as base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Use the official .NET 8 SDK for building
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy project files
COPY ["src/BookLendingApplication.csproj", "src/"]
RUN dotnet restore "src/BookLendingApplication.csproj"

# Copy source code
COPY . .
WORKDIR "/src/src"
RUN dotnet build "BookLendingApplication.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "BookLendingApplication.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Final stage
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BookLendingApplication.dll"]