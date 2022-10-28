#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["MainAPI/MainAPI.csproj", "MainAPI/"]
RUN dotnet restore "MainAPI/MainAPI.csproj"
COPY . .
WORKDIR "/src/MainAPI"
RUN dotnet build "MainAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MainAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MainAPI.dll"]