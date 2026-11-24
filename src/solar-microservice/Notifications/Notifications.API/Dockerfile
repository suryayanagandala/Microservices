# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["BuildingBlocks/BuildingBlocks.Observability/BuildingBlocks.Observability.csproj", "BuildingBlocks/BuildingBlocks.Observability/"]
COPY ["Notifications/Notifications.API/Notifications.API.csproj", "Notifications/Notifications.API/"]
COPY ["BuildingBlocks/BuildingBloacks.Behaviour/BuildingBloacks.Behaviour.csproj", "BuildingBlocks/BuildingBloacks.Behaviour/"]
COPY ["BuildingBlocks/BuildingBlocks.Authentication,/BuildingBlocks.Authentication.csproj", "BuildingBlocks/BuildingBlocks.Authentication,/"]
COPY ["BuildingBlocks/BuildingBlocks.Contracts/BuildingBlocks.Contracts.csproj", "BuildingBlocks/BuildingBlocks.Contracts/"]
COPY ["BuildingBlocks/BuildingBlocks.CQRS/BuildingBlocks.CQRS.csproj", "BuildingBlocks/BuildingBlocks.CQRS/"]
COPY ["BuildingBlocks/BuildingBlocks.Exceptions/BuildingBlocks.Exceptions.csproj", "BuildingBlocks/BuildingBlocks.Exceptions/"]
COPY ["BuildingBlocks/BuildingBlocks.Messaging/BuildingBlocks.Messaging.csproj", "BuildingBlocks/BuildingBlocks.Messaging/"]
COPY ["BuildingBlocks/BuildingBlocks.Utility/BuildingBlocks.Utility.csproj", "BuildingBlocks/BuildingBlocks.Utility/"]
COPY ["Notifications/Notifications.ApplicationLayer/Notifications.ApplicationLayer.csproj", "Notifications/Notifications.ApplicationLayer/"]
COPY ["Notifications/Notifications.BusinessLayer/Notifications.BusinessLayer.csproj", "Notifications/Notifications.BusinessLayer/"]
COPY ["Notifications/Notifications.CoreModels/Notifications.CoreModels.csproj", "Notifications/Notifications.CoreModels/"]
COPY ["Notifications/Notifications.DbAccessLayer/Notifications.DbAccessLayer.csproj", "Notifications/Notifications.DbAccessLayer/"]
RUN dotnet restore "./Notifications/Notifications.API/Notifications.API.csproj"
COPY . .
WORKDIR "/src/Notifications/Notifications.API"
RUN dotnet build "./Notifications.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Notifications.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Notifications.API.dll"]
