@echo off

	echo ---- DELETING...
	del "D:\Documents\GitHub\Source\Repository\WPF\Blood\Bin\rr*.dll" 
	
	echo ---- COPYING...
	
	echo ---- XDMessaging.Lite...
	copy "D:\Documents\Visual Studio\Projects\Library\Bin\XDMessaging.Lite.dll"  "D:\Documents\GitHub\Source\Repository\WPF\Blood\Bin" /V
	
	echo ---- Communication...
	copy "D:\Documents\Visual Studio\Projects\Library\Bin\rr.Library.Communication.dll"  "D:\Documents\GitHub\Source\Repository\WPF\Blood\Bin" /V
	
	echo ---- Controls...
	copy "D:\Documents\Visual Studio\Projects\Library\Bin\rr.Library.Controls.dll"  "D:\Documents\GitHub\Source\Repository\WPF\Blood\Bin" /V
	
	echo ---- Helper...
	copy "D:\Documents\Visual Studio\Projects\Library\Bin\rr.Library.Helper.dll"  "D:\Documents\GitHub\Source\Repository\WPF\Blood\Bin" /V
		
	echo ---- Infrastructure...
	copy "D:\Documents\Visual Studio\Projects\Library\Bin\rr.Library.Infrastructure.dll"  "D:\Documents\GitHub\Source\Repository\WPF\Blood\Bin" /V
	
	echo ---- Message...
	copy "D:\Documents\Visual Studio\Projects\Library\Bin\rr.Library.Message.dll"  "D:\Documents\GitHub\Source\Repository\WPF\Blood\Bin" /V
	
	echo ---- Services...
	copy "D:\Documents\Visual Studio\Projects\Library\Bin\rr.Library.Services.dll"  "D:\Documents\GitHub\Source\Repository\WPF\Blood\Bin" /V
	
	echo ---- Types...
	copy "D:\Documents\Visual Studio\Projects\Library\Bin\rr.Library.Types.dll"  "D:\Documents\GitHub\Source\Repository\WPF\Blood\Bin" /V
	
	
	
	
	
	pause