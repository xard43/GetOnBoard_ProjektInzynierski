# Backend

## Wymagania:
* .NET Core 2.2 SDK [Pobierz](https://dotnet.microsoft.com/download/thank-you/dotnet-sdk-2.2.105-windows-x64-installer)

## Instrukcja uruchomienia:
W folderze Backend\GetOnBoard:
- Środowisko developerskie: `dotnet run`
- Środowisko produkcyjne: `dotnet run --environment Production`

## Odpalanie backendu z Visuala na różnych środowiskach pracy: 
- IIS Express - środowisko developerskie
- IIS Express Production - środowisko produkcyjne

## Aplikowanie migracji na bazy na różnych środowiskach pracy: 
- Otwórz Package Manager Console
- Wpisz $env:ASPNETCORE_ENVIRONMENT='Development'. Ustawisz wtedy zmienną środowiskową odpowiedzialną za środowisko pracy 
- Wspierane są dwa środowiska:
	* Development - środowisko developerskie, łączy się do bazy `GetOnBoard_Development`. (Zobacz `appsettings.Development.json`)
	* Production - środowisko produkcyjne, łączy się do bazy `GetOnBoard_Production`. (Zobacz `appsettings.Procustion.json`)
- Wpisz Add-Migration

## Deploy na Azure: 
- Backend ma ustawioną zmienną środowiskową `ASPNETCORE_ENVIRONMENT` na wartość Production