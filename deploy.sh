#!/bin/bash

dotnet build -c Release

LIBS=('KarcagS.Shared' 'KarcagS.API' 'KarcagS.API.Auth' 'KarcagS.API.Data' 'KarcagS.API.Export' 'KarcagS.API.Http' 'KarcagS.API.Mail' 'KarcagS.API.Mongo' 'KarcagS.API.Repository' 'KarcagS.API.Shared' 'KarcagS.API.Table' 'KarcagS.Client.Common' 'KarcagS.Blazor.Common')

if [ -z "$1" ]; then
    echo >&2 'ERROR: Missing version number'
    exit 1
fi

if [ -z "$2" ]; then
    echo >&2 'ERROR: Missing token'
    exit 1
fi

VER=$1
TOKEN=$2

for l in ${LIBS[@]}; do
    echo
    echo "--------$l--------"
    echo

    dotnet nuget push ./$l/bin/Release/$l.$VER.nupkg --api-key $TOKEN --source https://api.nuget.org/v3/index.json --skip-duplicate

    echo
    echo "--------$l--------"
    echo
done