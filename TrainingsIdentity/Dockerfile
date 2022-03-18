FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Infrastructure/Infrastructure.csproj", "Infrastructure/"]
COPY ["EmailService/EmailService.csproj", "EmailService/"]
COPY ["TrainingsIdentity/TrainingsIdentity.csproj", "TrainingsIdentity/"]

RUN dotnet restore "TrainingsIdentity/TrainingsIdentity.csproj"
COPY . .

RUN openssl req -x509 -newkey rsa:4096 -sha256 -nodes -days 3650 -keyout Certs/identityCertificate.key -out Certs/identityCertificate.crt -config Certs/opensslIdentity.config
RUN openssl pkcs12 -export -inkey Certs/identityCertificate.key -in Certs/identityCertificate.crt -out Certs/identityCertificate.pfx -password pass:H)h9j34jf:,hei/HI746hgGF78GJb
WORKDIR "/src/TrainingsIdentity"

RUN dotnet build "TrainingsIdentity.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TrainingsIdentity.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
RUN ls
COPY /Certs/identityCertificate.pfx . 
ENTRYPOINT ["dotnet", "TrainingsIdentity.dll"]
