# 1. IMAGEN BASE
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# --- CAMBIO IMPORTANTE AQUÍ ---
# Como el csproj está en la raíz, lo copiamos directamente sin buscar carpetas extra
COPY ["Asilo.Core.csproj", "./"]
RUN dotnet restore "Asilo.Core.csproj"

# Copiamos todo lo demás
COPY . .
WORKDIR "/src/."
RUN dotnet build "Asilo.Core.csproj" -c Release -o /app/build

# 2. PUBLICAR
FROM build AS publish
RUN dotnet publish "Asilo.Core.csproj" -c Release -o /app/publish

# 3. IMAGEN FINAL
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

# PUERTO PARA RENDER
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "Asilo.Core.dll"]