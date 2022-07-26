# Godoor - Identity
This is a service that contains more user specific data that wouldn't be attached as a claim on Azure B2C.  This is the main service for the user specific data.

Inital Setup:

 1. Create a file titled `appsettings.Development.json` inside of the Identity folder next to `exampleappsettings.json`
 2. Copy the contents inside of `exampleappsettings.json` and paste them inside of `appsettings.Development.json`
 3. Replace all of the values that are marked inside of curly brackets `{}` with the necessary values

To run you have multiple options:
1. Use Visual Studio and open the `Identity.sln` file and run using the play button
2. Run `dotnet run` inside of the Identity folder that contains `Identity.csproj`
