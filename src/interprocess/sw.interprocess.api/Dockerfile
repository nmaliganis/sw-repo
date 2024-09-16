
FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
EXPOSE 5500
EXPOSE 8883
EXPOSE 1883
EXPOSE 6001
EXPOSE 6002

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build

WORKDIR /src
COPY ["sw/sw.azure.messaging/", "sw/sw.azure.messaging/"]
COPY ["interprocess/sw.interprocess.api/", "/src/interprocess/sw.interprocess.api/"]
WORKDIR /src/interprocess/sw.interprocess.api
RUN dotnet restore "sw.interprocess.api.csproj"

RUN dotnet publish -c release -o /app --no-cache
FROM build AS publish
#Final
FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "sw.interprocess.api.dll"]