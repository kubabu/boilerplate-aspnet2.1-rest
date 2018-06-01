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