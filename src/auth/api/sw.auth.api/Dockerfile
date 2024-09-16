
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
EXPOSE 5200

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

WORKDIR /src
COPY ["sw/sw.azure.messaging/", "sw/sw.azure.messaging/"]
COPY ["sw/sw.infrastructure/", "sw/sw.infrastructure/"]
COPY ["auth/api/sw.auth.api/", "/src/auth/api/sw.auth.api"]
COPY ["auth/common/sw.auth.common.dtos/", "/src/auth/common/sw.auth.common.dtos"]
COPY ["auth/common/sw.auth.common.infrastructure/", "/src/auth/common/sw.auth.common.infrastructure"]
COPY ["auth/domain/sw.auth.model/", "/src/auth/domain/sw.auth.model"]
COPY ["auth/domain/sw.auth.contracts/", "/src/auth/domain/sw.auth.contracts"]
COPY ["auth/domain/sw.auth.repository/", "/src/auth/domain/sw.auth.repository"]
COPY ["auth/domain/sw.auth.services/", "/src/auth/domain/sw.auth.services"]
WORKDIR /src/auth/api/sw.auth.api
RUN dotnet restore "sw.auth.api.csproj"

RUN dotnet publish -c release -o /app --no-cache
FROM build AS publish
#Final
FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "sw.auth.api.dll"]