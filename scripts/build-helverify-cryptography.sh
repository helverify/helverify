#!/bin/bash
rm -rf ../cryptography/dotnet/Helverify.Cryptography/bin/Release/net6.0/
dotnet build ../cryptography/dotnet/Helverify.Cryptography/Helverify.Cryptography.csproj --configuration=Release --self-contained false --force -o ../cryptography/dotnet/Helverify.Cryptography/bin/Release/net6.0/
