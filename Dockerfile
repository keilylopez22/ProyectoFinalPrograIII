# Usar la imagen oficial de .NET SDK para construir la aplicación
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copiar los archivos del proyecto y restaurar dependencias
COPY *.csproj ./
RUN dotnet restore

# Copiar el resto del código fuente y compilar la aplicación
COPY . ./
RUN dotnet publish -c Release -o /out

# Usar la imagen oficial de .NET Runtime para ejecutar la app
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Copiar los archivos compilados desde la etapa anterior
COPY --from=build /out .

# Definir el puerto en el que se ejecutará la aplicación
EXPOSE 5000
EXPOSE 5001

# Configurar la API para que escuche en cualquier dirección
ENV ASPNETCORE_URLS=http://+:5000

# Comando para ejecutar la aplicación
ENTRYPOINT ["dotnet", "ApiECommerce.dll"]
