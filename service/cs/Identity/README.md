# Godoor - Identity
This is a service that contains more user specific data that wouldn't be attached as a claim on Azure B2C.  This is the main service for the user specific data.

Configuration Setup:  
 1. Create a file titled `appsettings.Development.json` inside of the Identity folder next to `exampleappsettings.json`
 2. Copy the contents inside of `exampleappsettings.json` and paste them inside of `appsettings.Development.json`
 3. Replace all of the values that are marked inside of curly brackets `{}` with the necessary values

Local Database Setup:  
1. Run `docker compose up` to create the local MSSql docker instance (you must have docker cli installed)

Entity Framework Core Setup:
1. In a terminal navigate to the `Identity.Data` folder
2. Run `dotnet tool install --global dotnet-ef` (you must have dotnet cli installed)
3. Run `dotnet ef --startup-project ../Identity.API database update`

To start the API you have multiple options:
1. Use Visual Studio and open the `Identity.sln` file and run using the play button
2. Run `dotnet run` inside of the Identity.API folder that contains `Identity.API.csproj`
