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
COPY ["ASP.NETCoreWebApplication/ASP.NETCoreWebApplication.csproj", "ASP.NETCoreWebApplication/"]

RUN dotnet restore "ASP.NETCoreWebApplication/ASP.NETCoreWebApplication.csproj"
COPY . .

WORKDIR "/src/ASP.NETCoreWebApplication"

RUN dotnet build "ASP.NETCoreWebApplication.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ASP.NETCoreWebApplication.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ASP.NETCoreWebApplication.dll"]

