#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0.8-focal-amd64 AS base

RUN apt-get update
RUN apt-get install -y software-properties-common
RUN add-apt-repository -y ppa:ethereum/ethereum
RUN apt-get update
RUN apt-get install -y ethereum
RUN apt-get install -y dnsutils
RUN apt-get install -y jq
RUN apt-get install -y fontconfig
RUN apt-get install -y fonts-freefont-ttf

COPY Helverify.VotingAuthority.Backend/fonts /usr/share/fonts/TTF

WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Helverify.VotingAuthority.Backend/Helverify.VotingAuthority.Backend.csproj", "Helverify.VotingAuthority.Backend/"]
COPY ["Helverify.VotingAuthority.Application/Helverify.VotingAuthority.Application.csproj", "Helverify.VotingAuthority.Application/"]
COPY ["Helverify.VotingAuthority.Domain/Helverify.VotingAuthority.Domain.csproj", "Helverify.VotingAuthority.Domain/"]
COPY ["Helverify.VotingAuthority.DataAccess/Helverify.VotingAuthority.DataAccess.csproj", "Helverify.VotingAuthority.DataAccess/"]
RUN dotnet restore "Helverify.VotingAuthority.Backend/Helverify.VotingAuthority.Backend.csproj"
COPY . .
WORKDIR "/src/Helverify.VotingAuthority.Backend"
RUN dotnet build "Helverify.VotingAuthority.Backend.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Helverify.VotingAuthority.Backend.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

RUN chmod +x /app/scripts/*.sh

ENTRYPOINT ["dotnet", "Helverify.VotingAuthority.Backend.dll"]