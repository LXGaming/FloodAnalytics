# syntax=docker/dockerfile:1
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
ARG TARGETARCH
WORKDIR /src

COPY *.sln ./
COPY LXGaming.FloodAnalytics/*.csproj LXGaming.FloodAnalytics/
RUN dotnet restore LXGaming.FloodAnalytics --arch $TARGETARCH

COPY LXGaming.FloodAnalytics/ LXGaming.FloodAnalytics/
RUN dotnet publish LXGaming.FloodAnalytics --arch $TARGETARCH --configuration Release --no-restore --output /app

FROM mcr.microsoft.com/dotnet/runtime-deps:8.0-alpine
RUN apk add --no-cache --upgrade tzdata
WORKDIR /app
COPY --from=build /app ./
USER $APP_UID
ENTRYPOINT ["./LXGaming.FloodAnalytics"]