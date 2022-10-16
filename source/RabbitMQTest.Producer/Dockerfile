#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["RabbitMQTest.Producer/RabbitMQTest.Producer.csproj", "RabbitMQTest.Producer/"]
COPY ["Shared/RabbitMQTest.Shared.csproj", "Shared/"]
RUN dotnet restore "RabbitMQTest.Producer/RabbitMQTest.Producer.csproj"
COPY . .
WORKDIR "/src/RabbitMQTest.Producer"
RUN dotnet build "RabbitMQTest.Producer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RabbitMQTest.Producer.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RabbitMQTest.Producer.dll"]