FROM mcr.microsoft.com/dotnet/sdk:9.0 AS base

# Install SQL Tools: SQLPackage and sqlcmd
COPY mssql/installSQLtools.sh installSQLtools.sh
# RUN bash ./installSQLtools.sh \
# && apt-get clean -y && rm -rf /var/lib/apt/lists/* /tmp/library-scripts
# COPY devcontainer.json /root/.devcontainer/devcontainer.json

# Install GIT
RUN apt-get update \
&& apt-get install git -y \
&& apt-get clean -y

# Install NodeJs
RUN apt-get update \
&& apt-get install -y wget \
&& apt-get install -y gnupg2 \
&& wget -qO- https://deb.nodesource.com/setup_20.x | bash - \
&& apt-get install -y build-essential nodejs

# Install Angular
RUN npm install -g @angular/cli@18.2

# Clone the growthware repository
RUN git config --global --add safe.directory /growthware
RUN git clone --branch 5_2_0 https://github.com/Kahiko/Growthware.git growthware

# Setup the dev certs
RUN mkdir -p $HOME/.aspnet/https
RUN chmod 777 $HOME/.aspnet/https
RUN dotnet dev-certs https
RUN dotnet dev-certs https --trust
WORKDIR /growthware/Net_Core

# Restore and build
RUN dotnet restore
RUN dotnet build

# Install required packages
WORKDIR /growthware/Net_Core/Web.Angular
RUN npm install