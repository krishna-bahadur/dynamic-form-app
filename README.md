# Dynamic Form App

## step 1
Clone the repository

## step 2
Open the repository in Visual Studio

## step 3
Change the local database server name in your `appsettings.Development.json` file:
```bash
"ConnectionStrings": {
  "DefaultConnection": "Server=SERVERNAME;Database=DynamicFormDB;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
}
```

## step 4
Add migration using these command
```bash
add-migration Initial
update-database
```

## step 5
run the application
