FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["Notification/Notification.csproj", "Notification/"]
RUN dotnet restore "Notification/Notification.csproj"
COPY . .
WORKDIR "/src/Notification"
RUN dotnet build "Notification.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Notification.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Notification.dll"]