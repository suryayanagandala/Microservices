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
COPY ["BuildingBlocks/BuildingBlocks.Resilience/BuildingBlocks.Resilience.csproj", "BuildingBlocks/BuildingBlocks.Resilience/"]
COPY ["BuildingBlocks/BuildingBlocks.Observability/BuildingBlocks.Observability.csproj", "BuildingBlocks/BuildingBlocks.Observability/"]
COPY ["Cart/CART.API/CART.API.csproj", "Cart/CART.API/"]
COPY ["BuildingBlocks/BuildingBloacks.Behaviour/BuildingBloacks.Behaviour.csproj", "BuildingBlocks/BuildingBloacks.Behaviour/"]
COPY ["BuildingBlocks/BuildingBlocks.Authentication,/BuildingBlocks.Authentication.csproj", "BuildingBlocks/BuildingBlocks.Authentication,/"]
COPY ["BuildingBlocks/BuildingBlocks.Contracts/BuildingBlocks.Contracts.csproj", "BuildingBlocks/BuildingBlocks.Contracts/"]
COPY ["BuildingBlocks/BuildingBlocks.CQRS/BuildingBlocks.CQRS.csproj", "BuildingBlocks/BuildingBlocks.CQRS/"]
COPY ["BuildingBlocks/BuildingBlocks.Exceptions/BuildingBlocks.Exceptions.csproj", "BuildingBlocks/BuildingBlocks.Exceptions/"]
COPY ["BuildingBlocks/BuildingBlocks.Messaging/BuildingBlocks.Messaging.csproj", "BuildingBlocks/BuildingBlocks.Messaging/"]
COPY ["BuildingBlocks/BuildingBlocks.Utility/BuildingBlocks.Utility.csproj", "BuildingBlocks/BuildingBlocks.Utility/"]
COPY ["Cart/Cart.ApplicationLayer/Cart.ApplicationLayer.csproj", "Cart/Cart.ApplicationLayer/"]
COPY ["Cart/Cart.BusinessLayer/Cart.BusinessLayer.csproj", "Cart/Cart.BusinessLayer/"]
COPY ["Cart/Cart.CoreModels/Cart.CoreModels.csproj", "Cart/Cart.CoreModels/"]
COPY ["Cart/Cart.Infrastructure/Cart.Infrastructure.csproj", "Cart/Cart.Infrastructure/"]
RUN dotnet restore "./Cart/CART.API/CART.API.csproj"
COPY . .
WORKDIR "/src/Cart/CART.API"
RUN dotnet build "./CART.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./CART.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CART.API.dll"]
