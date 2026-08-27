FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /source

COPY Stallwick.slnx ./
COPY src/Stallwick/Stallwick.csproj src/Stallwick/
COPY tests/Stallwick.Tests/Stallwick.Tests.csproj tests/Stallwick.Tests/
RUN dotnet restore src/Stallwick/Stallwick.csproj

COPY src/ src/
RUN dotnet publish src/Stallwick/Stallwick.csproj -c Release -o /app --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app ./

# SQLite lives on a volume so listings survive container restarts.
ENV ConnectionStrings__DefaultConnection="DataSource=/app/data/app.db;Cache=Shared" \
    ASPNETCORE_HTTP_PORTS=8080
RUN mkdir -p /app/data && chown -R $APP_UID:$APP_UID /app/data
VOLUME /app/data

USER $APP_UID
EXPOSE 8080
ENTRYPOINT ["dotnet", "Stallwick.dll"]
