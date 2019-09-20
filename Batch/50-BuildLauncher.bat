
@echo off

IF EXIST D:\Program Files (x86)\Microsoft Visual Studio\2019\Enterprise\MSBuild\Current\Bin\msbuild.exe (
    CALL :rebuild "D:\Program Files (x86)\Microsoft Visual Studio\2019\Enterprise\MSBuild\Current\Bin\"
    GOTO:eof
)


CALL :error "Could not find Visual Studio directory."


:error
    echo --- rebuild FAILED: %1
    GOTO:eof

:rebuild
    chdir /d D:\
    chdir %1
    GOTO:rebuild_1

:rebuild_1
	title rebuild 5-Blood Launcher at D:\Documents\GitHub\Source\Repository\WPF\Blood\Launcher
	
	echo  -- Blood LAUNCHER
	msbuild.exe "D:\Documents\GitHub\Source\Repository\WPF\Blood\Launcher\Blood Launcher.sln" -t:rebuild -verbosity:minimal -nologo
	
    GOTO:rebuild_2

:rebuild_2
	rem start "D:\Documents\GitHub\Source\Repository\WPF\Blood\Batch\6-BloodBuildModules.bat"
	
pause    

