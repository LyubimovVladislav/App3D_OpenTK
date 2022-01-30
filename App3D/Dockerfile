FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["App3D/App3D.csproj", "App3D/"]
RUN dotnet restore "App3D/App3D.csproj"
COPY . .
WORKDIR "/src/App3D"
RUN dotnet build "App3D.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "App3D.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "App3D.dll"]
