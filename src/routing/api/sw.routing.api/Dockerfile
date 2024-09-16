
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
EXPOSE 5600

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

WORKDIR /src
COPY ["sw/sw.azure.messaging/", "sw/sw.azure.messaging/"]
COPY ["sw/sw.infrastructure/", "sw/sw.infrastructure/"]
COPY ["routing/api/sw.routing.api/", "/src/routing/api/sw.routing.api"]
COPY ["routing/common/sw.routing.common.dtos/", "/src/routing/common/sw.routing.common.dtos"]
COPY ["routing/common/sw.routing.common.infrastructure/", "/src/routing/common/sw.routing.common.infrastructure"]
COPY ["routing/domain/sw.routing.model/", "/src/routing/domain/sw.routing.model"]
COPY ["routing/domain/sw.routing.contracts/", "/src/routing/domain/sw.routing.contracts"]
COPY ["routing/domain/sw.routing.repository/", "/src/routing/domain/sw.routing.repository"]
COPY ["routing/domain/sw.routing.services/", "/src/routing/domain/sw.routing.services"]
WORKDIR /src/routing/api/sw.routing.api
RUN dotnet restore "sw.routing.api.csproj"

RUN dotnet publish -c release -o /app --no-cache

FROM build AS publish
#Final
FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "sw.routing.api.dll"]