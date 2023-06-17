$targetPath = "C:\Users\JonathanROGER\OneDrive\Immobilier\Software"
$exePath = "C:\Personal\Frimmo.RealEstate\Frimmo.Console\bin\Release\net6.0\*"

write-host '1. Generate application'  -ForegroundColor Green
dotnet publish -c Release 

write-host '2. Publish exe in shared folder'  -ForegroundColor Green

Copy-Item -Path $exePath -Destination $targetPath -Force -Recurse -Exclude @("db.json", "markets.json")

write-host '3. Deployment Finished'  -ForegroundColor Green

