@echo on

:: Project post-build event for Windows

set MSFS_SDK_DIR="C:\MSFS SDK\SimConnect SDK\lib"
set PROJECT_DIR=%~1
set TARGET_DIR=%~2

if "%PROJECT_DIR%" == "" (
    echo %~0: Error: Project directory was not given
    exit /b 1
)

if "%TARGET_DIR%" == "" (
    echo %~0: Error: Target directory was not given
    exit /b 1
)

if not exist "%TARGET_DIR%" (
    echo %~0: Error: Target directory does not exist: '%TARGET_DIR%'"
    exit /b 1
)

echo "%MSFS_SDK_DIR%\SimConnect.dll"
echo "%TARGET_DIR%\"
xcopy /y "%MSFS_SDK_DIR%\SimConnect.dll" "%TARGET_DIR%"
