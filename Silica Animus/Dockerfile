FROM microsoft/aspnetcore:2.0 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2.0 AS build
WORKDIR /src
COPY Silica Animus/Silica Animus.csproj Silica Animus/
RUN dotnet restore Silica Animus/Silica Animus.csproj
COPY . .
WORKDIR /src/Silica Animus
RUN dotnet build Silica Animus.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish Silica Animus.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Silica Animus.dll"]
