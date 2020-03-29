# Tyba-Challenge
Esteban Villegas C.C. 1112476929

## Setup
1. Create database with name Tyba and execute the DatabaseScripts/Table-creation.sql script on it.
2. Update the connection string accordingly at Tyba.App.Api/appsettings.Development.json > TybaConnectionString (provide a valid server, user and password).
3. Run the Tyba.App.Api.csproj either with Visual Studio or Visual Studio Code.
4. Use the 'Tyba' Postman collection, or the Swagger UI in the browser for testing.
5. Steps for testing: first register a new user, then login with the same credentials, then save the token from the login response. Call the Restaurants Nearby endpoint with a valid 'Authorization' headed providing the token. Example 'Authorization': 'Bearer xxyyzz...".
