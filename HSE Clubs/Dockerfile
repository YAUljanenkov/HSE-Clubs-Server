FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY ["HSE Clubs/HSE Clubs.csproj", "HSE Clubs/"]
RUN dotnet restore "HSE Clubs/HSE Clubs.csproj"
COPY . .
WORKDIR "/src/HSE Clubs"
RUN dotnet build "HSE Clubs.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HSE Clubs.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HSE Clubs.dll"]
