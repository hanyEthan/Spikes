#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["ApiGateways/SystemApis.Bff.Api/SystemApis.Bff.Api.csproj", "ApiGateways/SystemApis.Bff.Api/"]
RUN dotnet restore "ApiGateways/SystemApis.Bff.Api/SystemApis.Bff.Api.csproj"
COPY . .
WORKDIR "/src/ApiGateways/SystemApis.Bff.Api"
RUN dotnet build "SystemApis.Bff.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SystemApis.Bff.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SystemApis.Bff.Api.dll"]