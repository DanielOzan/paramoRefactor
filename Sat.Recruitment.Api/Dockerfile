#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["Sat.Recruitment.Api/Sat.Recruitment.Api.csproj", "Sat.Recruitment.Api/"]
COPY ["Sat.Recruitment.Test/Sat.Recruitment.Test.csproj", "Sat.Recruitment.Test/"]
RUN dotnet restore 


# copy full solution over
COPY . .
RUN dotnet build

# create a new layer from the build later
FROM build AS publish
# set the working directory to be the web api project
WORKDIR /app/src/Sat.Recruitment.Api
# publish the web api project to a directory called out
RUN dotnet publish -c Release -o out

# create a new layer using the cut-down aspnet runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-alpine AS runtime
WORKDIR /app
# copy over the files produced when publishing the service
COPY --from=publish /app/src/Example.Service/out ./
# expose port 80 as our application will be listening on this port
EXPOSE 80
# run the web api when the docker image is started
ENTRYPOINT ["dotnet", "Example.Service.dll"]
