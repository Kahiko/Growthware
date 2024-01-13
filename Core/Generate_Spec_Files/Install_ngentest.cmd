@ECHO OFF
ECHO Installing ngentest
SET OriginalDir="D:\Development\Growthware\Core\Generate_Spec_Files"
SET AngularSrc="D:\Development\Growthware\Core\Web.Angular\Angular\"
CD %AngularSrc%
CALL npm install ngentest -D
CD %OriginalDir%
