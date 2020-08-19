FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY ["ApiGateways/AdminPortal.Bff.Api/AdminPortal.Bff.Api.csproj", "ApiGateways/AdminPortal.Bff.Api/"]

RUN dotnet restore "ApiGateways/AdminPortal.Bff.Api/AdminPortal.Bff.Api.csproj"
COPY . .
WORKDIR "/src/ApiGateways/AdminPortal.Bff.Api"
RUN dotnet build "AdminPortal.Bff.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AdminPortal.Bff.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Mcs.Invoicing.AdminPortal.Bff.Api.dll", "--environment=__ENV__"]