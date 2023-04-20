#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["Sat.Recruitment.Api/Sat.Recruitment.Api.csproj", "Sat.Recruitment.Api/"]
RUN dotnet restore "Sat.Recruitment.Api/Sat.Recruitment.Api.csproj"
COPY . .
WORKDIR "/src/Sat.Recruitment.Api"
RUN dotnet build "Sat.Recruitment.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Sat.Recruitment.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Sat.Recruitment.Api.dll"]