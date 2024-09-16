FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
EXPOSE 5700

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

WORKDIR /src
COPY ["sw/sw.azure.messaging/", "sw/sw.azure.messaging/"]
COPY ["sw/sw.infrastructure/", "sw/sw.infrastructure/"]
COPY ["logging/api/sw.logging.api/", "/src/logging/api/sw.logging.api"]
WORKDIR /src/logging/api/sw.logging.api
RUN dotnet restore "sw.logging.api.csproj"

RUN dotnet publish -c release -o /app --no-cache
FROM build AS publish
#Final
FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "sw.logging.api.dll"]