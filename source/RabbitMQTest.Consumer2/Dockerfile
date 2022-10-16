#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["RabbitMQTest.Consumer2/RabbitMQTest.Consumer2.csproj", "RabbitMQTest.Consumer2/"]
COPY ["Shared/RabbitMQTest.Shared.csproj", "Shared/"]
RUN dotnet restore "RabbitMQTest.Consumer2/RabbitMQTest.Consumer2.csproj"
COPY . .
WORKDIR "/src/RabbitMQTest.Consumer2"
RUN dotnet build "RabbitMQTest.Consumer2.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RabbitMQTest.Consumer2.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RabbitMQTest.Consumer2.dll"]