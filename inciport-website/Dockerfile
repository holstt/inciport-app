FROM node:16-alpine as build

# Create app directory for the docker image
WORKDIR /app

ARG FOLDER

# Install app dependencies from package.json. If modules are not included in the package.json file enter a RUN command. E.g. RUN npm install <module-name>
COPY ${FOLDER}/package.json ${FOLDER}/package.json
COPY ${FOLDER}/package-lock.json ${FOLDER}/package-lock.json
COPY ${FOLDER}/tsconfig.json ${FOLDER}/tsconfig.json

# Install all dependencies. Specify folder of package.json
RUN npm --prefix ${FOLDER} install
# Install typescrip
RUN npm i -g typescript
# Copy the source code to image. NB! This keeps existing files that have been already copied to image.
COPY ${FOLDER} ${FOLDER}

# Validate typescript installation
RUN tsc --version
WORKDIR "/app/${FOLDER}"


# Test project - LAYER IS SKIPPED IF NOT EXPLICITLY TARGETED
FROM build AS test
# Run in CI mode, which will exit tests when done instead of going into "watch" mode.
ENV CI=true 
RUN npm test

# Build static files for production
FROM build AS publish
RUN npm run build


FROM node:16-alpine as final
RUN npm install -g serve
ARG FOLDER

WORKDIR /app/${FOLDER}/publish
COPY --from=publish /app/${FOLDER}/build .

ENTRYPOINT ["serve", "-s", "."]
