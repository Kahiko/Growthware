# Creating a .Net API
## Directory Structure
In this example we'll be using Angular 18 along with multiple .Net projects.  The directory structure will simular to the following:

[yourProjectsDirectory]<br/>
├── [businessLogic]<br/>
├── [webAPI]<br/>
├── [angularWorkSpace]<br/>
└── solution.sln<br/>
In my particular case I have 2 projets, 1 is .Net projects and 1 is the Angular workspace.<br/>
Net_Core<br/>
├── .vscode             * Important for the launch.json and task.json<br/>
├── Web.Angular         * This is the Angular workspace and not included in the solution<br/>
├── Web.Api             - .Net Project included in the solution<br/>
└── GrowthwareCore.sln<br/>

## Pre-requisites
    1. Install .Net 9 SDK https://dotnet.microsoft.com/en-us/download/dotnet/9.0    
## Create the .Net 9 API
Within your projects directory we will create a new .Net API in the Web.Api directory
### Create a dotnet webapi project
    1. Create a dotnet webapi project
	    1. dotnet new webapi -o Web.Api
    2. Change to the newly created directory
	    1. cd Web.Api
	    2. ren Web.Api.csproj GrowthWare.Web.Api.csproj
	3. Add swagger (Be aware this is being depricated and won't be supported for long)
        1. dotnet add package Swashbuckle.AspNetCore --version 7.2.0
    4. Add Microsoft.AspNetCore.Authentication.JwtBearer
        1. dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer --version 9.0.2
    