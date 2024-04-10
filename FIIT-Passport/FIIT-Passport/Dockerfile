FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Fiit-passport/Fiit-passport.csproj", "Fiit-passport/"]
RUN dotnet restore "Fiit-passport/Fiit-passport.csproj"
COPY . .
WORKDIR "/src/Fiit-passport"
RUN dotnet build "Fiit-passport.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Fiit-passport.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Fiit-passport.dll"]
