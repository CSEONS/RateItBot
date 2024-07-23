echo Y | rmdir .\Migrations /s
dotnet-ef --project .\RateItBot.csproj --startup-project .\RateItBot.csproj migrations add "Initialize"
echo Y | dotnet-ef --project .\RateItBot.csproj --startup-project .\RateItBot.csproj database drop
dotnet-ef --project .\RateItBot.csproj --startup-project .\RateItBot.csproj database update
pause