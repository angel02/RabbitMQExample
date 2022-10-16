#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["RabbitMQTest.Consumer3/RabbitMQTest.Consumer3.csproj", "RabbitMQTest.Consumer3/"]
RUN dotnet restore "RabbitMQTest.Consumer3/RabbitMQTest.Consumer3.csproj"
COPY . .
WORKDIR "/src/RabbitMQTest.Consumer3"
RUN dotnet build "RabbitMQTest.Consumer3.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RabbitMQTest.Consumer3.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RabbitMQTest.Consumer3.dll"]