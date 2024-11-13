#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["src/UserService.API/UserService.API.csproj", "./UserService.API/"]
COPY ["src/UserService.Domain/UserService.Domain.csproj", "./UserService.Domain/"]
COPY ["src/UserService.Infrastructure/UserService.Infrastructure.csproj", "./UserService.Infrastructure/"]
RUN dotnet restore "./UserService.API/UserService.API.csproj"

COPY "./src" .
WORKDIR "/src"


RUN dotnet build "UserService.API/UserService.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "UserService.API/UserService.API.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 36801

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UserService.API.dll"]