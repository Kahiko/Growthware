FROM mcr.microsoft.com/dotnet/sdk:8.0
EXPOSE 44455
EXPOSE 5000
EXPOSE 5001

# Install NodeJs
RUN apt-get update && \
apt-get install -y wget && \
apt-get install -y gnupg2 && \
wget -qO- https://deb.nodesource.com/setup_20.x | bash - && \
apt-get install -y build-essential nodejs
# End Install

# Install Angular
RUN npm install -g @angular/cli@17.2
# End Install

# Install GIT
RUN apt-get update
RUN apt-get install git -y
# End Install