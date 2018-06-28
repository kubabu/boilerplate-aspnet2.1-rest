# Setup

Install postgresql, do not forget password. Set proper ConnectionString in appsettings.json and run
	
	dotnet restore
	dotnet ef database update

# SSL Certificate

For development create certificate for localhost domain with "pfx" password and copy it to project root as localhost.pfx
Install it, or add it CA to trusted root certificates

# DB Models changes

This is code first EF Core application, after updating models run
	
	dotnet ef migrations add MigrationName
	dotnet ef database update 

# Unit tests

Command line: 

	cd webapi/tests
	dotnet test

# Unit tests not found

Visual Studio (and dotnet test command line util) can suffer from Nunit adapter cache crash. Solution: remove cache from directory:

	C:\Users\username\AppData\Local\Temp\VisualStudioTestExplorerExtensions\

and restart Visual Studio. More about problem: https://github.com/nunit/nunit3-vs-adapter/issues/261