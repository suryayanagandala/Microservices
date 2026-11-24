FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Payments/PAYMENTS.API/PAYMENTS.API.csproj", "Payments/PAYMENTS.API/"]
COPY ["BuildingBlocks/BuildingBloacks.Behaviour/BuildingBloacks.Behaviour.csproj", "BuildingBlocks/BuildingBloacks.Behaviour/"]
COPY ["BuildingBlocks/BuildingBlocks.Authentication,/BuildingBlocks.Authentication.csproj", "BuildingBlocks/BuildingBlocks.Authentication,/"]
COPY ["BuildingBlocks/BuildingBlocks.Contracts/BuildingBlocks.Contracts.csproj", "BuildingBlocks/BuildingBlocks.Contracts/"]
COPY ["BuildingBlocks/BuildingBlocks.CQRS/BuildingBlocks.CQRS.csproj", "BuildingBlocks/BuildingBlocks.CQRS/"]
COPY ["BuildingBlocks/BuildingBlocks.Exceptions/BuildingBlocks.Exceptions.csproj", "BuildingBlocks/BuildingBlocks.Exceptions/"]
COPY ["BuildingBlocks/BuildingBlocks.Messaging/BuildingBlocks.Messaging.csproj", "BuildingBlocks/BuildingBlocks.Messaging/"]
COPY ["BuildingBlocks/BuildingBlocks.Utility/BuildingBlocks.Utility.csproj", "BuildingBlocks/BuildingBlocks.Utility/"]
COPY ["Payments/Payments.ApplicationLayer/Payments.ApplicationLayer.csproj", "Payments/Payments.ApplicationLayer/"]
COPY ["Payments/Payments.BusinessLayer/Payments.BusinessLayer.csproj", "Payments/Payments.BusinessLayer/"]
COPY ["Payments/Payments.CoreModels/Payments.CoreModels.csproj", "Payments/Payments.CoreModels/"]
COPY ["Payments/Payments.Infrastructure/Payments.Infrastructure.csproj", "Payments/Payments.Infrastructure/"]
RUN dotnet restore "./Payments/PAYMENTS.API/PAYMENTS.API.csproj"
COPY . .
WORKDIR "/src/Payments/PAYMENTS.API"
RUN dotnet build "./PAYMENTS.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./PAYMENTS.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PAYMENTS.API.dll"]
