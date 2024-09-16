
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
EXPOSE 5400

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

WORKDIR /src
COPY ["sw/sw.auth.messaging/", "sw/sw.auth.messaging/"]
COPY ["sw/sw.azure.messaging/", "sw/sw.azure.messaging/"]
COPY ["sw/sw.infrastructure/", "sw/sw.infrastructure/"]
COPY ["admin/api/sw.admin.api/", "/src/admin/api/sw.admin.api"]
COPY ["admin/common/sw.admin.common.dtos/", "/src/admin/common/sw.admin.common.dtos"]
COPY ["admin/domain/sw.admin.model/", "/src/admin/domain/sw.admin.model"]
COPY ["admin/domain/sw.admin.contracts/", "/src/admin/domain/sw.admin.contracts"]
COPY ["admin/domain/sw.admin.repository/", "/src/admin/domain/sw.admin.repository"]
COPY ["admin/domain/sw.admin.services/", "/src/admin/domain/sw.admin.services"]
WORKDIR /src/admin/api/sw.admin.api
RUN dotnet restore "sw.admin.api.csproj"

RUN dotnet publish -c release -o /app --no-cache
FROM build AS publish
#Final
FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "sw.admin.api.dll"]