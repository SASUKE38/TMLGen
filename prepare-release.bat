set config=%1
shift

if %config%==Release (
	mkdir ..\Release\TMLGen

	copy bin\Release\net8.0-windows\*.dll ..\Release\TMLGen\
	copy bin\Release\net8.0-windows\*.json ..\Release\TMLGen\
	copy bin\Release\net8.0-windows\*.config ..\Release\TMLGen\
	copy bin\Release\net8.0-windows\*.exe ..\Release\TMLGen\
	copy bin\Release\net8.0-windows\*.pdb ..\Release\TMLGen\

	copy ..\README.md ..\Release\TMLGen
	copy ..\LICENSE ..\Release\TMLGen
	copy Libraries\LSLib\LICENSE.txt "..\Release\TMLGen\LSLib LICENSE.txt"

	if not "%1"=="" (
		if "%1"=="/o" (
			start ..\Release\
		)
	)

	pause
)