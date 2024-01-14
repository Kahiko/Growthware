@ECHO OFF
SET AngularSrc="D:\Development\Growthware\Core\Web.Angular\Angular\projects\"
FOR /r %AngularSrc% %%G in (*.spec.ts) DO (
	DEL %%G
)