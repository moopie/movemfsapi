# Movem File Server API

A simple file server for the masses

## Usage

To use this application you first need to either have your own postgres instance (but then you will have to mess around with connection strings) or use docker-compose to create an empty database

To create an empty database using docker first you need to configure the env file:

```sh
~ cd docker
~ cp .env.defaults .env
```

After configuring the .env file, you can start up postman and redis services:

```sh
~ docker-compose --env-file .env up -d
```

After that is done, you need to apply migrations to the database:

```sh
~ cd - # you need to go back to the root project directory
~ dotnet ef database update --project src/Movem.Db --startup-project src/Movem.Api
```

Then you can finally start the api server:

```sh
~ dotnet run --project src/Movem.Api/Movem.Api.csproj
```

## Postman

Postman collection is located in the `postman` folder
