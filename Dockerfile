FROM microsoft/dotnet:sdk AS build-env
WORKDIR /app

# Copy and restore
COPY . ./
RUN dotnet restore

# Build
RUN dotnet publish -c Release -o out

# Build runtime image
FROM microsoft/dotnet:aspnetcore-runtime
WORKDIR /app
COPY --from=build-env /app/PizzaOffer/out .
ENTRYPOINT ["dotnet", "PizzaOffer.dll"]