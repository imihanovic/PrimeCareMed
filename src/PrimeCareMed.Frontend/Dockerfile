#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/PrimeCareMed.Frontend/PrimeCareMed.Frontend.csproj", "src/PrimeCareMed.Frontend/"]
RUN dotnet restore "src/PrimeCareMed.Frontend/PrimeCareMed.Frontend.csproj"
COPY . .
WORKDIR "/src/src/PrimeCareMed.Frontend"
RUN dotnet build "PrimeCareMed.Frontend.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PrimeCareMed.Frontend.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PrimeCareMed.Frontend.dll"]