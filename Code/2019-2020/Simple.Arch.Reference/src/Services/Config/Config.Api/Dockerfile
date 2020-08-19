#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Services/Config/Config.Api/Config.Api.csproj", "Services/Config/Config.Api/"]
COPY ["Services/Config/Config.Infrastructure/Config.Infrastructure.csproj", "Services/Config/Config.Infrastructure/"]
COPY ["Services/Config/Config.Application/Config.Application.csproj", "Services/Config/Config.Application/"]
COPY ["Services/Audit/Audit.Client.Sdk/Audit.Client.Sdk.csproj", "Services/Audit/Audit.Client.Sdk/"]
COPY ["Core/Core.Framework/Core.Framework.csproj", "Core/Core.Framework/"]
COPY ["Services/Audit/Audit.Messaging.Contracts/Audit.Messaging.Contracts.csproj", "Services/Audit/Audit.Messaging.Contracts/"]
COPY ["Services/Config/Config.Domain/Config.Domain.csproj", "Services/Config/Config.Domain/"]
COPY ["Services/Config/Config.Messaging.Contracts/Config.Messaging.Contracts.csproj", "Services/Config/Config.Messaging.Contracts/"]
RUN dotnet restore "Services/Config/Config.Api/Config.Api.csproj"
COPY . .
WORKDIR "/src/Services/Config/Config.Api"
RUN dotnet build "Config.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Config.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Mcs.Invoicing.Services.Config.Api.dll"]