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

	echo ---- DELETING...
	del "D:\Documents\GitHub\Source\Repository\WPF\Blood\Bin\Blood*.dll" 
	
    echo Blood SERVER MODELS 
	echo --- Blood SERVER MODELS INFRASTRUCTURE . . .
    msbuild.exe "D:\Documents\GitHub\Source\Repository\WPF\Blood\Server\Models\Infrastructure\Blood Server Models Infrastructure.sln" -t:rebuild -verbosity:minimal -nologo
	
	echo --- Blood SERVER MODELS COMPONENT . . .
    msbuild.exe "D:\Documents\GitHub\Source\Repository\WPF\Blood\Server\Models\Component\Blood Server Models Component.sln" -t:rebuild -verbosity:minimal -nologo
	
	echo --- Blood SERVER MODELS ACTION . . .
    msbuild.exe "D:\Documents\GitHub\Source\Repository\WPF\Blood\Server\Models\Action\Blood Server Models Action.sln" -t:rebuild -verbosity:minimal -nologo


	echo Blood SERVER CONTEXT
	echo --- Blood SERVER CONTEXT COMPONENT . . .
    msbuild.exe "D:\Documents\GitHub\Source\Repository\WPF\Blood\Server\Context\Component\Blood Server Context Component.sln" -t:rebuild -verbosity:minimal -nologo


	echo Blood SERVER SERVICES
	echo --- Blood SERVER SERVICES COMPONENT . . .
    msbuild.exe "D:\Documents\GitHub\Source\Repository\WPF\Blood\Server\Services\Component\Blood Server Services Component.sln" -t:rebuild -verbosity:minimal -nologo


	echo Blood SHARED
	echo --- Blood SHARED MODELS GADGET . . .
    msbuild.exe "D:\Documents\GitHub\Source\Repository\WPF\Blood\Shared\Models\Gadget\Blood Shared Models Gadget.sln" -t:rebuild -verbosity:minimal -nologo
	
	echo --- Blood SHARED MODELS ACTION . . .
    msbuild.exe "D:\Documents\GitHub\Source\Repository\WPF\Blood\Shared\Models\Action\Blood Shared Models Action.sln" -t:rebuild -verbosity:minimal -nologo
	
	echo --- Blood SHARED RESOURCE . . .
	msbuild.exe "D:\Documents\GitHub\Source\Repository\WPF\Blood\Shared\Resources\Blood Shared Resources.sln" -t:rebuild -verbosity:minimal -nologo

	echo --- Blood SHARED TYPES . . .
	msbuild.exe "D:\Documents\GitHub\Source\Repository\WPF\Blood\Shared\Types\Blood Shared Types.sln" -t:rebuild -verbosity:minimal -nologo
	
	echo --- Blood SHARED MESSAGE . . .
	msbuild.exe "D:\Documents\GitHub\Source\Repository\WPF\Blood\Shared\Message\Blood Shared Message.sln" -t:rebuild -verbosity:minimal -nologo

	echo --- Blood SHARED COMMUNICATION . . .
	msbuild.exe "D:\Documents\GitHub\Source\Repository\WPF\Blood\Shared\Communication\Blood Shared Communication.sln" -t:rebuild -verbosity:minimal -nologo
	
	echo --- Blood SHARED VIEWMODEL . . .
	msbuild.exe "D:\Documents\GitHub\Source\Repository\WPF\Blood\Shared\ViewModel\Blood Shared ViewModel.sln" -t:rebuild -verbosity:minimal -nologo
	
	echo --- Blood SHARED SERVICES . . .
	msbuild.exe "D:\Documents\GitHub\Source\Repository\WPF\Blood\Shared\Services\Blood Shared Services.sln" -t:rebuild -verbosity:minimal -nologo

	
	echo Blood SHARED GADGET MEDICAL TEST
	echo --- Blood SHARED GADGET MATERIAL . . .
	msbuild.exe "D:\Documents\GitHub\Source\Repository\WPF\Blood\Shared\MedicalTest\Material\Blood Shared Gadget Material.sln" -t:rebuild -verbosity:minimal -nologo
	
	echo --- Blood SHARED GADGET TARGET . . .
	msbuild.exe "D:\Documents\GitHub\Source\Repository\WPF\Blood\Shared\MedicalTest\Target\Blood Shared Gadget Target.sln" -t:rebuild -verbosity:minimal -nologo
	
	echo --- Blood SHARED GADGET TEST . . .
	msbuild.exe "D:\Documents\GitHub\Source\Repository\WPF\Blood\Shared\MedicalTest\Test\Blood Shared Gadget Test.sln" -t:rebuild -verbosity:minimal -nologo
	
	
	echo Blood SHARED GADGET MEDICAL CARE
	echo --- Blood SHARED GADGET REGISTRATION . . .
	msbuild.exe "D:\Documents\GitHub\Source\Repository\WPF\Blood\Shared\MedicalCare\Registration\Blood Shared Gadget Registration.sln" -t:rebuild -verbosity:minimal -nologo
	
	echo --- Blood SHARED GADGET TESTS . . .
	msbuild.exe "D:\Documents\GitHub\Source\Repository\WPF\Blood\Shared\MedicalCare\Tests\Blood Shared Gadget Tests.sln" -t:rebuild -verbosity:minimal -nologo
	
	echo --- Blood SHARED GADGET REPORT . . .
	msbuild.exe "D:\Documents\GitHub\Source\Repository\WPF\Blood\Shared\MedicalCare\Report\Blood Shared Gadget Report.sln" -t:rebuild -verbosity:minimal -nologo
	
	
	echo  -- Blood LAUNCHER
	msbuild.exe "D:\Documents\GitHub\Source\Repository\WPF\Blood\Launcher\Blood Launcher.sln" -t:rebuild -verbosity:minimal -nologo
	
	
	echo  -- Blood MODULES
	echo --- Blood MODULES SETTINGS . . .
	msbuild.exe "D:\Documents\GitHub\Source\Repository\WPF\Blood\Module\Settings\Blood Module Settings.sln" -t:rebuild -verbosity:minimal -nologo

	
	echo  -- Blood GADGET MEDICAL TEST
	echo --- Blood GADGET MATERIAL . . .
	msbuild.exe "D:\Documents\GitHub\Source\Repository\WPF\Blood\Gadget\MedicalTest\Material\Blood Gadget Material.sln" -t:rebuild -verbosity:minimal -nologo
	
	echo --- Blood GADGET TARGET . . .
	msbuild.exe "D:\Documents\GitHub\Source\Repository\WPF\Blood\Gadget\MedicalTest\Target\Blood Gadget Target.sln" -t:rebuild -verbosity:minimal -nologo
	
	echo --- Blood GADGET TEST . . .
	msbuild.exe "D:\Documents\GitHub\Source\Repository\WPF\Blood\Gadget\MedicalTest\Test\Blood Gadget Test.sln" -t:rebuild -verbosity:minimal -nologo
	
	
	echo  -- Blood GADGET MEDICAL CARE
	echo --- Blood GADGET REGISTRATION . . .
	msbuild.exe "D:\Documents\GitHub\Source\Repository\WPF\Blood\Gadget\MedicalCare\Registration\Blood Gadget Registration.sln" -t:rebuild -verbosity:minimal -nologo
	
	echo --- Blood GADGET TESTS . . .
	msbuild.exe "D:\Documents\GitHub\Source\Repository\WPF\Blood\Gadget\MedicalCare\Tests\Blood Gadget Tests.sln" -t:rebuild -verbosity:minimal -nologo
	
	echo --- Blood GADGET REPORT . . .
	msbuild.exe "D:\Documents\GitHub\Source\Repository\WPF\Blood\Gadget\MedicalCare\Report\Blood Gadget Report.sln" -t:rebuild -verbosity:minimal -nologo


	echo --- Blood BUILD DONE
	
pause
