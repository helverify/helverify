#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Helverify.VotingAuthority.Backend/Helverify.VotingAuthority.Backend.csproj", "Helverify.VotingAuthority.Backend/"]
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
ENTRYPOINT ["dotnet", "Helverify.VotingAuthority.Backend.dll"]