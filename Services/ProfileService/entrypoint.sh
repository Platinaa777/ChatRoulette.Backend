# This script run migrations and run application after migrations

app_run="dotnet ProfileService.Api.dll --no-build -v d"

echo "Run Migration ProfileService...."
dotnet ProfileService.Migration dll --no-build -v d
echo "Migration completed..."

echo "App start"
echo "Run Api: $app_run"

exec $app_run