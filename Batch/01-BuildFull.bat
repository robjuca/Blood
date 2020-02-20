@echo off

IF EXIST C:\Program Files (x86)\Microsoft Visual Studio\2019\Enterprise\MSBuild\Current\Bin\msbuild.exe (
    CALL :rebuild "C:\Program Files (x86)\Microsoft Visual Studio\2019\Enterprise\MSBuild\Current\Bin\"
    GOTO:eof
)


CALL :error "Could not find Visual Studio directory."


:error
    echo --- rebuild FAILED: %1
    GOTO:eof

:rebuild
    chdir /d C:\
    chdir %1
    GOTO:rebuild_1

:rebuild_1
	title rebuild Blood 
	echo Blood 
	 
	rem "do not change this order"

	echo ---- DELETING...
	del "C:\Users\Roberto\Documents\GitHub\Source\Repository\WPF\Blood\Bin\Blood*.dll" 
	del "C:\Users\Roberto\Documents\GitHub\Source\Repository\WPF\Blood\Bin\Blood*.exe" 
	
    echo Blood SERVER MODELS 
	echo --- Blood SERVER MODELS INFRASTRUCTURE . . .
    msbuild.exe "C:\Users\Roberto\Documents\GitHub\Source\Repository\WPF\Blood\Server\Models\Infrastructure\Blood Server Models Infrastructure.sln" -t:rebuild -verbosity:minimal -nologo
	
	echo --- Blood SERVER MODELS COMPONENT . . .
    msbuild.exe "C:\Users\Roberto\Documents\GitHub\Source\Repository\WPF\Blood\Server\Models\Component\Blood Server Models Component.sln" -t:rebuild -verbosity:minimal -nologo
	
	echo --- Blood SERVER MODELS ACTION . . .
    msbuild.exe "C:\Users\Roberto\Documents\GitHub\Source\Repository\WPF\Blood\Server\Models\Action\Blood Server Models Action.sln" -t:rebuild -verbosity:minimal -nologo


	echo Blood SERVER CONTEXT
	echo --- Blood SERVER CONTEXT COMPONENT . . .
    msbuild.exe "C:\Users\Roberto\Documents\GitHub\Source\Repository\WPF\Blood\Server\Context\Component\Blood Server Context Component.sln" -t:rebuild -verbosity:minimal -nologo


	echo Blood SERVER SERVICES
	echo --- Blood SERVER SERVICES COMPONENT . . .
    msbuild.exe "C:\Users\Roberto\Documents\GitHub\Source\Repository\WPF\Blood\Server\Services\Component\Blood Server Services Component.sln" -t:rebuild -verbosity:minimal -nologo


	echo Blood SHARED
	echo --- Blood SHARED GADGET MODELS COMPONENT . . .
    msbuild.exe "C:\Users\Roberto\Documents\GitHub\Source\Repository\WPF\Blood\Shared\Gadget\Models\Component\Blood Shared Gadget Models Component.sln" -t:rebuild -verbosity:minimal -nologo
	
	echo --- Blood SHARED GADGET MODELS ACTION . . .
    msbuild.exe "C:\Users\Roberto\Documents\GitHub\Source\Repository\WPF\Blood\Shared\Gadget\Models\Action\Blood Shared Gadget Models Action.sln" -t:rebuild -verbosity:minimal -nologo
	
	echo --- Blood SHARED RESOURCE . . .
	msbuild.exe "C:\Users\Roberto\Documents\GitHub\Source\Repository\WPF\Blood\Shared\Resources\Blood Shared Resources.sln" -t:rebuild -verbosity:minimal -nologo

	echo --- Blood SHARED TYPES . . .
	msbuild.exe "C:\Users\Roberto\Documents\GitHub\Source\Repository\WPF\Blood\Shared\Types\Blood Shared Types.sln" -t:rebuild -verbosity:minimal -nologo
	
	echo --- Blood SHARED MESSAGE . . .
	msbuild.exe "C:\Users\Roberto\Documents\GitHub\Source\Repository\WPF\Blood\Shared\Message\Blood Shared Message.sln" -t:rebuild -verbosity:minimal -nologo

	echo --- Blood SHARED COMMUNICATION . . .
	msbuild.exe "C:\Users\Roberto\Documents\GitHub\Source\Repository\WPF\Blood\Shared\Communication\Blood Shared Communication.sln" -t:rebuild -verbosity:minimal -nologo
	
	echo --- Blood SHARED VIEWMODEL . . .
	msbuild.exe "C:\Users\Roberto\Documents\GitHub\Source\Repository\WPF\Blood\Shared\ViewModel\Blood Shared ViewModel.sln" -t:rebuild -verbosity:minimal -nologo
	
	echo --- Blood SHARED SERVICES . . .
	msbuild.exe "C:\Users\Roberto\Documents\GitHub\Source\Repository\WPF\Blood\Shared\Services\Blood Shared Services.sln" -t:rebuild -verbosity:minimal -nologo

	
	echo Blood SHARED GADGET 
	echo --- Blood SHARED GADGET MATERIAL . . .
	msbuild.exe "C:\Users\Roberto\Documents\GitHub\Source\Repository\WPF\Blood\Shared\Gadget\Material\Blood Shared Gadget Material.sln" -t:rebuild -verbosity:minimal -nologo
		
	echo --- Blood SHARED GADGET TARGET . . .
	msbuild.exe "C:\Users\Roberto\Documents\GitHub\Source\Repository\WPF\Blood\Shared\Gadget\Target\Blood Shared Gadget Target.sln" -t:rebuild -verbosity:minimal -nologo
	
	echo --- Blood SHARED GADGET TEST . . .
	msbuild.exe "C:\Users\Roberto\Documents\GitHub\Source\Repository\WPF\Blood\Shared\Gadget\Test\Blood Shared Gadget Test.sln" -t:rebuild -verbosity:minimal -nologo
	
	echo --- Blood SHARED GADGET REGISTRATION . . .
	msbuild.exe "C:\Users\Roberto\Documents\GitHub\Source\Repository\WPF\Blood\Shared\Gadget\Registration\Blood Shared Gadget Registration.sln" -t:rebuild -verbosity:minimal -nologo
	
	echo --- Blood SHARED GADGET RESULT . . .
	msbuild.exe "C:\Users\Roberto\Documents\GitHub\Source\Repository\WPF\Blood\Shared\Gadget\Result\Blood Shared Gadget Result.sln" -t:rebuild -verbosity:minimal -nologo
	
	echo --- Blood SHARED GADGET REPORT . . .
	msbuild.exe "C:\Users\Roberto\Documents\GitHub\Source\Repository\WPF\Blood\Shared\Gadget\Report\Blood Shared Gadget Report.sln" -t:rebuild -verbosity:minimal -nologo
	
	
	echo  -- Blood LAUNCHER
	msbuild.exe "C:\Users\Roberto\Documents\GitHub\Source\Repository\WPF\Blood\Launcher\Blood Launcher.sln" -t:rebuild -verbosity:minimal -nologo
	
	
	echo  -- Blood MODULES
	echo --- Blood MODULES SETTINGS . . .
	msbuild.exe "C:\Users\Roberto\Documents\GitHub\Source\Repository\WPF\Blood\Module\Settings\Blood Module Settings.sln" -t:rebuild -verbosity:minimal -nologo

	
	echo  -- Blood GADGET 
	echo --- Blood GADGET MATERIAL . . .
	msbuild.exe "C:\Users\Roberto\Documents\GitHub\Source\Repository\WPF\Blood\Gadget\Material\Blood Gadget Material.sln" -t:rebuild -verbosity:minimal -nologo
	
	echo --- Blood GADGET TARGET . . .
	msbuild.exe "C:\Users\Roberto\Documents\GitHub\Source\Repository\WPF\Blood\Gadget\Target\Blood Gadget Target.sln" -t:rebuild -verbosity:minimal -nologo
	
	echo --- Blood GADGET TEST . . .
	msbuild.exe "C:\Users\Roberto\Documents\GitHub\Source\Repository\WPF\Blood\Gadget\Test\Blood Gadget Test.sln" -t:rebuild -verbosity:minimal -nologo
		
	echo --- Blood GADGET REGISTRATION . . .
	msbuild.exe "C:\Users\Roberto\Documents\GitHub\Source\Repository\WPF\Blood\Gadget\Registration\Blood Gadget Registration.sln" -t:rebuild -verbosity:minimal -nologo
	
	echo --- Blood GADGET RESULT . . .
	msbuild.exe "C:\Users\Roberto\Documents\GitHub\Source\Repository\WPF\Blood\Gadget\Result\Blood Gadget Result.sln" -t:rebuild -verbosity:minimal -nologo
	
	echo --- Blood GADGET REPORT . . .
	msbuild.exe "C:\Users\Roberto\Documents\GitHub\Source\Repository\WPF\Blood\Gadget\Report\Blood Gadget Report.sln" -t:rebuild -verbosity:minimal -nologo


	echo --- Blood BUILD DONE
	
pause
