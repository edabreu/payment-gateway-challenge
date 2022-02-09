#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["card-tokenizer/CardTokenizer.API/CardTokenizer.API.csproj", "card-tokenizer/CardTokenizer.API/"]
RUN dotnet restore "card-tokenizer/CardTokenizer.API/CardTokenizer.API.csproj"
COPY . .
WORKDIR "/src/card-tokenizer/CardTokenizer.API"
RUN dotnet build "CardTokenizer.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CardTokenizer.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CardTokenizer.API.dll"]
