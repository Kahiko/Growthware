@ECHO OFF
CLS
IF "%~1"=="" GOTO MissingParameters
IF "%~2"=="" GOTO NoPassword
REM Docker Login
:Password
REM The password is a previously generated access token
REM The token looks simular to: "dckr_pat_?????????????????????????"
docker login -u kahiko -p %2
GOTO DoneLogin
:NoPassword
docker login -u kahiko
:DoneLogin
REM ************** Code **************
REM Tag the image
docker tag growthware-code:%1 kahiko/growthware-code:%1
REM Push the image
docker push kahiko/growthware-code:%1
REM Remove local image
REM docker rmi growthware-code:%1
REM ************** Database **************
REM Tag the image
docker tag growthware-db:%1 kahiko/growthware-db:%1
REM Push the image
docker push kahiko/growthware-db:%1
GOTO END
:MissingParameters
ECHO Please pass the version example: X.X.X.X
ECHO Where X is a number.
:END
