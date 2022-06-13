#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
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
ENTRYPOINT ["dotnet", "Helverify.ConsensusNode.Backend.dll"]