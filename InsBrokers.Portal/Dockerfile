#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["InsBrokers.Portal/InsBrokers.Portal.csproj", "InsBrokers.Portal/"]
RUN dotnet restore "InsBrokers.Portal/InsBrokers.Portal.csproj"
COPY . .
WORKDIR "/src/InsBrokers.Portal"
RUN dotnet build "InsBrokers.Portal.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "InsBrokers.Portal.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "InsBrokers.Portal.dll"]