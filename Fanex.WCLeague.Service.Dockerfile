FROM hub.nexdev.net/base/dotnet/core/sdk:9.0-alpine AS builder
WORKDIR /src

COPY ./Fanex.WCLeague.Service/Fanex.WCLeague.Business ./Fanex.WCLeague.Business
COPY ./Fanex.WCLeague.Service/Fanex.WCLeague.Service/Fanex.WCLeague.Service.csproj ./Fanex.WCLeague.Service/Fanex.WCLeague.Service.csproj

WORKDIR /src/Fanex.WCLeague.Service
RUN dotnet nuget add source https://nuget.nexdev.net/api/v2
RUN --mount=type=cache,target=/root/.nuget/packages \
	dotnet restore -v n -r linux-x64

COPY ./Fanex.WCLeague.Service/Fanex.WCLeague.Service .

RUN --mount=type=cache,target=/root/.nuget/packages \
	dotnet publish -r linux-x64 --self-contained false -c Release -o /app

FROM hub.nexdev.net/base/dotnet/core/aspnet:9.0-nap-mail-core

# # Don’t run as root
# RUN adduser --disabled-password \
#   --home /app \
#   --gecos '' dotnetuser && chown -R dotnetuser /app
# USER dotnetuser
# #

ENV TZ="America/La_Paz" 

WORKDIR /app
COPY --from=builder /app .

ENTRYPOINT ["dotnet", "Fanex.WCLeague.Service.dll"]
