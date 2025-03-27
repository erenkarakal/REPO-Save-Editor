dotnet publish -c Release -r win-x64 --self-contained false -p:PublishSingleFile=true

dotnet publish -c Release -r linux-x64 --self-contained false -p:PublishSingleFile=true

pause
