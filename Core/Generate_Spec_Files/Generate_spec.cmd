@ECHO OFF
@REM SET AngularSrc="D:\Development\Growthware\Core\Web.Angular\Angular\projects\"
@REM SET ConfigFile="D:\Development\Growthware\Core\Generate_Spec_Files\ngentest.config.js"

@REM CALL Delete_sepc.cmd

@REM IF EXIST "create_tests_services.cmd" (
@REM 	ECHO Deleting create_tests_services.cmd
@REM 	DEL create_tests_services.cmd
@REM )
@REM IF EXIST "create_tests_other.cmd" (
@REM 	ECHO Deleting create_tests_other.cmd
@REM 	DEL create_tests_other.cmd
@REM )
@REM FOR /r %AngularSrc% %%G IN (*.component.ts,*.service.ts,*.directive.ts) DO (
@REM 	ECHO %%G|findstr /i /L "component">nul
@REM 	IF ERRORLEVEL 1 (
@REM 		ECHO npx ngentest %%G -s -f -c %ConfigFile% >> c:\temp\create_tests_services.cmd
@REM 	) ELSE (
@REM 		ECHO npx ngentest %%G -s -f -c %ConfigFile% >> c:\temp\create_tests_other.cmd
@REM 	)
@REM )
powershell -f Generate_spec.ps1