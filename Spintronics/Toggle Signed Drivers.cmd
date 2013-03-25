@echo off

:start
SET /P yesno=Enter 0 to turn off, 1 to turn on: 
IF /I "%yesno%"=="1" (GOTO turnon)
IF /I "%yesno%"=="0" (GOTO turnoff)
GOTO error

:turnon
bcdedit /set TESTSIGNING ON
goto exit

:turnoff
bcdedit /set TESTSIGNING OFF
goto exit

:error
echo Error -- invalid input. Please try again.
goto start

:exit
echo Please reboot your computer.
pause