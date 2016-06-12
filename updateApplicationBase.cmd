@Echo off
set Source=\\dell56sgg31\c$\WebProjects\2005\ApplicationBase\ApplicationBase\ApplicationBase
set Destination=C:\WebProjects\2005\ApplicationBase\ApplicationBase
Echo Copying %Source%
Echo 	to
Echo %Destination%


rem Everything
goto newOnly
xcopy %Source%\*.vb %Destination%\*.* /s /Y
xcopy %Source%\*.resx %Destination%\*.* /s /Y

:newOnly
goto upDateOnly
xcopy %Source%\*.gif %Destination%\*.* /s /Y
xcopy %Source%\*.jpg %Destination%\*.* /s /Y
xcopy %Source%\*.css %Destination%\*.* /s /Y
xcopy %Source%\*.js %Destination%\*.* /s /Y
xcopy %Source%\*.config %Destination%\*.* /s /Y
xcopy %Source%\*.asax %Destination%\*.* /s /Y
xcopy %Source%\*.aspx %Destination%\*.* /s /Y
xcopy %Source%\*.ascx %Destination%\*.* /s /Y
:upDateOnly
xcopy %Source%\bin\*.dll %Destination%\bin\*.* /s /Y
