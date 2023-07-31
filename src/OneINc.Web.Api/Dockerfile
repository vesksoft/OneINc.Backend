#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["OneINc.Web.Api/OneINc.Web.Api.csproj", "OneINc.Web.Api/"]
RUN dotnet restore "OneINc.Web.Api/OneINc.Web.Api.csproj"
COPY . .
WORKDIR "/src/OneINc.Web.Api"
RUN dotnet build "OneINc.Web.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OneINc.Web.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OneINc.Web.Api.dll"]