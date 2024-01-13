@ECHO OFF
SET AngularSrc="D:\Development\Growthware\Core\Web.Angular\Angular\projects\"
setlocal EnableDelayedExpansion
SET LF=^


REM Previous two lines deliberately left blank for LF to work.
SET /a mCount=0
SET mExtList[0]=""
FOR /r %AngularSrc% %%G in (*.ts) DO (
    SET "mFileName=%%~nG"
    SET "mFileName=!mFileName: - =-!"
    FOR /f "tokens=1,2,3 delims=." %%A in ("!mFileName!") DO (
        IF NOT [%%B]==[] (
            SET mType=%%B
            ECHO !extlist! | find "!mType!:" > nul
            IF not !ERRORLEVEL! == 0 (
                SET extlist=!extlist!!mType!:
            )
        )
    )
)
ECHO %extlist::=!LF!%

endlocal
