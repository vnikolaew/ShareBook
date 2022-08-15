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

# Getting Started

Before running the application, there are some prerequisites needed. Make sure you first have:

-  installed .NET 6.0+ on your machine
-  Neo4J 4.4+ Graph database installed (download link - [Download Neo4J])
-  have setup an Microsoft Azure Blob Storage Account
