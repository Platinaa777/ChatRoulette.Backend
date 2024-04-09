FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

COPY "ChatRoulette.sln" "ChatRoulette.sln"

COPY "ApiGateways/Ocelot.Gateway/Ocelot.Gateway.csproj" "ApiGateways/Ocelot.Gateway/Ocelot.Gateway.csproj"

COPY "Infrastructure/AmazonS3/S3.Client/S3.Client.csproj"        "Infrastructure/AmazonS3/S3.Client/S3.Client.csproj"
COPY "Infrastructure/AmazonS3/S3.Contracts/S3.Contracts.csproj"  "Infrastructure/AmazonS3/S3.Contracts/S3.Contracts.csproj"

COPY "Infrastructure/MessageBroker/MassTransit.Client/MassTransit.Client.csproj"        "Infrastructure/MessageBroker/MassTransit.Client/MassTransit.Client.csproj"
COPY "Infrastructure/MessageBroker/MassTransit.Contracts/MassTransit.Contracts.csproj"  "Infrastructure/MessageBroker/MassTransit.Contracts/MassTransit.Contracts.csproj"

COPY "Infrastructure/SwaggerConfigurations/SwaggerConfigurations.csproj"  "Infrastructure/SwaggerConfigurations/SwaggerConfigurations.csproj"

# ADMIN
COPY "Services/AdminService/AdminService.Api/AdminService.Api.csproj"                       "Services/AdminService/AdminService.Api/AdminService.Api.csproj"
COPY "Services/AdminService/AdminService.Application/AdminService.Application.csproj"       "Services/AdminService/AdminService.Application/AdminService.Application.csproj"
COPY "Services/AdminService/AdminService.DataContext/AdminService.DataContext.csproj"       "Services/AdminService/AdminService.DataContext/AdminService.DataContext.csproj"
COPY "Services/AdminService/AdminService.Domain/AdminService.Domain.csproj"                 "Services/AdminService/AdminService.Domain/AdminService.Domain.csproj"
COPY "Services/AdminService/AdminService.HttpModels/AdminService.HttpModels.csproj"         "Services/AdminService/AdminService.HttpModels/AdminService.HttpModels.csproj"
COPY "Services/AdminService/AdminService.Infrastructure/AdminService.Infrastructure.csproj" "Services/AdminService/AdminService.Infrastructure/AdminService.Infrastructure.csproj"

# AUTH
COPY "Services/AuthService/AuthService.Api/AuthService.Api.csproj"                       "Services/AuthService/AuthService.Api/AuthService.Api.csproj"
COPY "Services/AuthService/AuthService.Application/AuthService.Application.csproj"       "Services/AuthService/AuthService.Application/AuthService.Application.csproj"
COPY "Services/AuthService/AuthService.DataContext/AuthService.DataContext.csproj"       "Services/AuthService/AuthService.DataContext/AuthService.DataContext.csproj"
COPY "Services/AuthService/AuthService.Domain/AuthService.Domain.csproj"                 "Services/AuthService/AuthService.Domain/AuthService.Domain.csproj"
COPY "Services/AuthService/AuthService.HttpModels/AuthService.HttpModels.csproj"         "Services/AuthService/AuthService.HttpModels/AuthService.HttpModels.csproj"
COPY "Services/AuthService/AuthService.Infrastructure/AuthService.Infrastructure.csproj" "Services/AuthService/AuthService.Infrastructure/AuthService.Infrastructure.csproj"
COPY "Services/AuthService/AuthService.Migrations/AuthService.Migrations.csproj"         "Services/AuthService/AuthService.Migrations/AuthService.Migrations.csproj"

# CHAT
COPY "Services/Chat/Chat.Api/Chat.Api.csproj"                       "Services/Chat/Chat.Api/Chat.Api.csproj"
COPY "Services/Chat/Chat.Application/Chat.Application.csproj"       "Services/Chat/Chat.Application/Chat.Application.csproj"
COPY "Services/Chat/Chat.DataContext/Chat.DataContext.csproj"       "Services/Chat/Chat.DataContext/Chat.DataContext.csproj"
COPY "Services/Chat/Chat.Domain/Chat.Domain.csproj"                 "Services/Chat/Chat.Domain/Chat.Domain.csproj"
COPY "Services/Chat/Chat.HttpModels/Chat.HttpModels.csproj"         "Services/Chat/Chat.HttpModels/Chat.HttpModels.csproj"
COPY "Services/Chat/Chat.Infrastructure/Chat.Infrastructure.csproj" "Services/Chat/Chat.Infrastructure/Chat.Infrastructure.csproj"

# EMAIL
COPY "Services/EmailingService/Emailing.HttpModels/Emailing.HttpModels.csproj" "Services/EmailingService/Emailing.HttpModels/Emailing.HttpModels.csproj"
COPY "Services/EmailingService/EmailingService.Api/EmailingService.Api.csproj" "Services/EmailingService/EmailingService.Api/EmailingService.Api.csproj"
COPY "Services/EmailingService/EmailingService/EmailingService.csproj"         "Services/EmailingService/EmailingService/EmailingService.csproj"

# PROFILE
COPY "Services/ProfileService/ProfileService.Api/ProfileService.Api.csproj"                       "Services/ProfileService/ProfileService.Api/ProfileService.Api.csproj"
COPY "Services/ProfileService/ProfileService.Application/ProfileService.Application.csproj"       "Services/ProfileService/ProfileService.Application/ProfileService.Application.csproj"
COPY "Services/ProfileService/ProfileService.Domain/ProfileService.Domain.csproj"                 "Services/ProfileService/ProfileService.Domain/ProfileService.Domain.csproj"
COPY "Services/ProfileService/ProfileService.HttpModels/ProfileService.HttpModels.csproj"         "Services/ProfileService/ProfileService.HttpModels/ProfileService.HttpModels.csproj"
COPY "Services/ProfileService/ProfileService.Infrastructure/ProfileService.Infrastructure.csproj" "Services/ProfileService/ProfileService.Infrastructure/ProfileService.Infrastructure.csproj"
COPY "Services/ProfileService/ProfileService.Migrations/ProfileService.Migrations.csproj"         "Services/ProfileService/ProfileService.Migrations/ProfileService.Migrations.csproj"

# DDD
COPY "SharedAbstractions/DomainDriverDesignAbstractions/DomainDriverDesignAbstractions.csproj" "SharedAbstractions/DomainDriverDesignAbstractions/DomainDriverDesignAbstractions.csproj"

COPY "Tests/Domain.Tests/Domain.Tests.csproj" "Tests/Domain.Tests/Domain.Tests.csproj"

RUN dotnet restore "ChatRoulette.sln"

COPY . .

WORKDIR /src/Services/EmailingService/EmailingService.Api

RUN dotnet publish -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app

EXPOSE 80
EXPOSE 443

COPY --from=build /app .
ENTRYPOINT ["dotnet", "EmailingService.Api.dll"]