#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["src/VendingMachine.API/VendingMachine.API.csproj", "src/VendingMachine.API/"]
COPY ["src/VendingMachine.Domain/VendingMachine.Domain.csproj", "src/VendingMachine.Domain/"]
RUN dotnet restore "src/VendingMachine.API/VendingMachine.API.csproj"
COPY . .
WORKDIR "/src/src/VendingMachine.API"
RUN dotnet build "VendingMachine.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "VendingMachine.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "VendingMachine.API.dll"]
