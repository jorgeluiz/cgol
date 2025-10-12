FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 5217

ENV ASPNETCORE_URLS=http://+:5217

# Creates a non-root user with an explicit UID and adds permission to access the /app folder
# For more info, please refer to https://aka.ms/vscode-docker-dotnet-configure-containers
RUN adduser -u 5678 --disabled-password --gecos "" appuser && chown -R appuser /app
USER appuser

FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG configuration=Release
WORKDIR /src
COPY ["backend/DL.GameOfLife.Tests/DL.GameOfLife.Tests.csproj", "backend/DL.GameOfLife.Tests/"]
COPY ["backend/DL.GameOfLife.Service/DL.GameOfLife.Service.csproj", "backend/DL.GameOfLife.Service/"]
COPY ["backend/DL.GameOfLife.Models/DL.GameOfLife.Models.csproj", "backend/DL.GameOfLife.Models/"]
COPY ["backend/DL.GameOfLife.Extensions/DL.GameOfLife.Extensions.csproj", "backend/DL.GameOfLife.Extensions/"]
COPY ["backend/DL.GameOfLife.Domain/DL.GameOfLife.Domain.csproj", "backend/DL.GameOfLife.Domain/"]
COPY ["backend/DL.GameOfLife.Data/DL.GameOfLife.Data.csproj", "backend/DL.GameOfLife.Data/"]
COPY ["backend/DL.GameOfLife.Api/DL.GameOfLife.Api.csproj", "backend/DL.GameOfLife.Api/"]
RUN dotnet restore "backend/DL.GameOfLife.Api/DL.GameOfLife.Api.csproj"
COPY . .
WORKDIR "/src/backend/DL.GameOfLife.Api"
RUN dotnet build "DL.GameOfLife.Api.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "DL.GameOfLife.Api.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DL.GameOfLife.Api.dll"]
