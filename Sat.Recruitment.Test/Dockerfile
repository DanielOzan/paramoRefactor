#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:3.1 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["Sat.Recruitment.Test/Sat.Recruitment.Test.csproj", "Sat.Recruitment.Test/"]
COPY ["Sat.Recruitment.Api/Sat.Recruitment.Api.csproj", "Sat.Recruitment.Api/"]
RUN dotnet restore "Sat.Recruitment.Test/Sat.Recruitment.Test.csproj"
COPY . .
WORKDIR "/src/Sat.Recruitment.Test"
RUN dotnet build "Sat.Recruitment.Test.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Sat.Recruitment.Test.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Sat.Recruitment.Test.dll"]