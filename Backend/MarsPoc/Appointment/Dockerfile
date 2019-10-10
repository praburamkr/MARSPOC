FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["Appointment/Appointment.csproj", "Appointment/"]
RUN dotnet restore "Appointment/Appointment.csproj"
COPY . .
WORKDIR "/src/Appointment"
RUN dotnet build "Appointment.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Appointment.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Appointment.dll"]