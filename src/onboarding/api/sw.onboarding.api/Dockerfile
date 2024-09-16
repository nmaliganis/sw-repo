
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
EXPOSE 5700

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

WORKDIR /src
COPY ["sw/sw.auth.messaging/", "sw/sw.auth.messaging/"]
COPY ["sw/sw.azure.messaging/", "sw/sw.azure.messaging/"]
COPY ["sw/sw.infrastructure/", "sw/sw.infrastructure/"]
COPY ["onboarding/api/sw.onboarding.api/", "/src/onboarding/api/sw.onboarding.api"]
COPY ["onboarding/common/sw.onboarding.common.dtos/", "/src/onboarding/common/sw.onboarding.common.dtos"]
COPY ["onboarding/common/sw.onboarding.common.infrastructure/", "/src/onboarding/common/sw.onboarding.common.infrastructure"]
COPY ["onboarding/domain/sw.onboarding.model/", "/src/onboarding/domain/sw.onboarding.model"]
COPY ["onboarding/domain/sw.onboarding.contracts/", "/src/onboarding/domain/sw.onboarding.contracts"]
COPY ["onboarding/domain/sw.onboarding.repository/", "/src/onboarding/domain/sw.onboarding.repository"]
COPY ["onboarding/domain/sw.onboarding.services/", "/src/onboarding/domain/sw.onboarding.services"]
WORKDIR /src/onboarding/api/sw.onboarding.api
RUN dotnet restore "sw.onboarding.api.csproj"

RUN dotnet publish -c release -o /app --no-cache
FROM build AS publish
#Final
FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "sw.onboarding.api.dll"]