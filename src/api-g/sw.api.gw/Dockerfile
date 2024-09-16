FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

WORKDIR /src
COPY ["api-g/sw.api.gw/", "/src/api-g/sw.api.gw"]
WORKDIR /src/api-g/sw.api.gw
RUN dotnet restore "sw.api.gw.csproj"

RUN dotnet publish -c release -o /app --no-cache
FROM build AS publish
#Final
FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "sw.api.gw.dll"]