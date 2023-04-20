#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
#WORKDIR /src
COPY ["Sat.Recruitment.Api/Sat.Recruitment.Api.csproj", "Sat.Recruitment.Api/"]
COPY ["Sat.Recruitment.Test/Sat.Recruitment.Test.csproj", "Sat.Recruitment.Test/"]
RUN dotnet restore "Sat.Recruitment.Api/Sat.Recruitment.Api.csproj"
RUN dotnet restore "Sat.Recruitment.Test/Sat.Recruitment.Test.csproj"


# copy full solution over
COPY . .
RUN dotnet build "Sat.Recruitment.Api/Sat.Recruitment.Api.csproj" -c Release -o /app/build
RUN dotnet build "Sat.Recruitment.Test/Sat.Recruitment.Test.csproj" -c Release -o /app/build

# run the unit tests
FROM build AS test
# set the directory to be within the unit test project
WORKDIR /app/Sat.Recruitment.Test
# run the unit tests
RUN dotnet test --logger:trx


#Publish the api
FROM build AS publish
RUN dotnet publish "Sat.Recruitment.Api/Sat.Recruitment.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
EXPOSE 80
ENTRYPOINT ["dotnet", "Sat.Recruitment.Api.dll"]




# For docker images building
# build to the test target of the Dockerfile
# docker build --target testrunner -t example-service-tests:latest .
# run the unit tests
# docker run example-service-tests:latest