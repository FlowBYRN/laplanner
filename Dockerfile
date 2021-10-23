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
#Create Certificate
RUN openssl req -x509 -newkey rsa:4096 -sha256 -nodes -days 3650 -keyout Certs/trainingsCertificate.key -out Certs/trainingsCertificate.crt -config Certs/openssl.config
RUN openssl pkcs12 -export -inkey Certs/trainingsCertificate.key -in Certs/trainingsCertificate.crt -out Certs/trainingsCertificate.pfx -password pass:Thisisapassword_123
WORKDIR "/src/TrainingsIdentity"




RUN dotnet build "TrainingsIdentity.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TrainingsIdentity.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY /Certs/trainingsCertificate.pfx Certs/ 
RUN ls
ENTRYPOINT ["dotnet", "TrainingsIdentity.dll"]

