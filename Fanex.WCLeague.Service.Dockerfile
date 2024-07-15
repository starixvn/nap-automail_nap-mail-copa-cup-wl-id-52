FROM hub.nexdev.net/base/dotnet/core/sdk:5.0.102-ca-patch-buster-slim AS builder
WORKDIR /src

COPY ./Fanex.WCLeague.Service/Fanex.WCLeague.Business ./Fanex.WCLeague.Business
COPY ./Fanex.WCLeague.Service/Fanex.WCLeague.Service/Fanex.WCLeague.Service.csproj ./Fanex.WCLeague.Service/Fanex.WCLeague.Service.csproj

RUN ls /src -l
WORKDIR /src/Fanex.WCLeague.Service
RUN dotnet nuget add source https://nuget.nexdev.net/api/v2
RUN dotnet restore -v n

COPY ./Fanex.WCLeague.Service/Fanex.WCLeague.Service .

RUN dotnet publish -r linux-x64 --self-contained false -c Release -o /app

FROM hub.nexdev.net/base/dotnet/core/aspnet:5.0-nap-mail-core

RUN sed -i 's/DEFAULT@SECLEVEL=2/DEFAULT@SECLEVEL=1/g' /etc/ssl/openssl.cnf
RUN sed -i 's/MinProtocol = TLSv1.2/MinProtocol = TLSv1/g' /etc/ssl/openssl.cnf
RUN sed -i 's/DEFAULT@SECLEVEL=2/DEFAULT@SECLEVEL=1/g' /usr/lib/ssl/openssl.cnf
RUN sed -i 's/MinProtocol = TLSv1.2/MinProtocol = TLSv1/g' /usr/lib/ssl/openssl.cnf

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
