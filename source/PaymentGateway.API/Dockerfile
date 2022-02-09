#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["source/PaymentGateway.API/PaymentGateway.API.csproj", "source/PaymentGateway.API/"]
COPY ["source/Data.Gateways/Data.Gateways.csproj", "source/Data.Gateways/"]
COPY ["source/Data.Repositories/Data.Repositories.csproj", "source/Data.Repositories/"]
COPY ["source/Domain.Abstractions/Domain.Abstractions.csproj", "source/Domain.Abstractions/"]
COPY ["source/Domain.Models/Domain.Models.csproj", "source/Domain.Models/"]
RUN dotnet restore "source/PaymentGateway.API/PaymentGateway.API.csproj"
COPY . .
WORKDIR "/src/source/PaymentGateway.API"
RUN dotnet build "PaymentGateway.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PaymentGateway.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PaymentGateway.API.dll"]
