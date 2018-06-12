FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
ENV VMSTATS_WEBSERVER_URLENV FILE_TYPE http://docker01.dest.internal


FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY VmstatsGUI.sln ./
COPY VmstatsGUI/*.csproj ./VmstatsGUI/

RUN dotnet restore
COPY . .
WORKDIR /src/VmstatsGUI
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "VmstatsGUI.dll"]