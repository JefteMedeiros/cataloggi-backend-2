#!/usr/bin/env bash
set -euo pipefail

repo_root="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
cd "$repo_root"

dotnet build cataloggi-backend-2.sln

dotnet publish cataloggi-backend-2/cataloggi-backend-2.csproj -c Release -o ./publish

rm -f publish.zip
cd publish
zip -r ../publish.zip .
cd ..

az webapp deploy \
  --resource-group cataloggi \
  --name cataloggi-api \
  --src-path publish.zip \
  --type zip
