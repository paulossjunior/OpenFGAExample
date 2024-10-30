# OpenFGA Authorization Model Setup

This repository contains a C# application for setting up an authorization model and testing access control with OpenFGA, a system for fine-grained access control. The application creates an authorization model and defines user relationships to control read and write access to a document.

## Features

* Create Authorization Model: Defines types and relationships for users and documents, specifying read and write permissions.
* Create Relationships: Establishes user permissions for specific documents.
* Check Permissions: Verifies user access (read/write) to documents based on the created authorization model.

## Prerequisites

   * .NET SDK
   * Docker and Docker Compose (to run OpenFGA)
   * OpenFGA instance (local or hosted), configured with a base URL and Store ID
## Running OpenFGA with Docker Compose

To run OpenFGA, use docker-compose. Ensure you have the correct docker-compose.yml file configured in your environment, then execute the command:
```bash
docker-compose up -d
```
## Creating and Accessing a Store
1. Creating a Store: To create a store in OpenFGA, you can use the OpenFGA Playground at: [http://localhost:3000/playground](http://localhost:3000/playground
). From the playground interface, you can create a new store and manage other configurations interactively.
2. Retrieving the Store ID: Once the store is created, you can access its ID by making a request or visiting: [http://localhost:8080/stores
](http://localhost:8080/stores)

This endpoint will list all stores, including their IDs, which you’ll need for configuring the application
## Usage
1. Clone the Repository
2. Configure the Program: Update baseUrl and storeId in the Program.Main method with your OpenFGA URL and the retrieved Store ID.
3. Run the Application: ```bash dotnet run```
4. Expected Output:
    * Create an authorization model with user and document types.
    *  Define read and write relationships for specific users.
    *  Verify if users have the correct access permissions.
## Code Overview

  * OpenFGASetup Class: Sets up the authorization model and creates relationships.
       * CreateAuthorizationModel(): Sends a request to create a model with user and document types.
       * CreateRelationships(): Defines relationships, linking users (reader or writer) to documents.

  * OpenFGAClient Class: Checks user permissions.
       * Check(): Verifies if a user has a specific relationship (e.g., read or write) with a document
## Example
Use the following example to check access permissions:  

```csharp
var client = new OpenFGAClient(baseUrl, storeId, setup.AuthorizationModelId);

// Check if "user:john" has "reader" access to "document:doc1"
bool canRead = await client.Check("user:john", "reader", "document", "doc1");
Console.WriteLine($"Can John read doc1? {canRead}");

// Check if "user:jane" has "writer" access to "document:doc1"
bool canWrite = await client.Check("user:jane", "writer", "document", "doc1");
Console.WriteLine($"Can Jane write doc1? {canWrite}");

  ```
