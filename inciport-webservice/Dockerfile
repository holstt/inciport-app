#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

# Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS runtime
WORKDIR /app

# Download any depenencies to runtime image

# Image used for build
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src

ARG FOLDER
ARG SLN

# Copy the main source project files to restore from (create cached dep. layer)
# Copy solution as well in order to restore and build from this (ie. instead of the startup project)
COPY ${FOLDER}/${SLN}.sln ${FOLDER}/

# ------ START OF USER MODIFICATION ------

# Add a ARG 'PX' and a copy command for every project name

ARG P1
COPY ${FOLDER}/${SLN}.${P1}/*.csproj ${FOLDER}/${SLN}.${P1}/
ARG P2
COPY ${FOLDER}/${SLN}.${P2}/*.csproj ${FOLDER}/${SLN}.${P2}/
ARG P3
COPY ${FOLDER}/${SLN}.${P3}/*.csproj ${FOLDER}/${SLN}.${P3}/
ARG P4
COPY ${FOLDER}/${SLN}.${P4}/*.csproj ${FOLDER}/${SLN}.${P4}/
ARG P5
COPY ${FOLDER}/${SLN}.${P5}/*.csproj ${FOLDER}/${SLN}.${P5}/
ARG P6
COPY ${FOLDER}/${SLN}.${P6}/*.csproj ${FOLDER}/${SLN}.${P6}/

# ----- END OF USER MODIFICATION ---------



# Download dependencies for entire solution
RUN dotnet restore ${FOLDER}

# Copy the source code to image. NB! This keeps existing files that have been already copied to image.
COPY ${FOLDER} ${FOLDER}

# Change dir to folder of solution.
WORKDIR "/src/${FOLDER}"

# Build project | Precond: Current dir contains .sln file.
# -c Release (set build configuration to release)
# -/p:WarningLevel=0 (suppress all compiler warnings)
# --no-restore Do not restore as done in seperate step
# Result: Creates /bin dir in project folders with compiled files.
RUN dotnet build -c Release /p:WarningLevel=0

# Test project - LAYER IS SKIPPED IF NOT EXPLICITLY TARGETED | Precond: Current dir contains .sln file
# --no-build Do not build as solution already built into /bin dir in prev layer
FROM build AS test
RUN dotnet test -c Release --no-build

# Publish project | Precond: Current dir contains .sln file.
# --no-build Do not build as solution already built into /bin dir in prev layer
FROM build AS publish
RUN dotnet publish -c Release -o /app/publish /p:WarningLevel=0 --no-build

FROM runtime AS final
WORKDIR /app

# Copy compiled files from publish (build) image into runtime image
COPY --from=publish /app/publish .

# Get variables from input
ARG SLN
ARG START_P
ARG FOLDER

# Why ENV? -> to persist in environment after docker build.
ENV RUN_CMD=${SLN}.${START_P}.dll

ENTRYPOINT dotnet $RUN_CMD