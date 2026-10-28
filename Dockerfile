# Stage 1: build with the full SDK
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy only the project file first so the restore layer is cached
COPY *.csproj ./
RUN dotnet restore

COPY . .
RUN dotnet publish -c Release -o /app --no-restore

# Stage 2: small runtime image, no SDK and no source code
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app .

EXPOSE 8080
USER $APP_UID
ENTRYPOINT ["dotnet", "MyApi.dll"]