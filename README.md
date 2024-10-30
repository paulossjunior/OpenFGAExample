# OpenFGA Authorization Model Setup

This repository contains a C# application for setting up an authorization model and testing access control with OpenFGA, a system for fine-grained access control. The application creates an authorization model and defines user relationships to control read and write access to a document.

## Features

* Create Authorization Model: Defines types and relationships for users and documents, specifying read and write permissions.
* Create Relationships: Establishes user permissions for specific documents.
* Check Permissions: Verifies user access (read/write) to documents based on the created authorization model.

## Prerequisites

   * .NET SDK
   * OpenFGA instance (local or hosted), configured with a base URL and Store ID
## Code Overview

  * OpenFGASetup Class: Sets up the authorization model and creates relationships.
       * CreateAuthorizationModel(): Sends a request to create a model with user and document types.
       * CreateRelationships(): Defines relationships, linking users (reader or writer) to documents.

  * OpenFGAClient Class: Checks user permissions.
       * Check(): Verifies if a user has a specific relationship (e.g., read or write) with a document
   
  ```chsarp
var client = new OpenFGAClient(baseUrl, storeId, setup.AuthorizationModelId);

// Check if "user:john" has "reader" access to "document:doc1"
bool canRead = await client.Check("user:john", "reader", "document", "doc1");
Console.WriteLine($"Can John read doc1? {canRead}");

// Check if "user:jane" has "writer" access to "document:doc1"
bool canWrite = await client.Check("user:jane", "writer", "document", "doc1");
Console.WriteLine($"Can Jane write doc1? {canWrite}");

  ```
