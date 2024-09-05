FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release

WORKDIR /src

COPY ./*.props ./
COPY ./*.sln .

COPY ["src/XrayUi/XrayUi.csproj", "src/XrayUi/"]
COPY ["src/Application/XrayUi.Application/XrayUi.Application.csproj", "src/Application/XrayUi.Application/"]
COPY ["src/Application/XrayUi.Application.Contracts/XrayUi.Application.Contracts.csproj", "src/Application/XrayUi.Application.Contracts/"]
COPY ["src/Application/XrayUi.Application.Models/XrayUi.Application.Models.csproj", "src/Application/XrayUi.Application.Models/"]
COPY ["src/Application/XrayUi.Application.Abstractions/XrayUi.Application.Abstractions.csproj", "src/Application/XrayUi.Application.Abstractions/"]
COPY ["src/Infrastructure/XrayUi.Infrastructure.Persistence/XrayUi.Infrastructure.Persistence.csproj", "src/Infrastructure/XrayUi.Infrastructure.Persistence/"]
COPY ["src/Infrastructure/XrayUi.Infrastructure.Xray/XrayUi.Infrastructure.Xray.csproj", "src/Infrastructure/XrayUi.Infrastructure.Xray/"]

RUN dotnet restore "src/XrayUi/XrayUi.csproj"

COPY . .

WORKDIR "/src/src/XrayUi"
RUN dotnet build "XrayUi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release

RUN dotnet publish "XrayUi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final

WORKDIR /app

COPY --from=publish /app/publish .

RUN set -ex \
    && apt upgrade \
    && apt update \
    && apt install -y curl tzdata ca-certificates \
	&& mkdir -p /var/log/xray /usr/share/xray \
    && curl --output /usr/bin/xray https://dl.lamp.sh/files/xray_linux_amd64 \
    && chmod +x /usr/bin/xray \
	&& curl --output /usr/share/xray/geosite.dat https://github.com/v2fly/domain-list-community/releases/latest/download/dlc.dat \
	&& curl --output /usr/share/xray/geoip.dat https://github.com/v2fly/geoip/releases/latest/download/geoip.dat 

VOLUME /etc/xray

ENTRYPOINT ["dotnet", "XrayUi.dll"]
