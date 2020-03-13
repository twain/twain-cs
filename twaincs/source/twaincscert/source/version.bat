::
:: Grab our version info...
::
for /f "tokens=2 delims=^(^)" %%a in ('type "%~dp0%Properties\AssemblyInfo.cs" ^| find "AssemblyFileVersion"') do (
  set VERSION=%%a
)

::
:: Clean it up a little...
::
set VERSION=%VERSION:"=%

::
:: Save it...
::
echo %VERSION% > "%~dp0%data\version.txt"
