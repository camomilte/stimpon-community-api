# stimpon-community-api
.net backend built for bloom&amp;gloom

This application is built with .NET 9 and Entity Framework.

To Run this application in VS Code.
- Install .NET on your computer.
- Install required extensions in Visual Studio Code, C# Dev Kit + .NET Runtime Extension Pack.
- Setup the database, this can be done through the command line with Entity Framework or by manually running the SQL script for the database inside SQL/init.sql. MSSQL was used for this application but any database can be used.
- Configure appsettings.json, set database connection string and data for token generation (Secret, Issuer, Audience, TokenValidityTimeInSeconds).
- Start the application by running dotnet build and dotnet run.
