#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["RabbitMQTest.Consumer/RabbitMQTest.Consumer.csproj", "RabbitMQTest.Consumer/"]
COPY ["Shared/RabbitMQTest.Shared.csproj", "Shared/"]
RUN dotnet restore "RabbitMQTest.Consumer/RabbitMQTest.Consumer.csproj"
COPY . .
WORKDIR "/src/RabbitMQTest.Consumer"
RUN dotnet build "RabbitMQTest.Consumer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RabbitMQTest.Consumer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RabbitMQTest.Consumer.dll"]