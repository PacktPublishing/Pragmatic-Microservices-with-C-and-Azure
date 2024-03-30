# Chapter 3

## Technical requirements

### Source code repo

The code for this chapter can be found in the following GitHub repository: https://github.com/PacktPublishing/Pragmatic-Microservices-With-CSharp-and-Azure

The source code folder ch03 contains the code samples for this chapter. You'll find the code for the four core projects:

* `Codebreaker.Data.SqlServer` - this is the new library to access Microsoft SQL Server.
* `Codebreaker.Data.Cosmos` - this is the new library to access Azure Cosmos DB.
* `Codebreaker.GamesAPIs` - this is the Web API project created in the previous chapter. In this chapter the dependency injection container to use the repository implementations 
* These projects are used in this chapter but unchanged from the previous chapter: `Codebreaker.AppHost`, `Codebreaker.ServiceDefaults`, and `Codebreaker.GameAPIs.Models`.

The games analyzer library from the previous chapter is not included with this chapter. Here we'll just use the NuGet package CNinnovation.Codebreaker.Analyzers.

> If you worked through the previous chapter to create the models and implemented the minimal API project, you can continue from there.  You can also copy the files from the folder ch02 if you didn't complete the previous work, and start from there. Ch03 contains all the updates from this chapter.

### Development tools used

Other than a development environment, you need Microsoft SQL Server and Azure Cosmos DB. 

See [Installation of SQL Server, Azure Cosmos DB](../installation.md)

## Running the application

To run the application from this chapter, these options are available:

### In-Memory

Set this setting with `appsettings.Development.json` (project `Codebreaker.AppHost`):

```json
  "DataStore": "InMemory"
```

### SQL Server

Set the `DataStore` and the `GamesSqlServerConnection with `appsettings.Development.json`

```json
  "DataStore": "SqlServer"
```

Database migration happens when starting the application, thus the database is created on first use with the `SqlServer` setting.

### Azure Cosmos DB

> Currently there's an issue with the Cosmos Docker container - see https://github.com/dotnet/aspire/discussions/2535. As a temporary workaround, use the Azure Cosmos DB local emulator on Windows.

Install the Cosmos emulator: `winget install Microsoft.Azure.CosmosEmulator`

In case there are previous installations of the Cosmos emulator which stopped working, reset the data, or if this fails, uninstall the emulator and install it again.

Running the application with Azure Cosmos DB, set the `DataStore` with `appsettings.Development.json` (project Codebreaker.AppHost), and set the `GamesCosmosConnection` (with a well-known endpoint to the emulator):

```json
  "DataStore": "Cosmos",
  "ConnectionStrings": {
    "GamesCosmosConnection": "AccountEndpoint=https://localhost:8081/;AccountKey=C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==;"
  }
```

## Codebreaker diagrams

* [Components diagram](components.drawio)
* [Create a game using SQL Server](CreateAGameWithSQLServer.md)
