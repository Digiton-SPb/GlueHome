FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app

EXPOSE 80

WORKDIR /src
COPY ["src/BLL/TT.Deliveries.Common/*.csproj", "src/BLL/TT.Deliveries.Common/"]
COPY ["src/BLL/TT.Deliveries.Domain/*.csproj", "src/BLL/TT.Deliveries.Domain/"]
COPY ["src/BLL/TT.Deliveries.Domain.Dto/*.csproj", "src/BLL/TT.Deliveries.Domain.Dto/"]
COPY ["src/DAL/TT.Deliveries.DataAccess.EF/*.csproj", "src/DAL/TT.Deliveries.DataAccess.EF/"]
COPY ["src/DAL/TT.Deliveries.DataAccess.EF.Common/*.csproj", "src/DAL/TT.Deliveries.DataAccess.EF.Common/"]
COPY ["src/Web/TT.Deliveries.Web.Api/*.csproj", "src/Web/TT.Deliveries.Web.Api/"]
RUN dotnet restore "src/Web/TT.Deliveries.Web.Api/TT.Deliveries.Web.Api.csproj"

COPY . .
WORKDIR "/src/src/Web/TT.Deliveries.Web.Api"
RUN dotnet publish "TT.Deliveries.Web.Api.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "TT.Deliveries.Web.Api.dll"]

VOLUME /app/db /app/logs
