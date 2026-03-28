FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["TicTacToe.WebAPI/TicTacToe.WebAPI.csproj", "TicTacToe.WebAPI/"]
COPY ["TicTacToe.Application/TicTacToe.Application.csproj", "TicTacToe.Application/"]
COPY ["TicTacToe.Domain/TicTacToe.Domain.csproj", "TicTacToe.Domain/"]
COPY ["TicTacToe.Infra,Data/TicTacToe.Infra.Data.csproj", "TicTacToe.Infra,Data/"]
RUN dotnet restore "TicTacToe.WebAPI/TicTacToe.WebAPI.csproj"

COPY . .
WORKDIR "/src/TicTacToe.WebAPI"
RUN dotnet build -c Release -o /app/build

FROM build AS publish
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TicTacToe.WebAPI.dll"]