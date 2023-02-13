FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["UserAuthenticationSystemV2/UserAuthenticationSystemV2.csproj", "UserAuthenticationSystemV2/"]
RUN dotnet restore "UserAuthenticationSystemV2/UserAuthenticationSystemV2.csproj"
COPY . .
WORKDIR "/src/UserAuthenticationSystemV2"
RUN dotnet build "UserAuthenticationSystemV2.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "UserAuthenticationSystemV2.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UserAuthenticationSystemV2.dll"]
