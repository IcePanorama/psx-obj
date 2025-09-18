#! /usr/bin/env bash
set -e
dotnet test
dotnet run --project PSXObj/psx-obj.csproj textcube.obj
