You will need to install the following on your machine as a prerequisite
    1.) Docker Desktop
            https://www.docker.com/products/docker-desktop/
    2.) VS Code
            https://code.visualstudio.com/download
    3.) The following extensions will be helpful:
            1.) "Docker Extention Pack" - formulahendry.
            2.) "C#" - ms-dotnettools.csharp
            3.) ".NET Watch Attach (Extended)" - vhorinek.dotnetwatchattach-ext

So the goal is to build and run the container that will be used to develop Growthware in.
We do this by staring with a Dockerfile that uses the "mcr.microsoft.com/dotnet/sdk:8.0" image.  From there we install all of the dependencies needed to develop Growthware.
The process will install GIT as well as clone the repository.

pull_Images.cmd - Pull the two images used to create the net_core container
build_N_Start.cmd - Will build and start the net_core container without using cache

After you have built the container the first time you will need to run the command "ng completion" from the Web.Angular folder.
    1.) Start VS Code (code .)
    2.) Press F1 and select "Dev Container: Attach to Running Container..."
    3.) Select "/growthware-code"
    4.) Open a folder "/growthware/Net_Core/"
    5.) Open a terminal
    6.) Change to the "Web.Angular" folder
    7.) Run the command "ng completion"
NOTE: I tried to incoporate this into the Dockerfile but it didn't work

You may need to update the "Development_DAL_SQLServer_ConnectionString" in the GrowthWare.json file, depending on when it was last updated it may not match what is needed for the dev container enviorment.
Example:
        "Development_DAL_SQLServer_ConnectionString": "server=growthware-db;User Id=sa;Password=P@ssw0rd;TrustServerCertificate=true;database=GrowthWare;connection lifetime=5;enlist=true;min pool size=1;max pool size=50",

If this is your first time running the container you should need to create the database
    1.) Start VS Code
    2.) From the "Run and Debug" menu select "Database Manager" and Click start or press F5