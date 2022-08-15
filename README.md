# 📱Social Media Web Api built with ASP.NET Core and Neo4J.

This is a simple Social Media Web App built with the latest ASP.NET Core 7.0 that uses a Neo4J Graph Database as its primary data store as well as a Azure Blob Storage for storing media assets (like photos, etc). Some of its features include:

-  User Authentication using JWTs
-  Ability for users to create posts and attach media to it,
-  Profile creation and editing (including an avatar),
-  Liking other users' posts
-  Following other users.
-  Commenting on posts
-  And last but not least, a news feed generation feature that uses a basic algorithm.

## A sample diagram of the Graph database model schema:

![Neo4J model schema][neo4j-model-schema]

# Techologies used

-  ASP.NET Core 7.0
-  [AutoMapper](AutoMapper)
-  [FluentValidation](FluentValidation)
-  [Bogus](Bogus)
-  [Neo4JClient]
-  [Neo4J]

[automapper]: www.automapper.com
[neo4j]: https://neo4j.com/
[neo4jclient]: https://github.com/DotNet4Neo4j/Neo4jClient
[fluentvalidation]: www.fluent-validation.com
[neo4j-model-schema]: https://github.com/vnikolaew/ShareBook/blob/main/neo4j-model-schema.jpg?raw=true
[download neo4j]: https://neo4j.com/download-center
[appsettings.development.json]: https://github.com/vnikolaew/ShareBook/blob/main/ShareBook.Web/appsettings.Development.json
[here]: https://github.com/vnikolaew/ShareBook/blob/main/docker-compose.sample.yml
[mit license]: https://github.com/vnikolaew/ShareBook/blob/main/LICENSE.md

# Getting Started

Before running the application, there are some prerequisites needed. Make sure you first have:

-  installed .NET 6.0+ on your machine
-  Neo4J 4.4+ Graph database installed (download link - [Download Neo4J]) and have a locally running instance (prefferably using the Bolt protocol)
-  have setup an Microsoft Azure Blob Storage Account

If you're running the application locally, make sure you configure all the application's settings like Azure Blob and Neo4J configurations. You can achieve that by using the appsettings.json config file (see [appsettings.Development.json] for further details)

## Running in a Docker environment

The second option is by running the application and database services in Docker containers using the docker-compose tool. Make sure you have installed Docker (and docker-compose). You can find an example docker-compose.yml file [here] in the repository root and see how you can configure it.

# License

This project is licensed with the [MIT License].
