FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build

# Install Node.js
RUN curl -fsSL https://deb.nodesource.com/setup_14.x | bash - \
    && apt-get install -y \
        nodejs \
    && rm -rf /var/lib/apt/lists/*

WORKDIR /src
RUN dir
COPY ["TrainingsPlanner/TrainingsPlanner.csproj", "TrainingsPlanner/"]
COPY ["Infrastructure/Infrastructure.csproj", "Infrastructure/"]

RUN dotnet restore "TrainingsPlanner/TrainingsPlanner.csproj"
COPY . .

RUN openssl req -x509 -newkey rsa:4096 -sha256 -nodes -days 3650 -keyout Certs/trainingsCertificate.key -out Certs/trainingsCertificate.crt -config Certs/opensslTraining.config
RUN openssl pkcs12 -export -inkey Certs/trainingsCertificate.key -in Certs/trainingsCertificate.crt -out Certs/trainingsCertificate.pfx -password pass:Thisisapassword_123

WORKDIR "/src/TrainingsPlanner"

RUN dotnet build "TrainingsPlanner.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TrainingsPlanner.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY /Certs/trainingsCertificate.pfx . 
ENTRYPOINT ["dotnet", "TrainingsPlanner.dll"]

#https://mjarosie.github.io/dev/2020/09/24/running-identityserver4-on-docker-with-https.html