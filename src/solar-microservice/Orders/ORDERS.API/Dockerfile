FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Orders/ORDERS.API/ORDERS.API.csproj", "Orders/ORDERS.API/"]
COPY ["BuildingBlocks/BuildingBloacks.Behaviour/BuildingBloacks.Behaviour.csproj", "BuildingBlocks/BuildingBloacks.Behaviour/"]
COPY ["BuildingBlocks/BuildingBlocks.Authentication,/BuildingBlocks.Authentication.csproj", "BuildingBlocks/BuildingBlocks.Authentication,/"]
COPY ["BuildingBlocks/BuildingBlocks.Contracts/BuildingBlocks.Contracts.csproj", "BuildingBlocks/BuildingBlocks.Contracts/"]
COPY ["BuildingBlocks/BuildingBlocks.CQRS/BuildingBlocks.CQRS.csproj", "BuildingBlocks/BuildingBlocks.CQRS/"]
COPY ["BuildingBlocks/BuildingBlocks.Exceptions/BuildingBlocks.Exceptions.csproj", "BuildingBlocks/BuildingBlocks.Exceptions/"]
COPY ["BuildingBlocks/BuildingBlocks.Messaging/BuildingBlocks.Messaging.csproj", "BuildingBlocks/BuildingBlocks.Messaging/"]
COPY ["BuildingBlocks/BuildingBlocks.Utility/BuildingBlocks.Utility.csproj", "BuildingBlocks/BuildingBlocks.Utility/"]
COPY ["Orders/Orders.ApplicationLayer/Orders.ApplicationLayer.csproj", "Orders/Orders.ApplicationLayer/"]
COPY ["Orders/Orders.BusinessLayer/Orders.BusinessLayer.csproj", "Orders/Orders.BusinessLayer/"]
COPY ["Orders/Orders.CoreModels/Orders.CoreModels.csproj", "Orders/Orders.CoreModels/"]
COPY ["Orders/Orders.Infrastructure/Orders.Infrastructure.csproj", "Orders/Orders.Infrastructure/"]
RUN dotnet restore "./Orders/ORDERS.API/ORDERS.API.csproj"
COPY . .
WORKDIR "/src/Orders/ORDERS.API"
RUN dotnet build "./ORDERS.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ORDERS.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ORDERS.API.dll"]
