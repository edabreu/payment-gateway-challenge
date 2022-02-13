#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

COPY "payment-gateway-challenge.sln" "payment-gateway-challenge.sln"

COPY ["source/PaymentGateway.API/PaymentGateway.API.csproj", "source/PaymentGateway.API/"]
COPY ["source/Data.Gateways/Data.Gateways.csproj", "source/Data.Gateways/"]
COPY ["source/Data.Repositories/Data.Repositories.csproj", "source/Data.Repositories/"]
COPY ["source/Domain.Abstractions/Domain.Abstractions.csproj", "source/Domain.Abstractions/"]
COPY ["source/Domain.Models/Domain.Models.csproj", "source/Domain.Models/"]
COPY ["tests/integration/PaymentGateway.Integration.Tests/PaymentGateway.Integration.Tests.csproj", "tests/integration/PaymentGateway.Integration.Tests/"]
COPY ["tests/unit/Data.Repositories.Tests/Data.Repositories.Tests.csproj", "tests/unit/Data.Repositories.Tests/"]
COPY ["tests/unit/PaymentGateway.API.Tests/PaymentGateway.API.Tests.csproj", "tests/unit/PaymentGateway.API.Tests/"]
COPY ["card-tokenizer/CardTokenizer.API/CardTokenizer.API.csproj", "card-tokenizer/CardTokenizer.API/"]
COPY ["cko-bank/CKOBank/CKOBank.csproj", "cko-bank/CKOBank/"]

RUN dotnet restore payment-gateway-challenge.sln
COPY . .
WORKDIR "/src/source/PaymentGateway.API"
RUN dotnet publish --no-restore -c Release -o /app

FROM build as test
WORKDIR /src

FROM build AS publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "PaymentGateway.API.dll"]
