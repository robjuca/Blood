@echo off

	echo ---- DELETING...
	del "C:\Users\Roberto\Documents\GitHub\Source\Repository\WPF\Blood\Bin\rr*.dll" 
	
	echo ---- COPYING...
	
	echo ---- XDMessaging.Lite...
	copy "C:\Users\Roberto\Documents\Visual Studio\Projects\Library\Bin\XDMessaging.Lite.dll"  "C:\Users\Roberto\Documents\GitHub\Source\Repository\WPF\Blood\Bin" /V
	
	echo ---- MadMilkman.Ini...
	copy "C:\Users\Roberto\Documents\Visual Studio\Projects\Library\Bin\MadMilkman.Ini.dll"  "C:\Users\Roberto\Documents\GitHub\Source\Repository\WPF\Blood\Bin" /V
	
	
	
	
	echo ---- Communication...
	copy "C:\Users\Roberto\Documents\Visual Studio\Projects\Library\Bin\rr.Library.Communication.dll"  "C:\Users\Roberto\Documents\GitHub\Source\Repository\WPF\Blood\Bin" /V
	
	echo ---- Controls...
	copy "C:\Users\Roberto\Documents\Visual Studio\Projects\Library\Bin\rr.Library.Controls.dll"  "C:\Users\Roberto\Documents\GitHub\Source\Repository\WPF\Blood\Bin" /V
	
	echo ---- Converter...
	copy "C:\Users\Roberto\Documents\Visual Studio\Projects\Library\Bin\rr.Library.Converter.dll"  "C:\Users\Roberto\Documents\GitHub\Source\Repository\WPF\Blood\Bin" /V
	
	echo ---- Helper...
	copy "C:\Users\Roberto\Documents\Visual Studio\Projects\Library\Bin\rr.Library.Helper.dll"  "C:\Users\Roberto\Documents\GitHub\Source\Repository\WPF\Blood\Bin" /V
		
	echo ---- Infrastructure...
	copy "C:\Users\Roberto\Documents\Visual Studio\Projects\Library\Bin\rr.Library.Infrastructure.dll"  "C:\Users\Roberto\Documents\GitHub\Source\Repository\WPF\Blood\Bin" /V
	
	echo ---- Message...
	copy "C:\Users\Roberto\Documents\Visual Studio\Projects\Library\Bin\rr.Library.Message.dll"  "C:\Users\Roberto\Documents\GitHub\Source\Repository\WPF\Blood\Bin" /V
	
	echo ---- Services...
	copy "C:\Users\Roberto\Documents\Visual Studio\Projects\Library\Bin\rr.Library.Services.dll"  "C:\Users\Roberto\Documents\GitHub\Source\Repository\WPF\Blood\Bin" /V
	
	echo ---- Types...
	copy "C:\Users\Roberto\Documents\Visual Studio\Projects\Library\Bin\rr.Library.Types.dll"  "C:\Users\Roberto\Documents\GitHub\Source\Repository\WPF\Blood\Bin" /V
	
	
	
	
	pause