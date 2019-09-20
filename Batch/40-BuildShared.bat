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
	title rebuild 4-Blood Shared at D:\Documents\GitHub\Source\Repository\WPF\Blood\Shared
	echo Blood SHARED
	 
	rem "do not change this order"

	echo --- RESOURCE . . .
	msbuild.exe "D:\Documents\GitHub\Source\Repository\WPF\Blood\Shared\Resources\Blood Shared Resources.sln" -t:rebuild -verbosity:minimal -nologo

	echo --- TYPES . . .
	msbuild.exe "D:\Documents\GitHub\Source\Repository\WPF\Blood\Shared\Types\Blood Shared Types.sln" -t:rebuild -verbosity:minimal -nologo
	
	echo --- MESSAGE . . .
	msbuild.exe "D:\Documents\GitHub\Source\Repository\WPF\Blood\Shared\Message\Blood Shared Message.sln" -t:rebuild -verbosity:minimal -nologo

	echo --- VIEWMODEL . . .
	msbuild.exe "D:\Documents\GitHub\Source\Repository\WPF\Blood\Shared\ViewModel\Blood Shared ViewModel.sln" -t:rebuild -verbosity:minimal -nologo
	
	echo --- COMMUNICATION . . .
	msbuild.exe "D:\Documents\GitHub\Source\Repository\WPF\Blood\Shared\Communication\Blood Shared Communication.sln" -t:rebuild -verbosity:minimal -nologo
	
	echo --- DASHBOARD . . .
	msbuild.exe "D:\Documents\GitHub\Source\Repository\WPF\Blood\Shared\DashBoard\Blood Shared DashBoard.sln" -t:rebuild -verbosity:minimal -nologo
	
	echo --- SERVICES . . .
	msbuild.exe "D:\Documents\GitHub\Source\Repository\WPF\Blood\Shared\Services\Blood Shared Services.sln" -t:rebuild -verbosity:minimal -nologo
	
	   
	echo Blood SHARED GADGET
	echo --- DOCUMENT . . .
	msbuild.exe "D:\Documents\GitHub\Source\Repository\WPF\Blood\Shared\Document\Blood Shared Gadget Document.sln" -t:rebuild -verbosity:minimal -nologo
	
	echo --- IMAGE . . .
	msbuild.exe "D:\Documents\GitHub\Source\Repository\WPF\Blood\Shared\Image\Blood Shared Gadget Image.sln" -t:rebuild -verbosity:minimal -nologo
		
	echo Blood SHARED LAYOUT
	echo --- BAG . . .
    msbuild.exe "D:\Documents\GitHub\Source\Repository\WPF\Blood\Shared\Bag\Blood Shared Layout Bag.sln" -t:rebuild -verbosity:minimal -nologo
	
	echo --- SHELF . . .
	msbuild.exe "D:\Documents\GitHub\Source\Repository\WPF\Blood\Shared\Shelf\Blood Shared Layout Shelf.sln" -t:rebuild -verbosity:minimal -nologo
		
	echo --- DRAWER . . .
	msbuild.exe "D:\Documents\GitHub\Source\Repository\WPF\Blood\Shared\Drawer\Blood Shared Layout Drawer.sln" -t:rebuild -verbosity:minimal -nologo
	
	echo --- CHEST . . .
	msbuild.exe "D:\Documents\GitHub\Source\Repository\WPF\Blood\Shared\Chest\Blood Shared Layout Chest.sln" -t:rebuild -verbosity:minimal -nologo
		
		
	
	GOTO:rebuild_2

:rebuild_2

	rem start "D:\Documents\GitHub\Source\Repository\WPF\Blood\Batch\5-BloodBuildLauncher.bat"	
	
pause    



