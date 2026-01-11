# USAR LA IMAGEN DE MICROSOFT .NET 8 SDK
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# COPIAR LOS ARCHIVOS DE PROYECTO (Ajusta si tienes otros proyectos referenciados)
COPY ["Asilo.Core/Asilo.Core.csproj", "Asilo.Core/"]
# Si tienes un proyecto de interfaces, descomenta la siguiente línea:
# COPY ["Asilo.Core.Interfaces/Asilo.Core.Interfaces.csproj", "Asilo.Core.Interfaces/"]

RUN dotnet restore "Asilo.Core/Asilo.Core.csproj"

# COPIAR EL RESTO DEL CÓDIGO Y COMPILAR
COPY . .
WORKDIR "/src/Asilo.Core"
RUN dotnet build "Asilo.Core.csproj" -c Release -o /app/build

# PUBLICAR LA APP
FROM build AS publish
RUN dotnet publish "Asilo.Core.csproj" -c Release -o /app/publish

# CONFIGURAR LA IMAGEN FINAL
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

# ESTA LÍNEA ES CRÍTICA PARA RENDER (Usa el puerto dinámico)
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "Asilo.Core.dll"]