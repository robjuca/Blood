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
	title rebuild 1-Blood Server Models at D:\Documents\GitHub\Source\Repository\WPF\Blood\Server\Models
	echo Blood SERVER MODELS 
	 
	rem "do not change this order"

    
	echo --- INFRASTRUCTURE . . .
    msbuild.exe "D:\Documents\GitHub\Source\Repository\WPF\Blood\Server\Models\Infrastructure\Blood Server Models Infrastructure.sln" -t:rebuild -verbosity:minimal -nologo
	
	echo --- COMPONENT . . .
    msbuild.exe "D:\Documents\GitHub\Source\Repository\WPF\Blood\Server\Models\Component\Blood Server Models Component.sln" -t:rebuild -verbosity:minimal -nologo
	

	
    GOTO:rebuild_2

:rebuild_2
	rem start "D:\Documents\GitHub\Source\Repository\WPF\Blood\Batch\2-BloodBuildServerContext.bat"
	
pause
