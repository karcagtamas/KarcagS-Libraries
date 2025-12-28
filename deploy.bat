@echo off

if "%3" == "minimal" (
    echo "Minimal deployment is enabled"
) else (
    dotnet restore
    dotent build -c Release --no-restore
    dotnet pack -c Release --no-build --output ./nupkgs
)

set libs=KarcagS.Shared KarcagS.API KarcagS.Http KarcagS.API.Auth KarcagS.API.Data KarcagS.API.Export KarcagS.API.Http KarcagS.API.Mail KarcagS.API.Mongo KarcagS.API.Repository KarcagS.API.Shared KarcagS.API.Table KarcagS.Client.Common KarcagS.Blazor.Common

if "%1" == "" (
    echo "ERROR: Missing version number"
    goto end
)

if "%2" == "" (
    echo "ERROR: Missing token"
    goto end
)

set ver=%1
set token=%2

cd ./nupkgs

(for %%a in (%libs%) do (
    echo.  
    echo --------%%a--------
    echo. 

    dotnet nuget push .\%%a.%ver%.nupkg --api-key %token% --source https://api.nuget.org/v3/index.json --skip-duplicate
    
    echo.  
    echo --------%%a--------
    echo.  
))

cd ..

:end