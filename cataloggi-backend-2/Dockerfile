# syntax=docker/dockerfile:1

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["cataloggi-backend-2.csproj", "./"]
RUN dotnet restore "cataloggi-backend-2.csproj"

COPY . .
RUN dotnet publish "cataloggi-backend-2.csproj" \
    --configuration Release \
    --output /app/publish \
    /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

ENV ASPNETCORE_ENVIRONMENT=Production \
    DOTNET_EnableDiagnostics=0

COPY --from=build /app/publish .

EXPOSE 8080

ENTRYPOINT ["sh", "-c", "dotnet cataloggi-backend-2.dll --urls http://0.0.0.0:${PORT:-8080}"]
