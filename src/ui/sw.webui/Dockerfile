FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
EXPOSE 5100

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["ui/sw.webui/", "/src/ui/sw.webui"]

RUN curl -sL https://deb.nodesource.com/setup_14.x |  bash -
RUN apt-get install -y nodejs

WORKDIR /src/ui/sw.webui

RUN dotnet restore "sw.webui.csproj"
RUN dotnet publish -c release -o /app --no-cache

FROM build AS publish
# final stage/image
FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "sw.webui.dll"]