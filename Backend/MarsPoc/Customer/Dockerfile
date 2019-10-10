FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["Customer/Customer.csproj", "Customer/"]
RUN dotnet restore "Customer/Customer.csproj"
COPY . .
WORKDIR "/src/Customer"
RUN dotnet build "Customer.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Customer.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Customer.dll"]