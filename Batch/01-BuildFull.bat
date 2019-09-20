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
	title rebuild Blood 
	echo Blood 
	 
	rem "do not change this order"

    echo Blood SERVER MODELS 
	echo --- Blood SERVER MODELS INFRASTRUCTURE . . .
    msbuild.exe "D:\Documents\GitHub\Source\Repository\WPF\Blood\Server\Models\Infrastructure\Blood Server Models Infrastructure.sln" -t:rebuild -verbosity:minimal -nologo
	
	echo --- Blood SERVER MODELS COMPONENT . . .
    msbuild.exe "D:\Documents\GitHub\Source\Repository\WPF\Blood\Server\Models\Component\Blood Server Models Component.sln" -t:rebuild -verbosity:minimal -nologo
	

	echo Blood SERVER CONTEXT
	echo --- Blood SERVER CONTEXT COMPONENT . . .
    msbuild.exe "D:\Documents\GitHub\Source\Repository\WPF\Blood\Server\Context\Component\Blood Server Context Component.sln" -t:rebuild -verbosity:minimal -nologo


	echo Blood SHARED
	echo --- Blood SHARED RESOURCE . . .
	msbuild.exe "D:\Documents\GitHub\Source\Repository\WPF\Blood\Shared\Resources\Blood Shared Resources.sln" -t:rebuild -verbosity:minimal -nologo

	echo --- Blood SHARED TYPES . . .
	msbuild.exe "D:\Documents\GitHub\Source\Repository\WPF\Blood\Shared\Types\Blood Shared Types.sln" -t:rebuild -verbosity:minimal -nologo
	
	echo --- Blood SHARED MESSAGE . . .
	msbuild.exe "D:\Documents\GitHub\Source\Repository\WPF\Blood\Shared\Message\Blood Shared Message.sln" -t:rebuild -verbosity:minimal -nologo

	echo --- Blood SHARED VIEWMODEL . . .
	msbuild.exe "D:\Documents\GitHub\Source\Repository\WPF\Blood\Shared\ViewModel\Blood Shared ViewModel.sln" -t:rebuild -verbosity:minimal -nologo
	
	echo --- COMMUNICATION . . .
	msbuild.exe "D:\Documents\GitHub\Source\Repository\WPF\Blood\Shared\Communication\Blood Shared Communication.sln" -t:rebuild -verbosity:minimal -nologo
		
	echo --- Blood SHARED SERVICES . . .
	msbuild.exe "D:\Documents\GitHub\Source\Repository\WPF\Blood\Shared\Services\Blood Shared Services.sln" -t:rebuild -verbosity:minimal -nologo
	   
	
	
	echo Blood SERVER SERVICES
	echo --- Blood SERVER SERVICES COMPONENT . . .
    msbuild.exe "D:\Documents\GitHub\Source\Repository\WPF\Blood\Server\Services\Component\Blood Server Services Component.sln" -t:rebuild -verbosity:minimal -nologo
	
	
	echo  -- Blood LAUNCHER
	msbuild.exe "D:\Documents\GitHub\Source\Repository\WPF\Blood\Launcher\Blood Launcher.sln" -t:rebuild -verbosity:minimal -nologo
	
	
	echo  -- Blood MODULES
	echo --- Blood MODULES SETTINGS . . .
	msbuild.exe "D:\Documents\GitHub\Source\Repository\WPF\Blood\Module\Settings\Blood Module Settings.sln" -t:rebuild -verbosity:minimal -nologo

	
	
	

	echo --- Blood BUILD DONE
	
pause
