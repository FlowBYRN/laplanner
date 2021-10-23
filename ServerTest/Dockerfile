FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["./ServerTest.csproj", "ServerTest/"]
RUN dotnet restore "ServerTest/ServerTest.csproj"
WORKDIR "/src/ServerTest"
COPY . .

RUN dotnet build "ServerTest.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ServerTest.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ServerTest.dll"]
