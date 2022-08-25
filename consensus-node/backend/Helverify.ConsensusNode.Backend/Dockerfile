#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0.8-focal-amd64 AS base

RUN apt-get update
RUN apt-get install -y software-properties-common
RUN add-apt-repository -y ppa:ethereum/ethereum
RUN apt-get update
RUN apt-get install -y ethereum
RUN apt-get install -y dnsutils
RUN apt-get install -y jq

#FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Helverify.ConsensusNode.Backend/Helverify.ConsensusNode.Backend.csproj", "Helverify.ConsensusNode.Backend/"]
COPY ["Helverify.ConsensusNode.Domain/Helverify.ConsensusNode.Domain.csproj", "Helverify.ConsensusNode.Domain/"]
RUN dotnet restore "Helverify.ConsensusNode.Backend/Helverify.ConsensusNode.Backend.csproj"
COPY . .

WORKDIR "/src/Helverify.ConsensusNode.Backend"
RUN dotnet build "Helverify.ConsensusNode.Backend.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Helverify.ConsensusNode.Backend.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

RUN chmod +x /app/scripts/*.sh

ENTRYPOINT ["dotnet", "Helverify.ConsensusNode.Backend.dll"]
