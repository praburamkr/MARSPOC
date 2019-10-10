FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["Product/Product.csproj", "Product/"]
RUN dotnet restore "Product/Product.csproj"
COPY . .
WORKDIR "/src/Product"
RUN dotnet build "Product.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Product.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Product.dll"]