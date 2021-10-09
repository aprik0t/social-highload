# для локальной сборки можно использовать команду
# docker build . --force-rm -t aprikot/social-highload:latest
FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build
COPY . .
WORKDIR /SocialHighload
RUN dotnet publish SocialHighload.csproj -c Release -o /app

FROM mcr.microsoft.com/dotnet/core/aspnet:2.2 AS runtime
WORKDIR /app
COPY --from=build /app .

ENTRYPOINT ["dotnet", "SocialHighload.dll"]