# Setup

Install postgresql, do not forget password. Set proper ConnectionString in appsettings.json and run
	
	dotnet restore
	dotnet ef database update 

# DB Models changes

This is code first EF Core application, after updating models run
	
	dotnet ef database update 