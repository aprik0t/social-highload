FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build
COPY . .
WORKDIR /SocialHighload
RUN dotnet publish SocialHighload.csproj -c Release -o /app

FROM mcr.microsoft.com/dotnet/core/aspnet:2.2 AS runtime
WORKDIR /app
COPY --from=build /app .

ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT false
RUN apk add --no-cache icu-libs
ENV LC_ALL ru_RU.UTF-8
ENV LANG ru_RU.UTF-8

ENTRYPOINT ["dotnet", "SocialHighload.dll"]