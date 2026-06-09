FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY src/ .
RUN dotnet restore PrevisaoCompras.sln
RUN dotnet publish PrevisaoCompras.Api/PrevisaoCompras.Api.csproj -c Release -o /app/api
RUN dotnet publish PrevisaoCompras.Worker/PrevisaoCompras.Worker.csproj -c Release -o /app/worker

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS api
WORKDIR /app
COPY --from=build /app/api .
EXPOSE 5000
ENV ASPNETCORE_URLS=http://+:5000
ENTRYPOINT ["dotnet", "PrevisaoCompras.Api.dll"]

FROM mcr.microsoft.com/dotnet/runtime:9.0 AS worker
WORKDIR /app
COPY --from=build /app/worker .
ENTRYPOINT ["dotnet", "PrevisaoCompras.Worker.dll"]
