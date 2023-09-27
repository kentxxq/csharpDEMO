FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Aspnetcore/AddBootstrapBlazor/AddBootstrapBlazor.csproj", "Aspnetcore/AddBootstrapBlazor/"]
RUN dotnet restore "Aspnetcore/AddBootstrapBlazor/AddBootstrapBlazor.csproj"
COPY . .
WORKDIR "/src/Aspnetcore/AddBootstrapBlazor"
RUN dotnet build "AddBootstrapBlazor.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AddBootstrapBlazor.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AddBootstrapBlazor.dll"]
