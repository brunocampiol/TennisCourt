dotnet tool install --global dotnet-ef

dotnet ef migrations add InitialMigration -c TennisCourtContext -p ./TennisCourt.Infra.Data/TennisCourt.Infra.Data.csproj -s ./TennisCourt.Api/TennisCourt.Api.csproj