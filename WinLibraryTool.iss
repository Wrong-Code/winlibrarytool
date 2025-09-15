[Setup]
AppName=Win Library Tool
AppVersion=1.1.0
DefaultDirName={autopf}\Win Library Tool
DefaultGroupName=Win Library Tool
OutputDir=Installer
OutputBaseFilename=WinLibraryToolSetup
SetupIconFile=WinLibraryTool\app.ico
UninstallDisplayIcon={app}\WinLibraryTool.exe
Compression=lzma
SolidCompression=yes
ArchitecturesInstallIn64BitMode=x64compatible

[Files]
Source: "WinLibraryTool\bin\Release\WinLibraryTool.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "WinLibraryTool\bin\Release\*.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "WinLibraryTool\bin\Release\*.config"; DestDir: "{app}"; Flags: ignoreversion

[Icons]
Name: "{group}\Win Library Tool"; Filename: "{app}\WinLibraryTool.exe"; WorkingDir: "{app}"
Name: "{group}\Uninstall Win Library Tool"; Filename: "{uninstallexe}"

[Run]
Filename: "{app}\WinLibraryTool.exe"; Description: "Launch Win Library Tool"; Flags: nowait postinstall skipifsilent

[UninstallDelete]
Type: files; Name: "{app}\*.*"
