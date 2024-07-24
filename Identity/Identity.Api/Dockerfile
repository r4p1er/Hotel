FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env

WORKDIR /src
COPY . .
RUN dotnet restore 
RUN dotnet publish -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:8.0

WORKDIR /app
COPY --from=build-env /app .
EXPOSE 8080
ENTRYPOINT ["dotnet", "Identity.Api.dll"]