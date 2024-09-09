#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM mcr.microsoft.com/dotnet/runtime:8.0-nanoserver-1809 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0-nanoserver-1809 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["DeliVeggieApplicationNewTest/DeliVeggieApplicationNewTest.csproj", "DeliVeggieApplicationNewTest/"]
COPY ["DeliVeggieApplication/DeliVeggieApplication.csproj", "DeliVeggieApplication/"]
RUN dotnet restore "./DeliVeggieApplicationNewTest/DeliVeggieApplicationNewTest.csproj"
COPY . .
WORKDIR "/src/DeliVeggieApplicationNewTest"
RUN dotnet build "./DeliVeggieApplicationNewTest.csproj" -c %BUILD_CONFIGURATION% -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./DeliVeggieApplicationNewTest.csproj" -c %BUILD_CONFIGURATION% -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DeliVeggieApplicationNewTest.dll"]