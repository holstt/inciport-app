# Inciport Webservice

This is the backend web service for the two clients `inciport-app` and `inciport-webservice`.

### Running from source

The web service requires a postgres database on port 5432. Spin up a db using Docker and the docker-compose file in this project:

- From repository root: `cd docker`
- `docker-compose up database`

Run the web service:

- From repository root: `cd inciport-webservice\InciportWebService.Api`
- Install deps., build and run: `dotnet run`

### Running with Docker

If you would like to run the entire application in Docker, run:

- `export BRANCH_NAME=local && docker-compose --profile webservice up --build`

### Deploy to staging/production manually

NB! This should only be used in special circumstances. Deployment should always happen through the CI/CD pipeline.

From repository root: `cd docker`

Set the "branch name" (this could be anything)

- `export BRANCH_NAME=foo`

Build on local machine

- `docker-compose --profile webservice build`

Push to our Docker Registry (requires credentials)

- `docker-compose --profile webservice push`

Upload docker folder via rsync

- `rsync -av . user@host:~/${BRANCH_NAME}`

Now ssh into the server and run:

- `export BRANCH_NAME=foo`
- `cd ~/${BRANCH_NAME} && docker-compose -f docker-compose.yml -f docker-compose.staging.yml --profile webservice up`
