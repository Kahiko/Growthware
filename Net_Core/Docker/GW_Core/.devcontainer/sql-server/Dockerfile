# https://github.com/devcontainers/images/blob/main/src/dotnet/README.md
# Use the official SQL Server 2022 image from Microsoft
# FROM mcr.microsoft.com/mssql/server:2019-CU25-ubuntu-20.04 as build
# FROM mcr.microsoft.com/mssql/server:2022-CU12-ubuntu-20.04 as build
FROM mcr.microsoft.com/mssql/server:2022-latest as build

# EXPOSE 1433

# Switch to root user for access to apt-get install
USER root

# Set the EULA and SA password environment variables for SQL Server
ENV ACCEPT_EULA=Y
# ENV SA_PASSWORD=P@ssw0rd
ENV MSSQL_SA_PASSWORD=P@ssw0rd

# Use the directory as the SQL Server data storage
VOLUME /var/opt/mssql

# docker run -p 11433:1433 -d --name growthware-db growthware-db:4.0.1.0
# docker exec growthware-db ls -l
# docker exec growthware-db sqlcmd -S 127.0.0.1 -U sa -P "P@ssw0rd"

# "Development_DAL_SQLServer_ConnectionString": "server=(local);Integrated Security=SSPI;database=GrowthWare;connection lifetime=5;enlist=true;min pool size=1;max pool size=50",
# "Development_DAL_SQLServer_ConnectionString": "server=growthware-db,11433;User Id=sa;Password=P@ssw0rd;database=GrowthWare;connection lifetime=5;enlist=true;min pool size=1;max pool size=50",
# "Development_DAL_SQLServer_ConnectionString": "server=growthware-db;User Id=sa;Password=P@ssw0rd;TrustServerCertificate=true;database=GrowthWare;connection lifetime=5;enlist=true;min pool size=1;max pool size=50",
# /opt/mssql-tools/bin/sqlcmd -S 127.0.0.1 -U sa -P "P@ssw0rd"