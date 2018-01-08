* Getting started

Install postgresql (tested on Postgres 10)
Run:
dotnet restore
Set proper ConnectionString in appsettings.json
Run:
dotnet ef database update 

* DB Models changes

This is code first EF Core application, 
After updating models run:
dotnet ef database update 