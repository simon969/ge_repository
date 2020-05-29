# Create project
dotnet new razor -o ge_repository
cd ge_repository
dotnet run

# Add entity framework scaffolding
dotnet add package Microsoft.VisualStudio.Web.CodeGeneration.Design
dotnet restore
# Add data classes and context classes
#------
dotnet add package Microsoft.EntityFrameworkCore --version 2.1.4 
dotnet add package Microsoft.EntityFrameworkCore.Relational --version 2.1.4

# Create database and tables from class contexts
dotnet ef migrations add InitialCreate --context ge_DbContext
dotnet ef database update --context ge_DbContext

# Create file scaffolding for CRUD operations
dotnet aspnet-codegenerator razorpage -m ge_data -dc ge_DbContext -udl -outDir Pages\xData
dotnet aspnet-codegenerator razorpage -m ge_project -dc ge_DbContext -udl -outDir Pages\xProject
dotnet aspnet-codegenerator razorpage -m ge_office_user -dc ge_DbContext -udl -outDir Pages\xOffice2
dotnet aspnet-codegenerator razorpage -m ge_office -dc ge_DbContext -udl -outDir Pages\xOffice
dotnet aspnet-codegenerator razorpage -m ge_user -dc ge_DbContext -udl -outDir Pages\xUser
dotnet aspnet-codegenerator razorpage -m ge_transform -dc ge_DbContext -udl -outDir Pages\Transform

# Add ASP.Net Core identity
dotnet aspnet-codegenerator identity --useDefaultUI
dotnet ef migrations add CreateIdentitySchema --context ge_DbContext
dotnet ef database update
dotnet aspnet-codegenerator identity -fi Account.Register
dotnet aspnet-codegenerator identity -fi Account.ForgotPassword
dotnet aspnet-codegenerator identity -fi Account.ForgotPasswordConfirmation
dotnet aspnet-codegenerator identity -fi Account.ResetPassword
dotnet aspnet-codegenerator identity -fi Account.Index
dotnet aspnet-codegenerator identity -fi Account.Login
# Add dynamic sorting
dotnet add package System.Linq.Dynamic.Core

# add additional table without recreating all
dotnet ef migrations add ge_transform

# add mail service to use send mail for account verification
# https://www.nuget.org/packages/MailKit/

dotnet add package MailKit --version 2.1.2 

# https://forums.asp.net/t/2136703.aspx?core+2+0+sample+page+with+register+login+mailkit+instead+sendgrid+possible+

# https://docs.microsoft.com/en-us/aspnet/core/client-side/bundling-and-minification?view=aspnetcore-2.2&tabs=netcore-cli
dotnet add package BuildBundlerMinifier