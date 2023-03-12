@echo off

dotnet build -c Release

set libs=KarcagS.Shared KarcagS.Common KarcagS.Client.Common KarcagS.Blazor.Common

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

(for %%a in (%libs%) do (
    echo.  
    echo --------%%a--------
    echo. 

    cd ./%%a/bin/Release
    dotnet nuget push .\%%a.%ver%.nupkg --api-key %token% --source https://api.nuget.org/v3/index.json --skip-duplicate
    cd ../../..
    
    echo.  
    echo --------%%a--------
    echo.  
))

:end