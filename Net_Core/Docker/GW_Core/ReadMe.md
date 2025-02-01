You will need to install the following on your machine as a prerequisite
    1.) Docker Desktop
            https://www.docker.com/products/docker-desktop/
    2.) VS Code
            https://code.visualstudio.com/download
    3.) The following extensions will be helpful to install in the docker container as well as local:
            1.) "Docker" - ms-azuretools.vscode-docker
            2.) "Docker Extention Pack" - formulahendry.
            3.) "C#" - ms-dotnettools.csharp
            4.) "C# Dev Kit" - ms-dotnettools.csdevkit
            5.) ".NET Watch Attach (Extended)" - vhorinek.dotnetwatchattach-ext
            6.) ".NET Install Tool" - ms-dotnettools.vscode-dotnet-runtime
            7.) "SQL Server (mssql)" - ms-mssql.mssql

It is assumed you have a basic knowledge of VS Code not all steps are explicit

So the goal is to build and run the container that will be used to develop Growthware in.
We do this by staring with a Dockerfile that uses the "mcr.microsoft.com/dotnet/sdk:9.0" image.  From there we install all of the dependencies needed to develop Growthware.
The process will install GIT as well as clone the repository.

pull_Images.cmd - Pull the two images used to create the net_core container
build_NoCache_N_Start.cmd - Will build and start the net_core container without using cache
build_N_Start.cmd - Will build and start the net_core container using cache

After you have built the container the first time you will need to run the command "ng completion" from the Web.Angular folder.
    1.) Start VS Code (code .)
    2.) Press F1 and select "Dev Containers: Attach to Running Container..."
    3.) Select "/growthware-code"
    4.) Open a terminal
    5.) Change to folder - cd "/growthware/Net_Core/Web.Angular/"
    6.) Run the command "ng completion"
NOTE: I tried to incoporate this into the Dockerfile but it didn't work

You will need to edit the GrowthWare.json changes to find the file use your explorer "open folder" choose ".." then "growthware/Net_Core
        DAL_SQLServer_ConnectionString
                Example:
                        "Development_DAL_SQLServer_ConnectionString": "server=growthware-db;User Id=sa;Password=P@ssw0rd;TrustServerCertificate=true;database=GrowthWare;connection lifetime=5;enlist=true;min pool size=1;max pool size=50",
        Log_Path
                Example:
                        "/growthware/Net_Core/Web.Api/Logs/"

You will need to update the: 
        1.) "Manage Cachedependency" function's "Directory" on the "Directory Information" tab to CacheDependency/
        2.) "Manage Logs" function's "Directory" on the "Directory Information" tab to Logs/

If this is your first time running the container you should need to create the database
    1.) Start VS Code
    2.) From the "Run and Debug" tab. (CTRL + SHIFT + D)
    3.) Select "Database Manager" from the drop down
    4.) Click start or press F5

If you would like to connect to the database in the container using SSMS you will need to use the following:
        Server type:    Database Engine
        Server name:    localhost,11433
        Authentication: SQL Server Authentication
        Login:          sa
        Password:       P@ssw0rd