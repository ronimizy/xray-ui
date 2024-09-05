#!/bin/bash

export ASPNETCORE_ENVIRONMENT='Local'

dotnet ef migrations add "$1" --context DatabaseContext --output-dir Migrations -s ../../../src/XrayUi
