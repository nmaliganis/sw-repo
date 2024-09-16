
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
EXPOSE 5300

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

WORKDIR /src
COPY ["sw/sw.auth.messaging/", "sw/sw.auth.messaging/"]
COPY ["sw/sw.azure.messaging/", "sw/sw.azure.messaging/"]
COPY ["sw/sw.infrastructure/", "sw/sw.infrastructure/"]
COPY ["asset/api/sw.asset.api/", "/src/asset/api/sw.asset.api"]
COPY ["asset/common/sw.asset.common.dtos/", "/src/asset/common/sw.asset.common.dtos"]
COPY ["asset/common/sw.asset.common.infrastructure/", "/src/asset/common/sw.asset.common.infrastructure"]
COPY ["asset/domain/sw.asset.model/", "/src/asset/domain/sw.asset.model"]
COPY ["asset/domain/sw.asset.contracts/", "/src/asset/domain/sw.asset.contracts"]
COPY ["asset/domain/sw.asset.repository/", "/src/asset/domain/sw.asset.repository"]
COPY ["asset/domain/sw.asset.services/", "/src/asset/domain/sw.asset.services"]
WORKDIR /src/asset/api/sw.asset.api
RUN dotnet restore "sw.asset.api.csproj"

RUN dotnet publish -c release -o /app --no-cache
FROM build AS publish
#Final
FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "sw.asset.api.dll"]