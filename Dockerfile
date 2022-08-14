FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["./ShareBook.Web/ShareBook.Web.csproj", "ShareBook.Web/"]
COPY ["./ShareBook.Application/ShareBook.Application.csproj", "ShareBook.Application/"]
COPY ["./ShareBook.Domain/ShareBook.Domain.csproj", "ShareBook.Domain/"]
COPY ["./ShareBook.Shared/ShareBook.Shared.csproj", "ShareBook.Shared/"]
COPY ["./ShareBook.Infrastructure/ShareBook.Infrastructure.csproj", "ShareBook.Infrastructure/"]
RUN dotnet restore "./ShareBook.Web/ShareBook.Web.csproj"
COPY . .
WORKDIR "/src/ShareBook.Web"
RUN dotnet build "ShareBook.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ShareBook.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ShareBook.Web.dll"]
