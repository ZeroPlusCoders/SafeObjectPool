@echo off
Rem push the package to the github nuget repo 
set /p invar="Enter path to the solution's folder: "
set /p version="Enter the version for the packages - they need to be the same:  "
echo Deploying SafeObjectPool library packages from %invar% to github
for /F "delims=" %%i in ('gh auth token') do set PAT=%%i
echo PAT = %PAT%
rem dotnet nuget delete Middleware %version% -k %PAT% -s https://nuget.pkg.github.com/ZeroPlusCoders/index.json 
dotnet nuget push -k %PAT% -s https://nuget.pkg.github.com/ZeroPlusCoders/index.json %invar%\bin\Release\SafeObjectPool.%version%.nupkg
