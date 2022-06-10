#!/bin/bash
cryptolib=../cryptography/dotnet/Helverify.Cryptography/bin/Release/net6.0/Helverify.Cryptography.dll

cp $cryptolib ../voting-authority/backend/Helverify.VotingAuthority.Backend
cp $cryptolib ../consensus-node/backend/
