FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["Resource/Resource.csproj", "Resource/"]
RUN dotnet restore "Resource/Resource.csproj"
COPY . .
WORKDIR "/src/Resource"
RUN dotnet build "Resource.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Resource.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Resource.dll"]