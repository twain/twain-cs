@echo off
:: 
:: makerelease v1.1 08-Nov-2021
:: Ask some questions, and copy the built files into this folder
:: to make releasing stuff easier and less error prone.
::

::
:: Show the version info, and get an okay on it...
::
echo.
echo | set /p dummyName="TWAIN CS........................."
findstr /C:"AssemblyFileVersion" "%~dp0%..\twaincs\source\Properties\AssemblyInfo.cs"
::
echo | set /p dummyName="TWAIN CS Certification..........."
findstr /C:"AssemblyFileVersion" "%~dp0%..\twaincs\source\twaincscert\source\Properties\AssemblyInfo.cs"
::
echo | set /p dummyName="TWAIN CS Scan...................."
findstr /C:"AssemblyFileVersion" "%~dp0%..\twaincs\source\twaincsscan\source\Properties\AssemblyInfo.cs"
::
echo | set /p dummyName="TWAIN CS Test...................."
findstr /C:"AssemblyFileVersion" "%~dp0%..\twaincs\source\twaincstst\source\Properties\AssemblyInfo.cs"
::
echo.
set answer=
set /p answer="Are you happy with the version info (Y/n)? "
if "%answer%" == "" goto VERSIONDONE
if "%answer%" == "y" goto VERSIONDONE
goto:EOF
::
:VERSIONDONE


::
:: Delete the current folder, and recreate it with subfolders...
::
echo.
echo *** Cleaning the twain-cs_00000000 folder...
rmdir /s /q "%~dp0%twain-cs_00000000" > NUL 2>&1
mkdir "%~dp0%twain-cs_00000000"
::
mkdir "%~dp0%twain-cs_00000000\twaincscert\AnyCPU\Debug"
mkdir "%~dp0%twain-cs_00000000\twaincscert\AnyCPU\Release"
mkdir "%~dp0%twain-cs_00000000\twaincscert\x64\Debug"
mkdir "%~dp0%twain-cs_00000000\twaincscert\x64\Release"
mkdir "%~dp0%twain-cs_00000000\twaincscert\x86\Debug"
mkdir "%~dp0%twain-cs_00000000\twaincscert\x86\Release"
::
mkdir "%~dp0%twain-cs_00000000\twaincsscan\AnyCPU\Debug"
mkdir "%~dp0%twain-cs_00000000\twaincsscan\AnyCPU\Release"
mkdir "%~dp0%twain-cs_00000000\twaincsscan\x64\Debug"
mkdir "%~dp0%twain-cs_00000000\twaincsscan\x64\Release"
mkdir "%~dp0%twain-cs_00000000\twaincsscan\x86\Debug"
mkdir "%~dp0%twain-cs_00000000\twaincsscan\x86\Release"
::
mkdir "%~dp0%twain-cs_00000000\twaincstst\AnyCPU\Debug"
mkdir "%~dp0%twain-cs_00000000\twaincstst\AnyCPU\Release"
mkdir "%~dp0%twain-cs_00000000\twaincstst\x64\Debug"
mkdir "%~dp0%twain-cs_00000000\twaincstst\x64\Release"
mkdir "%~dp0%twain-cs_00000000\twaincstst\x86\Debug"
mkdir "%~dp0%twain-cs_00000000\twaincstst\x86\Release"


::
:: Copy the files...
::
echo.
echo *** Copying TWAIN CS.rtf to the twain-cs_00000000 folder...
xcopy "%~dp0%..\twaincs\source\TWAIN CS.rtf" "twain-cs_00000000\" | find /V "File(s)"
::
echo.
echo *** Copying TWAIN CS Certification files to the twain-cs_00000000 folder...
xcopy "%~dp0%..\twaincs\source\twaincscert\source\bin\AnyCPU\Debug\TWAIN.dll"                 "twain-cs_00000000\twaincscert\AnyCPU\Debug\" | find /V "File(s)"
xcopy "%~dp0%..\twaincs\source\twaincscert\source\bin\AnyCPU\Debug\TWAIN.pdb"                 "twain-cs_00000000\twaincscert\AnyCPU\Debug\" | find /V "File(s)"
xcopy "%~dp0%..\twaincs\source\twaincscert\source\bin\AnyCPU\Debug\twaincscert.exe"           "twain-cs_00000000\twaincscert\AnyCPU\Debug\" | find /V "File(s)"
xcopy "%~dp0%..\twaincs\source\twaincscert\source\bin\AnyCPU\Debug\twaincscert.pdb"           "twain-cs_00000000\twaincscert\AnyCPU\Debug\" | find /V "File(s)"
xcopy "%~dp0%..\twaincs\source\twaincscert\source\bin\AnyCPU\Debug\twaincscert.exe.config"    "twain-cs_00000000\twaincscert\AnyCPU\Debug\" | find /V "File(s)"
::
xcopy "%~dp0%..\twaincs\source\twaincscert\source\bin\AnyCPU\Release\TWAIN.dll"               "twain-cs_00000000\twaincscert\AnyCPU\Release\" | find /V "File(s)"
xcopy "%~dp0%..\twaincs\source\twaincscert\source\bin\AnyCPU\Release\TWAIN.pdb"               "twain-cs_00000000\twaincscert\AnyCPU\Release\" | find /V "File(s)"
xcopy "%~dp0%..\twaincs\source\twaincscert\source\bin\AnyCPU\Release\twaincscert.exe"         "twain-cs_00000000\twaincscert\AnyCPU\Release\" | find /V "File(s)"
xcopy "%~dp0%..\twaincs\source\twaincscert\source\bin\AnyCPU\Release\twaincscert.pdb"         "twain-cs_00000000\twaincscert\AnyCPU\Release\" | find /V "File(s)"
xcopy "%~dp0%..\twaincs\source\twaincscert\source\bin\AnyCPU\Release\twaincscert.exe.config"  "twain-cs_00000000\twaincscert\AnyCPU\Release\" | find /V "File(s)"
::
xcopy "%~dp0%..\twaincs\source\twaincscert\source\bin\x64\Debug\TWAIN.dll"                    "twain-cs_00000000\twaincscert\x64\Debug\" | find /V "File(s)"
xcopy "%~dp0%..\twaincs\source\twaincscert\source\bin\x64\Debug\TWAIN.pdb"                    "twain-cs_00000000\twaincscert\x64\Debug\" | find /V "File(s)"
xcopy "%~dp0%..\twaincs\source\twaincscert\source\bin\x64\Debug\twaincscert.exe"              "twain-cs_00000000\twaincscert\x64\Debug\" | find /V "File(s)"
xcopy "%~dp0%..\twaincs\source\twaincscert\source\bin\x64\Debug\twaincscert.pdb"              "twain-cs_00000000\twaincscert\x64\Debug\" | find /V "File(s)"
xcopy "%~dp0%..\twaincs\source\twaincscert\source\bin\x64\Debug\twaincscert.exe.config"       "twain-cs_00000000\twaincscert\x64\Debug\" | find /V "File(s)"
::
xcopy "%~dp0%..\twaincs\source\twaincscert\source\bin\x64\Release\TWAIN.dll"                  "twain-cs_00000000\twaincscert\x64\Release\" | find /V "File(s)"
xcopy "%~dp0%..\twaincs\source\twaincscert\source\bin\x64\Release\TWAIN.pdb"                  "twain-cs_00000000\twaincscert\x64\Release\" | find /V "File(s)"
xcopy "%~dp0%..\twaincs\source\twaincscert\source\bin\x64\Release\twaincscert.exe"            "twain-cs_00000000\twaincscert\x64\Release\" | find /V "File(s)"
xcopy "%~dp0%..\twaincs\source\twaincscert\source\bin\x64\Release\twaincscert.pdb"            "twain-cs_00000000\twaincscert\x64\Release\" | find /V "File(s)"
xcopy "%~dp0%..\twaincs\source\twaincscert\source\bin\x64\Release\twaincscert.exe.config"     "twain-cs_00000000\twaincscert\x64\Release\" | find /V "File(s)"
::
xcopy "%~dp0%..\twaincs\source\twaincscert\source\bin\x86\Debug\TWAIN.dll"                    "twain-cs_00000000\twaincscert\x86\Debug\" | find /V "File(s)"
xcopy "%~dp0%..\twaincs\source\twaincscert\source\bin\x86\Debug\TWAIN.pdb"                    "twain-cs_00000000\twaincscert\x86\Debug\" | find /V "File(s)"
xcopy "%~dp0%..\twaincs\source\twaincscert\source\bin\x86\Debug\twaincscert.exe"              "twain-cs_00000000\twaincscert\x86\Debug\" | find /V "File(s)"
xcopy "%~dp0%..\twaincs\source\twaincscert\source\bin\x86\Debug\twaincscert.pdb"              "twain-cs_00000000\twaincscert\x86\Debug\" | find /V "File(s)"
xcopy "%~dp0%..\twaincs\source\twaincscert\source\bin\x86\Debug\twaincscert.exe.config"       "twain-cs_00000000\twaincscert\x86\Debug\" | find /V "File(s)"
::
xcopy "%~dp0%..\twaincs\source\twaincscert\source\bin\x86\Release\TWAIN.dll"                  "twain-cs_00000000\twaincscert\x86\Release\" | find /V "File(s)"
xcopy "%~dp0%..\twaincs\source\twaincscert\source\bin\x86\Release\TWAIN.pdb"                  "twain-cs_00000000\twaincscert\x86\Release\" | find /V "File(s)"
xcopy "%~dp0%..\twaincs\source\twaincscert\source\bin\x86\Release\twaincscert.exe"            "twain-cs_00000000\twaincscert\x86\Release\" | find /V "File(s)"
xcopy "%~dp0%..\twaincs\source\twaincscert\source\bin\x86\Release\twaincscert.pdb"            "twain-cs_00000000\twaincscert\x86\Release\" | find /V "File(s)"
xcopy "%~dp0%..\twaincs\source\twaincscert\source\bin\x86\Release\twaincscert.exe.config"     "twain-cs_00000000\twaincscert\x86\Release\" | find /V "File(s)"
::
echo.
echo *** Copying TWAIN CS Scan files to the twain-cs_00000000 folder...
xcopy "%~dp0%..\twaincs\source\twaincsscan\source\bin\AnyCPU\Debug\TWAIN.dll"                 "twain-cs_00000000\twaincsscan\AnyCPU\Debug\" | find /V "File(s)"
xcopy "%~dp0%..\twaincs\source\twaincsscan\source\bin\AnyCPU\Debug\TWAIN.pdb"                 "twain-cs_00000000\twaincsscan\AnyCPU\Debug\" | find /V "File(s)"
xcopy "%~dp0%..\twaincs\source\twaincsscan\source\bin\AnyCPU\Debug\twaincsscan.exe"           "twain-cs_00000000\twaincsscan\AnyCPU\Debug\" | find /V "File(s)"
xcopy "%~dp0%..\twaincs\source\twaincsscan\source\bin\AnyCPU\Debug\twaincsscan.pdb"           "twain-cs_00000000\twaincsscan\AnyCPU\Debug\" | find /V "File(s)"
xcopy "%~dp0%..\twaincs\source\twaincsscan\source\bin\AnyCPU\Debug\twaincsscan.exe.config"    "twain-cs_00000000\twaincsscan\AnyCPU\Debug\" | find /V "File(s)"
::
xcopy "%~dp0%..\twaincs\source\twaincsscan\source\bin\AnyCPU\Release\TWAIN.dll"               "twain-cs_00000000\twaincsscan\AnyCPU\Release\" | find /V "File(s)"
xcopy "%~dp0%..\twaincs\source\twaincsscan\source\bin\AnyCPU\Release\TWAIN.pdb"               "twain-cs_00000000\twaincsscan\AnyCPU\Release\" | find /V "File(s)"
xcopy "%~dp0%..\twaincs\source\twaincsscan\source\bin\AnyCPU\Release\twaincsscan.exe"         "twain-cs_00000000\twaincsscan\AnyCPU\Release\" | find /V "File(s)"
xcopy "%~dp0%..\twaincs\source\twaincsscan\source\bin\AnyCPU\Release\twaincsscan.pdb"         "twain-cs_00000000\twaincsscan\AnyCPU\Release\" | find /V "File(s)"
xcopy "%~dp0%..\twaincs\source\twaincsscan\source\bin\AnyCPU\Release\twaincsscan.exe.config"  "twain-cs_00000000\twaincsscan\AnyCPU\Release\" | find /V "File(s)"
::
xcopy "%~dp0%..\twaincs\source\twaincsscan\source\bin\x64\Debug\TWAIN.dll"                    "twain-cs_00000000\twaincsscan\x64\Debug\" | find /V "File(s)"
xcopy "%~dp0%..\twaincs\source\twaincsscan\source\bin\x64\Debug\TWAIN.pdb"                    "twain-cs_00000000\twaincsscan\x64\Debug\" | find /V "File(s)"
xcopy "%~dp0%..\twaincs\source\twaincsscan\source\bin\x64\Debug\twaincsscan.exe"              "twain-cs_00000000\twaincsscan\x64\Debug\" | find /V "File(s)"
xcopy "%~dp0%..\twaincs\source\twaincsscan\source\bin\x64\Debug\twaincsscan.pdb"              "twain-cs_00000000\twaincsscan\x64\Debug\" | find /V "File(s)"
xcopy "%~dp0%..\twaincs\source\twaincsscan\source\bin\x64\Debug\twaincsscan.exe.config"       "twain-cs_00000000\twaincsscan\x64\Debug\" | find /V "File(s)"
::
xcopy "%~dp0%..\twaincs\source\twaincsscan\source\bin\x64\Release\TWAIN.dll"                  "twain-cs_00000000\twaincsscan\x64\Release\" | find /V "File(s)"
xcopy "%~dp0%..\twaincs\source\twaincsscan\source\bin\x64\Release\TWAIN.pdb"                  "twain-cs_00000000\twaincsscan\x64\Release\" | find /V "File(s)"
xcopy "%~dp0%..\twaincs\source\twaincsscan\source\bin\x64\Release\twaincsscan.exe"            "twain-cs_00000000\twaincsscan\x64\Release\" | find /V "File(s)"
xcopy "%~dp0%..\twaincs\source\twaincsscan\source\bin\x64\Release\twaincsscan.pdb"            "twain-cs_00000000\twaincsscan\x64\Release\" | find /V "File(s)"
xcopy "%~dp0%..\twaincs\source\twaincsscan\source\bin\x64\Release\twaincsscan.exe.config"     "twain-cs_00000000\twaincsscan\x64\Release\" | find /V "File(s)"
::
xcopy "%~dp0%..\twaincs\source\twaincsscan\source\bin\x86\Debug\TWAIN.dll"                    "twain-cs_00000000\twaincsscan\x86\Debug\" | find /V "File(s)"
xcopy "%~dp0%..\twaincs\source\twaincsscan\source\bin\x86\Debug\TWAIN.pdb"                    "twain-cs_00000000\twaincsscan\x86\Debug\" | find /V "File(s)"
xcopy "%~dp0%..\twaincs\source\twaincsscan\source\bin\x86\Debug\twaincsscan.exe"              "twain-cs_00000000\twaincsscan\x86\Debug\" | find /V "File(s)"
xcopy "%~dp0%..\twaincs\source\twaincsscan\source\bin\x86\Debug\twaincsscan.pdb"              "twain-cs_00000000\twaincsscan\x86\Debug\" | find /V "File(s)"
xcopy "%~dp0%..\twaincs\source\twaincsscan\source\bin\x86\Debug\twaincsscan.exe.config"       "twain-cs_00000000\twaincsscan\x86\Debug\" | find /V "File(s)"
::
xcopy "%~dp0%..\twaincs\source\twaincsscan\source\bin\x86\Release\TWAIN.dll"                  "twain-cs_00000000\twaincsscan\x86\Release\" | find /V "File(s)"
xcopy "%~dp0%..\twaincs\source\twaincsscan\source\bin\x86\Release\TWAIN.pdb"                  "twain-cs_00000000\twaincsscan\x86\Release\" | find /V "File(s)"
xcopy "%~dp0%..\twaincs\source\twaincsscan\source\bin\x86\Release\twaincsscan.exe"            "twain-cs_00000000\twaincsscan\x86\Release\" | find /V "File(s)"
xcopy "%~dp0%..\twaincs\source\twaincsscan\source\bin\x86\Release\twaincsscan.pdb"            "twain-cs_00000000\twaincsscan\x86\Release\" | find /V "File(s)"
xcopy "%~dp0%..\twaincs\source\twaincsscan\source\bin\x86\Release\twaincsscan.exe.config"     "twain-cs_00000000\twaincsscan\x86\Release\" | find /V "File(s)"
::
echo.
echo *** Copying TWAIN CS Test files to the twain-cs_00000000 folder...
xcopy "%~dp0%..\twaincs\source\twaincstst\source\bin\AnyCPU\Debug\TWAIN.dll"                  "twain-cs_00000000\twaincstst\AnyCPU\Debug\" | find /V "File(s)"
xcopy "%~dp0%..\twaincs\source\twaincstst\source\bin\AnyCPU\Debug\TWAIN.pdb"                  "twain-cs_00000000\twaincstst\AnyCPU\Debug\" | find /V "File(s)"
xcopy "%~dp0%..\twaincs\source\twaincstst\source\bin\AnyCPU\Debug\twaincstst.exe"             "twain-cs_00000000\twaincstst\AnyCPU\Debug\" | find /V "File(s)"
xcopy "%~dp0%..\twaincs\source\twaincstst\source\bin\AnyCPU\Debug\twaincstst.pdb"             "twain-cs_00000000\twaincstst\AnyCPU\Debug\" | find /V "File(s)"
xcopy "%~dp0%..\twaincs\source\twaincstst\source\bin\AnyCPU\Debug\twaincstst.exe.config"      "twain-cs_00000000\twaincstst\AnyCPU\Debug\" | find /V "File(s)"
::
xcopy "%~dp0%..\twaincs\source\twaincstst\source\bin\AnyCPU\Release\TWAIN.dll"                "twain-cs_00000000\twaincstst\AnyCPU\Release\" | find /V "File(s)"
xcopy "%~dp0%..\twaincs\source\twaincstst\source\bin\AnyCPU\Release\TWAIN.pdb"                "twain-cs_00000000\twaincstst\AnyCPU\Release\" | find /V "File(s)"
xcopy "%~dp0%..\twaincs\source\twaincstst\source\bin\AnyCPU\Release\twaincstst.exe"           "twain-cs_00000000\twaincstst\AnyCPU\Release\" | find /V "File(s)"
xcopy "%~dp0%..\twaincs\source\twaincstst\source\bin\AnyCPU\Release\twaincstst.pdb"           "twain-cs_00000000\twaincstst\AnyCPU\Release\" | find /V "File(s)"
xcopy "%~dp0%..\twaincs\source\twaincstst\source\bin\AnyCPU\Release\twaincstst.exe.config"    "twain-cs_00000000\twaincstst\AnyCPU\Release\" | find /V "File(s)"
::
xcopy "%~dp0%..\twaincs\source\twaincstst\source\bin\x64\Debug\TWAIN.dll"                     "twain-cs_00000000\twaincstst\x64\Debug\" | find /V "File(s)"
xcopy "%~dp0%..\twaincs\source\twaincstst\source\bin\x64\Debug\TWAIN.pdb"                     "twain-cs_00000000\twaincstst\x64\Debug\" | find /V "File(s)"
xcopy "%~dp0%..\twaincs\source\twaincstst\source\bin\x64\Debug\twaincstst.exe"                "twain-cs_00000000\twaincstst\x64\Debug\" | find /V "File(s)"
xcopy "%~dp0%..\twaincs\source\twaincstst\source\bin\x64\Debug\twaincstst.pdb"                "twain-cs_00000000\twaincstst\x64\Debug\" | find /V "File(s)"
xcopy "%~dp0%..\twaincs\source\twaincstst\source\bin\x64\Debug\twaincstst.exe.config"         "twain-cs_00000000\twaincstst\x64\Debug\" | find /V "File(s)"
::
xcopy "%~dp0%..\twaincs\source\twaincstst\source\bin\x64\Release\TWAIN.dll"                   "twain-cs_00000000\twaincstst\x64\Release\" | find /V "File(s)"
xcopy "%~dp0%..\twaincs\source\twaincstst\source\bin\x64\Release\TWAIN.pdb"                   "twain-cs_00000000\twaincstst\x64\Release\" | find /V "File(s)"
xcopy "%~dp0%..\twaincs\source\twaincstst\source\bin\x64\Release\twaincstst.exe"              "twain-cs_00000000\twaincstst\x64\Release\" | find /V "File(s)"
xcopy "%~dp0%..\twaincs\source\twaincstst\source\bin\x64\Release\twaincstst.pdb"              "twain-cs_00000000\twaincstst\x64\Release\" | find /V "File(s)"
xcopy "%~dp0%..\twaincs\source\twaincstst\source\bin\x64\Release\twaincstst.exe.config"       "twain-cs_00000000\twaincstst\x64\Release\" | find /V "File(s)"
::
xcopy "%~dp0%..\twaincs\source\twaincstst\source\bin\x86\Debug\TWAIN.dll"                     "twain-cs_00000000\twaincstst\x86\Debug\" | find /V "File(s)"
xcopy "%~dp0%..\twaincs\source\twaincstst\source\bin\x86\Debug\TWAIN.pdb"                     "twain-cs_00000000\twaincstst\x86\Debug\" | find /V "File(s)"
xcopy "%~dp0%..\twaincs\source\twaincstst\source\bin\x86\Debug\twaincstst.exe"                "twain-cs_00000000\twaincstst\x86\Debug\" | find /V "File(s)"
xcopy "%~dp0%..\twaincs\source\twaincstst\source\bin\x86\Debug\twaincstst.pdb"                "twain-cs_00000000\twaincstst\x86\Debug\" | find /V "File(s)"
xcopy "%~dp0%..\twaincs\source\twaincstst\source\bin\x86\Debug\twaincstst.exe.config"         "twain-cs_00000000\twaincstst\x86\Debug\" | find /V "File(s)"
::
xcopy "%~dp0%..\twaincs\source\twaincstst\source\bin\x86\Release\TWAIN.dll"                   "twain-cs_00000000\twaincstst\x86\Release\" | find /V "File(s)"
xcopy "%~dp0%..\twaincs\source\twaincstst\source\bin\x86\Release\TWAIN.pdb"                   "twain-cs_00000000\twaincstst\x86\Release\" | find /V "File(s)"
xcopy "%~dp0%..\twaincs\source\twaincstst\source\bin\x86\Release\twaincstst.exe"              "twain-cs_00000000\twaincstst\x86\Release\" | find /V "File(s)"
xcopy "%~dp0%..\twaincs\source\twaincstst\source\bin\x86\Release\twaincstst.pdb"              "twain-cs_00000000\twaincstst\x86\Release\" | find /V "File(s)"
xcopy "%~dp0%..\twaincs\source\twaincstst\source\bin\x86\Release\twaincstst.exe.config"       "twain-cs_00000000\twaincstst\x86\Release\" | find /V "File(s)"


::
:: All done...
::
echo.
echo *** All done, be sure to rename the twain-cs_00000000 folder before committing...
goto:EOF
