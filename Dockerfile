#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["N5PermisosAPI/N5PermisosAPI.csproj", "N5PermisosAPI/"]
COPY ["CQRS/CQRS.csproj", "CQRS/"]
COPY ["DataAccess/DataAccess.csproj", "DataAccess/"]
COPY ["Data/Data.csproj", "Data/"]
RUN dotnet restore "N5PermisosAPI/N5PermisosAPI.csproj"
COPY . .
WORKDIR "/src/Data"
RUN dotnet build "Data.csproj" -c Release -o /app/build/Data
WORKDIR "/src/DataAccess"
RUN dotnet build "DataAccess.csproj" -c Release -o /app/build/DataAccess
WORKDIR "/src/CQRS"
RUN dotnet build "CQRS.csproj" -c Release -o /app/build/CQRS
WORKDIR "/src"
RUN dotnet build "N5PermisosAPI/N5PermisosAPI.csproj" -c Release -o /app/build

FROM build AS publish
WORKDIR "/src/Data"
RUN dotnet publish "Data.csproj" -c Release -o /app/publish/Data /p:UseAppHost=false
WORKDIR "/src/DataAccess"
RUN dotnet publish "DataAccess.csproj" -c Release -o /app/publish/DataAccess /p:UseAppHost=false
WORKDIR "/src/CQRS"
RUN dotnet publish "CQRS.csproj" -c Release -o /app/publish/CQRS /p:UseAppHost=false
WORKDIR "/src/N5PermisosAPI"
RUN dotnet publish "N5PermisosAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false


FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "N5PermisosAPI.dll"]