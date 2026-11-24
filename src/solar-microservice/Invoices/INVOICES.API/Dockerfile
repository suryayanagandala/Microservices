FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["BuildingBlocks/BuildingBlocks.Observability/BuildingBlocks.Observability.csproj", "BuildingBlocks/BuildingBlocks.Observability/"]
COPY ["Invoices/INVOICES.API/INVOICES.API.csproj", "Invoices/INVOICES.API/"]
COPY ["BuildingBlocks/BuildingBloacks.Behaviour/BuildingBloacks.Behaviour.csproj", "BuildingBlocks/BuildingBloacks.Behaviour/"]
COPY ["BuildingBlocks/BuildingBlocks.Authentication,/BuildingBlocks.Authentication.csproj", "BuildingBlocks/BuildingBlocks.Authentication,/"]
COPY ["BuildingBlocks/BuildingBlocks.Contracts/BuildingBlocks.Contracts.csproj", "BuildingBlocks/BuildingBlocks.Contracts/"]
COPY ["BuildingBlocks/BuildingBlocks.CQRS/BuildingBlocks.CQRS.csproj", "BuildingBlocks/BuildingBlocks.CQRS/"]
COPY ["BuildingBlocks/BuildingBlocks.Exceptions/BuildingBlocks.Exceptions.csproj", "BuildingBlocks/BuildingBlocks.Exceptions/"]
COPY ["BuildingBlocks/BuildingBlocks.Messaging/BuildingBlocks.Messaging.csproj", "BuildingBlocks/BuildingBlocks.Messaging/"]
COPY ["BuildingBlocks/BuildingBlocks.Utility/BuildingBlocks.Utility.csproj", "BuildingBlocks/BuildingBlocks.Utility/"]
COPY ["Invoices/Invoices.ApplicationLayer/Invoices.ApplicationLayer.csproj", "Invoices/Invoices.ApplicationLayer/"]
COPY ["Invoices/Invoices.BusinessLayer/Invoices.BusinessLayer.csproj", "Invoices/Invoices.BusinessLayer/"]
COPY ["Invoices/Invoices.CoreModels/Invoices.CoreModels.csproj", "Invoices/Invoices.CoreModels/"]
COPY ["Invoices/Invoices.Infrastructure/Invoices.Infrastructure.csproj", "Invoices/Invoices.Infrastructure/"]
RUN dotnet restore "./Invoices/INVOICES.API/INVOICES.API.csproj"
COPY . .
WORKDIR "/src/Invoices/INVOICES.API"
RUN dotnet build "./INVOICES.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./INVOICES.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "INVOICES.API.dll"]
