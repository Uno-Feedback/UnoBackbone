#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM repo.asax.ir/mic/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM repo.asax.ir/mic/dotnet/sdk:7.0-alpine AS build
WORKDIR /src
COPY ["Uno.Api/Uno.Api.csproj", "Uno.Api/"]
COPY ["Uno.Application/Uno.Application.csproj", "Uno.Application/"]
COPY ["Uno.Domain/Uno.Domain.csproj", "Uno.Domain/"]
COPY ["Uno.Shared/Uno.Shared.csproj", "Uno.Shared/"]
COPY ["Uno.Infrastructer.ExternalServices/Uno.Infrastructer.ExternalServices.csproj", "Uno.Infrastructer.ExternalServices/"]
COPY ["Uno.Infrastructer/Uno.Infrastructer.csproj", "Uno.Infrastructer/"]
RUN dotnet restore "Uno.Api/Uno.Api.csproj"
COPY . .
WORKDIR "/src/Uno.Api"
RUN dotnet build "Uno.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Uno.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Uno.Api.dll"]